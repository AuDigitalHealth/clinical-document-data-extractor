using System.Collections.Generic;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class MedicationsExtractor : ICdaExtractor<List<Medication>>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public MedicationsExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public List<Medication> Extract(CdaXmlDocument cdaDocument)
        {
            var medications = new List<Medication>();
            var medicationNodes = cdaDocument.SelectNodes(_documentXPaths.Get("Medication"));
            if (medicationNodes == null) return null;

            foreach (XmlNode medicationNode in medicationNodes)
            {
                var medication = new Medication();

                var medicineNode = cdaDocument.SelectSingleNode(medicationNode, _documentXPaths.Get("Medicine")) as XmlElement;

                medication.Medicine = cdaDocument.GetCodableText(medicineNode);

                medication.GenericName = cdaDocument.SelectSingleNode(medicationNode, _documentXPaths.Get("GenericName"))?.InnerText;

                var clinicalIndication = cdaDocument.SelectSingleNode(medicationNode, _documentXPaths.Get("ClinicalIndication"));

                medication.ClinicalIndications = clinicalIndication == null ? null : clinicalIndication.InnerText;

                var directions = cdaDocument.SelectSingleNode(medicationNode, _documentXPaths.Get("Directions"));

                medication.Directions = directions == null || string.IsNullOrWhiteSpace(directions.InnerText) ? null : directions.InnerText;

                medication.Strength = cdaDocument.SelectSingleNode(medicationNode, _documentXPaths.Get("Strength"))?.InnerText;

                if (directions != null)
                {
                    var directionsNullFlavourAttribute = directions.Attributes["nullFlavor"];

                    if (directionsNullFlavourAttribute != null)
                        medication.DirectionsNullFlavour = directionsNullFlavourAttribute.Value;
                }

                var comments = cdaDocument.SelectSingleNode(medicationNode,
                    _documentXPaths.Get("Comments"));
                medication.Comment = comments == null ? null : comments.InnerText;

                XmlNode nameNode = cdaDocument.SelectSingleNode(medicationNode, _documentXPaths.Get("GenericName"));
                if (nameNode != null)
                {
                    medication.GenericName = nameNode.InnerText;
                }

                medication.Status = cdaDocument.GetCodableText(medicationNode, _documentXPaths.Get("Status"));

                medications.Add(medication);
            }

            return medications;
        }
    }
}
