using HandlebarsDotNet;

namespace A10w.DocumentGeneration.Handlebars.Helpers;

/// <summary>
/// Convert a web-based image url to an embedded base64 image
/// {{image-url "https://example.com/image.png"}}
/// </summary>
public class ImageUrlHelper : IHelper
{
    internal static string HelperName => "image-url";

    internal static HandlebarsHelper Delegate { get; } = (output, _, arguments) =>
    {
        try
        {
            var url = arguments[0].ToString();
            var httpClient = new HttpClient();
            var data = httpClient.GetByteArrayAsync(url).Result;
            var base64 = Convert.ToBase64String(data);
            var imgSrc = "data:image/png;base64," + base64;
            output.WriteSafeString(imgSrc);
        }
        catch (Exception)
        {
            output.WriteSafeString($"");
        }
    };
}