using System.Xml;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public interface ICdaExtractor<out T>
    {
        T Extract(CdaXmlDocument cdaDocument);
    }
}