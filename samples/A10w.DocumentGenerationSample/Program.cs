using A10w.DocumentGeneration;
using A10w.DocumentGeneration.Handlebars;
using System.Net.Mime;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHandlebarsDocumentGeneration(cfg =>
{
    cfg.AddPartialTemplate("styles", "Views/Styles/sample.css");
    cfg.AddPartialTemplate("Header", "Views/Partials/Header.hbs");
    cfg.AddView<SampleData>("Views/SamplePage.hbs");
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/samples/compiled", async (IDocumentGenerator documentGenerator) =>
{
    var sampleHtml = await documentGenerator.GenerateDocument(new SampleData());
    return new HtmlResult(sampleHtml);
});

app.MapGet("/samples/dynamic", async (IDocumentGenerator documentGenerator) =>
{
    var data = new SampleData() with
    {
        Header = new HeaderData("Dynamic Template Page")
    };
    var sampleHtml = await documentGenerator.GenerateDocument("https://raw.githubusercontent.com/adenearnshaw/document-generation-hbs/refs/heads/main/samples/dynamic-templates/dynamic-template.hbs", data);
    return new HtmlResult(sampleHtml);
});

app.Run();

internal record SampleData : IDocumentData
{
    public HeaderData Header { get; set; } = new HeaderData("SamplePage");

    public string Description { get; init; } = "This is a sample page that demonstrates and validates the usage of the A10w.DocumentGeneration Nuget Package";
    public List<string> Benefits { get; init; } = new List<string>()
    {
        "Shared Layouts",
        "Reusable Partial Layouts",
        "Strongly Typed Data Objects",
        "Strongly Named Data Properties",
        "Syntactic highlighting for HTML documents"
    };
}

internal record struct HeaderData(string Title, string Subtitle = "");

class HtmlResult : IResult
{
    private readonly string _html;

    public HtmlResult(string html)
    {
        _html = html;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }
}