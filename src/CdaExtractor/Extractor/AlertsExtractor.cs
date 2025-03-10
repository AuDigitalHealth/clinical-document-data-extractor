using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class AlertsExtractor : ICdaExtractor<List<Alert>>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public AlertsExtractor(IDictionary<string, string>  documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public List<Alert> Extract(CdaXmlDocument document)
        {
            var nodes = document.SelectNodes(_documentXPaths.Get("Alerts"));
            if(nodes == null || nodes.Count == 0) return null;

            return
                (from XmlNode node in nodes
                    select new Alert
                    {
                        AlertId = document.GetString(node, _documentXPaths.Get("AlertId")),
                        AlertValue = document.GetRelativeCode(node, _documentXPaths.Get("AlertText")),
                    }
                ).ToList();
        }
      
    }
}
