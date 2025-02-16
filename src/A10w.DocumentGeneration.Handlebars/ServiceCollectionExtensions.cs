using Microsoft.Extensions.DependencyInjection;

namespace A10w.DocumentGeneration.Handlebars;

/// <summary>
/// <see cref="IServiceCollection"/> extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddHandlebarsDocumentGeneration(this IServiceCollection services,
                                                             Action<HandlebarsDocumentGenerationConfiguration> configure)
    {
        services.AddSingleton<IDocumentGenerator>(new HandlebarsDocumentGenerator());

        var builder = new HandlebarsDocumentGenerationConfiguration();
        configure(builder);

        var config = builder.Build();
        HandlebarsCompiler.Create(config.Handlebars, config.Paths);

        return services;
    }
}
