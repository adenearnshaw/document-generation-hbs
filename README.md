# A10w.DocumentGeneration

Library to implement and wrap [Handlebars.Net](https://github.com/Handlebars-Net/Handlebars.Net) into a reusable class library suitable for converting to a Nuget.

Using [Handlebars](https://handlebarsjs.com/guide/partials.html) as a foundation, this package allows consumers of the Document Generation Library (DocGen) to divide up html templates into components and then combine them with POCO models to generate an interpolated HTML documnt.

This means the HTML stays within the source of the Project repo, but minimises duplication.

## Adding to your project

1. Add the Handlebar files in the location of choice
1. In you _.csproj_ file add the following code to include the handlebar files in the output:

    ```xml
    <ItemGroup>
        <None Include="Views\**\*.hbs">
            <CopyToOutputDirectory>Always</CopyToOutputDirectory>
        </None>
    </ItemGroup>
    ```

1. Create a POCO that inherits `IDocumentData` e.g. `MyDocumentData`
1. In _program.cs_ call `builder.Services.AddHandlebarsDocumentGeneration(cfg => ...);`
    e.g.

    ```csharp
    builder.Services.AddHandlebarsDocumentGeneration(cfg =>
    {
        cfg.AddPartialTemplate("DocumentHeader", "Views/Documents/Partials/Header.hbs");
        cfg.AddView<MyDocumentData>("Views/Documents/MyDocument.hbs");
    }
    ```

## Using Document Generator

1. Inject `IDocumentGenerator` into your constructor/consumer
1. In your method, call `IDocumentGenerator.GenerateDocument<T>()` passing in an instance of the `IDocumentData` registered to the View

    ```csharp
    var myDocumentData = new MyDocumentData("Custom Value");
    var myDocumentHtml = await _documentGenerator.GenerateDocument(myDocumentData);
    ```

## Handlebars

[Handlebars.Net](https://github.com/Handlebars-Net/Handlebars.Net) is a library that replicates the functionality and api of [handlebarsjs](https://handlebarsjs.com/guide) in .NET.

Handlebars.js is an extension to the Mustache templating language. Handlebars.js and Mustache are both logicless templating languages that keep the view and the code separated.

Using Handlebars, we can interpolate strings, iterate through arrays and take advantage of both partials and shared layouts. The [handlebarsjs guide](https://handlebarsjs.com/guide/expressions.html#basic-usage) provides a good idea of the type of expressions that can be taken advantage of.

For additional examples, see the [Integration Test Project](./src/A10w.DocumentGeneration.Handlebars.IntegrationTests/DocumentGeneratorTests.cs)
