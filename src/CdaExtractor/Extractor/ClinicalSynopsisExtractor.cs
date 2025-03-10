using System.Collections.Generic;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class ClinicalSynopsisExtractor : ICdaExtractor<ClinicalSynopsis>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public ClinicalSynopsisExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public ClinicalSynopsis Extract(CdaXmlDocument cdaDocument)
        {
            var cs = new ClinicalSynopsis();
            var xpath = _documentXPaths.Get("ClinicalSynopsis");

            if (xpath != null)
            {
                cs.Description = cdaDocument.SelectSingleNode(xpath)?.InnerText;
            }
            
            return cs;
        }
    }
}
