using System.ComponentModel;

namespace A10w.DocumentGeneration.Handlebars;

/// <summary>
/// <see cref="IDocumentGenerator"/> implementation using Handlebars.NET
/// </summary>
public class HandlebarsDocumentGenerator : IDocumentGenerator
{
    /// <inheritdoc />
    public Task<string> GenerateDocument<T>(T documentData) where T : IDocumentData
    {
        return GenerateDocument(typeof(T), documentData);
    }

    /// <inheritdoc />
    public Task<string> GenerateDocument(Type dataType, object documentData)
    {
        try
        {
            if (!HandlebarsCompiler.IsInstantiated)
            {
                throw new DocumentGenerationException("DocumentGeneration.Handlebars not initialized. Please use services.AddHandlebarsDocumentGeneration() in Program");
            }

            var template = HandlebarsCompiler.Instance.GetCompiler(dataType);

            var output = template(documentData);

            return Task.FromResult(output);
        }
        catch (Exception ex) when (ex is not DocumentGenerationException)
        {
            throw new DocumentGenerationException("GenerateDocument Failed", ex);
        }
    }

    /// <inheritdoc />
    public Task<string> GenerateDocumentFromTemplate<T>(string templatePath, T documentData) where T : IDocumentData
    {
        try
        {
            if (!HandlebarsCompiler.IsInstantiated)
            {
                throw new DocumentGenerationException("DocumentGeneration.Handlebars not initialized. Please use services.AddHandlebarsDocumentGeneration() in Program");
            }

            var template = HandlebarsCompiler.Instance.GetDynamicCompiler(templatePath);

            var output = template(documentData);

            return Task.FromResult(output);
        }
        catch (Exception ex)when (ex is not DocumentGenerationException)
        {
            throw new DocumentGenerationException("GenerateDocument Failed", ex);
        }
    }

    /// <summary>
    /// Prevent construction from outside this library
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal HandlebarsDocumentGenerator()
    {
    }
}

