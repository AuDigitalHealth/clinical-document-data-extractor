using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class ImmunisationRegisterExtractor : ICdaExtractor<List<ImmunisationRegister>>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public ImmunisationRegisterExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public List<ImmunisationRegister> Extract(CdaXmlDocument document)
        {
            var nodes = document.SelectNodes(_documentXPaths.Get("ImmunisationRegister"));
            if(nodes == null || nodes.Count == 0) return null;

            return
                (from XmlNode node in nodes
                 select new ImmunisationRegister
                 {
                        DateTime = document.GetDateTimeValue(node, _documentXPaths.Get("ImmunisationDateTime")),
                        Immunisation = document.GetRelativeCode(node, _documentXPaths.Get("Immunisation")),
                        BrandName = document.GetRelativeCode(node, _documentXPaths.Get("BrandName"))
                    }
                ).ToList();
        }
    }
}
