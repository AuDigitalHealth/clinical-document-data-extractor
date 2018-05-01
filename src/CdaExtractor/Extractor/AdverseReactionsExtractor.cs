using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class AdverseReactionsExtractor : ICdaExtractor<List<AdverseReaction>>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public AdverseReactionsExtractor(IDictionary<string, string>  documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public List<AdverseReaction> Extract(CdaXmlDocument document)
        {
            var nodes = document.SelectNodes(_documentXPaths.Get("AdverseReactions"));
            if(nodes == null || nodes.Count == 0) return null;

            return
                (from XmlNode node in nodes
                    select new AdverseReaction
                    {
                        AdverseReactionId = document.GetString(node, _documentXPaths.Get("AdverseReactionsId")),
                        SubstanceAgent = document.GetRelativeCode(node, _documentXPaths.Get("SubstanceAgent")),
                        AdverseReactionType = document.GetRelativeCode(node, _documentXPaths.Get("AdverseReactionType")),
                        Manifestations = document.GetListOfRelativeCodableText(node, _documentXPaths.Get("Manifestations"))
                    }
                ).ToList();
        }
      
    }
}
