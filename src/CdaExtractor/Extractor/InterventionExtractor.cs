﻿using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class InterventionExtractor : ICdaExtractor<List<Intervention>>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public InterventionExtractor(IDictionary<string, string>  documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public List<Intervention> Extract(CdaXmlDocument document)
        {
            var nodes = document.SelectNodes(_documentXPaths.Get("Interventions"));
            if(nodes == null || nodes.Count == 0) return null;

            return
                (from XmlNode node in nodes
                    select new Intervention
                    {
                        InterventionId = document.GetString(node, _documentXPaths.Get("InterventionId")),
                        InterventionValue = document.GetRelativeCode(node, _documentXPaths.Get("InterventionText")),
                    }
                ).ToList();
        }
      
    }
}
