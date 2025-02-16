using System.ComponentModel;

namespace A10w.DocumentGeneration.Handlebars;

/// <summary>
/// <see cref="IDocumentGenerator"/> implementation using Handlebars.NET
/// </summary>
public class HandlebarsDocumentGenerator : IDocumentGenerator
{
    /// <summary>
    /// Generate Document based on Data using Handlebars engine
    /// </summary>
    /// <typeparam name="T"><see cref="IDocumentData"/></typeparam>
    /// <param name="documentData">Data object associated with document</param>
    /// <returns>Full interpolated string representation of the document</returns>
    public Task<string> GenerateDocument<T>(T documentData) where T : IDocumentData
    {
        return GenerateDocument(typeof(T), documentData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataType">Type of <see cref="IDocumentData"/>registered against the View</param>
    /// <param name="documentData">Instance of <see cref="IDocumentData"/></param>
    /// <returns>Full interpolated string representation of the document</returns>
    /// <exception cref="DocumentGenerationException"></exception>
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

    /// <summary>
    /// Prevent construction from outside this library
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal HandlebarsDocumentGenerator()
    {
    }
}

