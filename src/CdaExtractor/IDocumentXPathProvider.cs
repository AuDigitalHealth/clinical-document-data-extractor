using System.Collections.Generic;

namespace Nehta.VendorLibrary.CdaExtractor
{
    public interface IDocumentXPathProvider
    {
        IDictionary<string, string> GetDocumentXPaths(string templateId);
    }
}