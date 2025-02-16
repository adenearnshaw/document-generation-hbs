﻿using HandlebarsDotNet;
using System.Reflection;

namespace A10w.DocumentGeneration.Handlebars;

/// <summary>
/// Configuration option for Handlebars DocumentGeneration
/// </summary>
public class HandlebarsDocumentGenerationConfiguration
{
    private Dictionary<Type, string> _paths = new();

    private Dictionary<string, string> _partials = new();

    /// <summary>
    /// Register the path to a Partial Template
    /// </summary>
    /// <param name="partialName">Name of partial. Used as {{> partialName}} in hbs file</param>
    /// <param name="filePath">Relative path of partial.hbs file</param>
    /// <returns>Current intance of <see cref="HandlebarsDocumentGenerationConfiguration"/> for fluent chaining</returns>
    /// <exception cref="ArgumentException">Duplicate partials with same name not allowed</exception>
    public HandlebarsDocumentGenerationConfiguration AddPartialTemplate(string partialName, string filePath)
    {
        if (_partials.ContainsKey(partialName))
        {
            throw new ArgumentException("Partial already registered with this name");
        }

        _partials[partialName] = filePath;
        return this;
    }

    /// <summary>
    /// Register the path to the View to associate to a given DocumentData Type
    /// </summary>
    /// <typeparam name="T">Type of Document Data</typeparam>
    /// <param name="path">Relative path to the view.hbs file</param>
    /// <returns>Current intance of <see cref="HandlebarsDocumentGenerationConfiguration"/> for fluent chaining</returns>
    /// <exception cref="ArgumentException">Duplicate views for the same Data Object not allowed</exception>
    public HandlebarsDocumentGenerationConfiguration AddView<T>(string path) where T : IDocumentData
    {
        if (_paths.ContainsKey(typeof(T)))
        {
            throw new ArgumentException("View already registered for this Data Type");
        }

        _paths.Add(typeof(T), path);
        return this;
    }

    internal (IHandlebars Handlebars, Dictionary<Type, string> Paths) Build()
    {
        var fs = new DiskFileSystem();
        var hbs = Hbs.Create(new HandlebarsConfiguration
        {
            FileSystem = fs,
            TextEncoder = new HtmlEncoder()
        });

        RegisterTemplates(hbs, _partials);

        return (hbs, _paths);
    }

    private static IHandlebars RegisterTemplates(IHandlebars hbs, Dictionary<string, string> partials)
    {
        var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        foreach (var template in partials)
        {
            var path = Path.Combine(executableLocation, template.Value);
            var templateContents = File.ReadAllText(path);
            hbs.RegisterTemplate(template.Key, templateContents);
        }

        return hbs;
    }
}