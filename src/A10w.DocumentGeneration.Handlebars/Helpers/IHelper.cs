using HandlebarsDotNet;

namespace A10w.DocumentGeneration.Handlebars.Helpers;

/// <summary>
/// Block Helper
/// </summary>
public interface IHelper
{
    internal static string HelperName { get; }
    internal static HandlebarsHelper Delegate { get; }
}