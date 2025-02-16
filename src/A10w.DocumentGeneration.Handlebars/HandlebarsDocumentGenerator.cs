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
    public async Task<string> GenerateDocument<T>(string templateUrl, T documentData) where T : IDocumentData
    {
        try
        {
            if (!HandlebarsCompiler.IsInstantiated)
            {
                throw new DocumentGenerationException("DocumentGeneration.Handlebars not initialized. Please use services.AddHandlebarsDocumentGeneration() in Program");
            }
            
            var template = await GetTemplateString(templateUrl);
            var hbsTemplate = HandlebarsCompiler.Instance.GetCompiler(template);

            var output = hbsTemplate(documentData);

            return output;
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

    private async Task<string> GetTemplateString(string templatePath)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(templatePath);
        return await response.Content.ReadAsStringAsync();
    }
}

