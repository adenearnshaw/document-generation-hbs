using Microsoft.Extensions.DependencyInjection;

namespace A10w.DocumentGeneration.Handlebars.IntegrationTests;

public class DocumentGeneratorTestWrapper : IDisposable
{
    public IServiceCollection Services { get; private set; }

    public DocumentGeneratorTestWrapper()
    {
        Services = new ServiceCollection();
    }

    public IDocumentGenerator GetSubject()
    {
        var sp = Services.BuildServiceProvider();
        var service = sp.GetService<IDocumentGenerator>();

        return service;
    }

    public void Dispose()
    {
        Services = null;
    }
}