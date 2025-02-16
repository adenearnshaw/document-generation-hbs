using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;

namespace A10w.DocumentGeneration.Handlebars.IntegrationTests;

[Collection("Sequential")]
public class DocumentGeneratorTests
{
    [Fact]
    public async Task GenerateDocument_ReturnsHtmlWithLayoutAndPartials_WhenConfiguredCorrectly()
    {
        // Arrange
        var wrapper = new DocumentGeneratorTestWrapper();
        wrapper.Services.AddHandlebarsDocumentGeneration(cfg =>
        {
            cfg.AddPartialTemplate("header", "SampleData/Partials/Header.hbs");
            cfg.AddPartialTemplate("footer", "SampleData/Partials/Footer.hbs");
            cfg.AddView<EmptyDocument>("SampleData/EmptyPage.hbs");
        });

        var sut = wrapper.GetSubject();

        // Act
        var document = await sut.GenerateDocument(new EmptyDocument());

        // Assert
        Assert.NotNull(document);
        Assert.NotEmpty(document);
        Assert.Contains("<h1>This is an empty page</h1>", document);
        Assert.Contains("Header", document);
        Assert.Contains("FooterContent", document);
    }

    [Fact]
    public async Task GenerateDocument_ReturnsInterpolatedHtml_WhenConfiguredCorrectly()
    {
        // Arrange
        var wrapper = new DocumentGeneratorTestWrapper();
        wrapper.Services.AddHandlebarsDocumentGeneration(cfg =>
        {
            cfg.AddPartialTemplate("header", "SampleData/Partials/Header.hbs");
            cfg.AddPartialTemplate("footer", "SampleData/Partials/Footer.hbs");
            cfg.AddView<SimpleDocument>("SampleData/SimplePage.hbs");
        });

        var sut = wrapper.GetSubject();

        // Act
        var document = await sut.GenerateDocument(new SimpleDocument("Simple"));

        // Assert
        Assert.NotNull(document);
        Assert.NotEmpty(document);
        Assert.Contains("<h1>This is an Simple page</h1>", document);
        Assert.Contains("Header", document);
        Assert.Contains("FooterContent", document);
    }

    [Fact]
    public async Task GenerateDocument_ReturnsInterpolatedHtml_WhenDataHasList()
    {
        // Arrange
        var wrapper = new DocumentGeneratorTestWrapper();
        wrapper.Services.AddHandlebarsDocumentGeneration(cfg =>
        {
            cfg.AddPartialTemplate("header", "SampleData/Partials/Header.hbs");
            cfg.AddPartialTemplate("footer", "SampleData/Partials/Footer.hbs");
            cfg.AddView<ListDocument>("SampleData/ListPage.hbs");
        });

        var sut = wrapper.GetSubject();

        // Act
        var document = await sut.GenerateDocument(new ListDocument(new List<string> { "One", "Two", "Three" }));

        // Assert
        Assert.NotNull(document);
        Assert.NotEmpty(document);
        Assert.Contains("<h1>This is a List page</h1>", document);
        Assert.Contains("Header", document);
        Assert.Contains("FooterContent", document);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(document);
        var liNodes = htmlDoc.DocumentNode.SelectNodes("//li");
        Assert.Equal(3, liNodes.Count);
        Assert.Equal("Three", liNodes[2].InnerText);
    }

    [Fact]
    public async Task GenerateDocument_ReturnsInterpolatedHtml_WhenDataHasComplexObject()
    {
        // Arrange
        var wrapper = new DocumentGeneratorTestWrapper();
        wrapper.Services.AddHandlebarsDocumentGeneration(cfg =>
        {
            cfg.AddPartialTemplate("header", "SampleData/Partials/Header.hbs");
            cfg.AddPartialTemplate("footer", "SampleData/Partials/Footer.hbs");
            cfg.AddView<ComplexDocument>("SampleData/ComplexPage.hbs");
        });

        var sut = wrapper.GetSubject();

        // Act
        var document = await sut.GenerateDocument(new ComplexDocument(new List<ComplexData>
        {
            new ComplexData
            {
                SectionName = "One",
                Children = new List<string> { "A", "B", "C" },
            },
            new ComplexData
            {
                SectionName = "Two",
                Children = new List<string> { "D", "E", "F" },
            },
            new ComplexData
            {
                SectionName = "Three",
                Children = new List<string> { "G", "H", "I" },
            },
            new ComplexData
            {
                SectionName = "Four",
                Children = new List<string> { "J", "K", "L", "M" },
            }
        }));

        // Assert
        Assert.NotNull(document);
        Assert.NotEmpty(document);
        Assert.Contains("<h1>This is a List page</h1>", document);
        Assert.Contains("Header", document);
        Assert.Contains("FooterContent", document);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(document);
        var liNodes = htmlDoc.DocumentNode.SelectNodes("//li[contains(@class, 'child-item')]");
        Assert.Equal(13, liNodes.Count);
        Assert.Equal("M", liNodes[12].InnerText);
    }


    // [Fact(Skip = "Fails in test runner when run in batch (despite turning of parallization")]
    [Fact]
    public async Task GenerateDocument_ThrowsInformativeException_WhenServiceNotAddedCorrectly()
    {
        // Arrange
        var wrapper = new DocumentGeneratorTestWrapper();
        wrapper.Services.AddSingleton<IDocumentGenerator>(new HandlebarsDocumentGenerator());
        var sut = wrapper.GetSubject();

        // Act + Assert
        var exception = await Assert.ThrowsAsync<DocumentGenerationException>(() => sut.GenerateDocument(new SimpleDocument("Simple")));
        Assert.Contains("DocumentGeneration.Handlebars not initialized", exception.Message);
    }

    // [Fact(Skip = "Fails in test runner when run in batch (despite turning of parallization")]
    [Fact]
    public async Task GenerateDocument_ThrowsInformativeException_WhenPartialNotRegistered()
    {
        // Arrange
        var wrapper = new DocumentGeneratorTestWrapper();
        wrapper.Services.AddHandlebarsDocumentGeneration(cfg =>
        {
            cfg.AddPartialTemplate("header", "SampleData/Partials/Header.hbs");
            cfg.AddView<SimpleDocument>("SampleData/SimplePage.hbs");
        });
        var sut = wrapper.GetSubject();

        // Act + Assert
        var exception = await Assert.ThrowsAsync<DocumentGenerationException>(() => sut.GenerateDocument(new SimpleDocument("Simple")));
        Assert.Equal("Referenced partial name footer could not be resolved", exception.InnerException.Message);
    }

    // [Fact(Skip = "Fails in test runner when run in batch (despite turning of parallization")]
    [Fact]
    public async Task GenerateDocument_ThrowsInformativeException_WhenPartialSpeltWrong()
    {
        // Arrange
        var wrapper = new DocumentGeneratorTestWrapper();
        wrapper.Services.AddHandlebarsDocumentGeneration(cfg =>
        {
            cfg.AddPartialTemplate("header", "SampleData/Partials/Header.hbs");
            cfg.AddPartialTemplate("dooter", "SampleData/Partials/Footer.hbs");
            cfg.AddView<SimpleDocument>("SampleData/SimplePage.hbs");
        });
        var sut = wrapper.GetSubject();

        // Act + Assert
        var exception = await Assert.ThrowsAsync<DocumentGenerationException>(() => sut.GenerateDocument(new SimpleDocument("Simple")));
        Assert.Equal("Referenced partial name footer could not be resolved", exception.InnerException.Message);
    }

    public record struct EmptyDocument : IDocumentData;
    public record struct SimpleDocument(string PageName) : IDocumentData;
    public record struct ListDocument(List<string> Items) : IDocumentData;
    public record struct ComplexData(string SectionName, List<string> Children);
    public record struct ComplexDocument(List<ComplexData> Items) : IDocumentData;
}