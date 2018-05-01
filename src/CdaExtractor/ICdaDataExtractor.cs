using System.Collections.Generic;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor
{
    public interface ICdaDataExtractor
    {
        CdaDocument Extract(XmlDocument cdaDocument);
        List<CdaDocument> Extract(IList<XmlDocument> cdaDocuments);
    }
}