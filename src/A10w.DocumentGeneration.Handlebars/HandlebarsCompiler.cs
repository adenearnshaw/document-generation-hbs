using HandlebarsDotNet;

namespace A10w.DocumentGeneration.Handlebars;

internal class HandlebarsCompiler : IDisposable
{
    public static HandlebarsCompiler Instance { get; private set; }

    public static bool IsInstantiated => Instance != null;


    private IHandlebars _handlebars;
    private Dictionary<Type, string> _paths;

    internal static void Create(IHandlebars handlebars, Dictionary<Type, string> paths)
    {
        Instance = new HandlebarsCompiler(handlebars, paths);
    }

    private HandlebarsCompiler(IHandlebars handlebars, Dictionary<Type, string> paths)
    {
        _handlebars = handlebars;
        _paths = paths;
    }

    internal HandlebarsTemplate<object, object> GetCompiler(Type dataType)
    {
        return GetCompilerInternal(_paths[dataType]);
    }

    internal HandlebarsTemplate<object, object> GetCompiler<T>() where T : IDocumentData
    {
        return GetCompilerInternal(_paths[typeof(T)]);
    }

    internal HandlebarsTemplate<object, object> GetCompiler(string template)
    {
        return _handlebars.Compile(template);
    }

    private HandlebarsTemplate<object, object> GetCompilerInternal(string path)
    {
        if (path == null)
            throw new Exception("Type not registered");
        
        var template = _handlebars.CompileView(path);
        return template;
    }

    public void Dispose()
    {
        Instance = null;
    }
}
