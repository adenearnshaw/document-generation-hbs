using HandlebarsDotNet;
using System.Reflection;

namespace A10w.DocumentGeneration.Handlebars;

internal class DiskFileSystem : ViewEngineFileSystem
{
    private readonly string _assemblyLocation;

    public DiskFileSystem()
    {
        _assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }

    public override string GetFileContent(string filename)
    {
        var path = Path.Combine(_assemblyLocation, filename);
        return File.ReadAllText(path);
    }

    protected override string CombinePath(string dir, string otherFileName)
    {
        return Path.Combine(dir, otherFileName);
    }

    public override bool FileExists(string filePath)
    {
        var path = Path.Combine(_assemblyLocation, filePath);
        return File.Exists(path);
    }
}