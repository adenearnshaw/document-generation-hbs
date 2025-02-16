namespace A10w.DocumentGeneration;

/// <summary>
/// Service that generates Documents
/// </summary>
public interface IDocumentGenerator
{
    /// <summary>
    /// Generate Document based on Data
    /// </summary>
    /// <typeparam name="T"><see cref="IDocumentData"/></typeparam>
    /// <param name="documentData">Data object associated with document</param>
    /// <returns>Full interpolated string representation of the document</returns>
    Task<string> GenerateDocument<T>(T documentData) where T : IDocumentData;

    /// <summary>
    /// Generate Document based on Data
    /// </summary>
    /// <param name="dataType">Type of Data object</param>
    /// <param name="documentData">Data object associated with document</param>
    /// <returns>Full interpolated string representation of the document</returns>
    Task<string> GenerateDocument(Type dataType, object documentData);
}
