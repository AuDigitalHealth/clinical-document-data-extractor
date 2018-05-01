using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class PharmaceuticalBenefitItemExtractor : ICdaExtractor<List<PharmaceuticalBenefitItem>>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public PharmaceuticalBenefitItemExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public List<PharmaceuticalBenefitItem> Extract(CdaXmlDocument cdaDocument)
        {
            var pbsItems = new List<PharmaceuticalBenefitItem>();

            var pbsNodes = cdaDocument.SelectNodes(_documentXPaths.Get("PharmaceuticalBenefitItems"));
            if (pbsNodes == null) return null;

            foreach (XmlNode pbsNode in pbsNodes)
            {
                var pbsItem = new PharmaceuticalBenefitItem();
                
                pbsItem.ItemCode = cdaDocument.GetCodableText(cdaDocument.SelectSingleNode(pbsNode, _documentXPaths.Get("ItemCode")) as XmlElement);

                pbsItem.ManufacturerCode = cdaDocument.GetString(cdaDocument.SelectSingleNode(pbsNode, _documentXPaths.Get("ManufacturerCode")) as XmlAttribute);

                pbsItem.Brand = cdaDocument.GetString(pbsNode, _documentXPaths.Get("Brand"));

                pbsItem.ItemGenericName = cdaDocument.GetString(cdaDocument.SelectSingleNode(pbsNode, _documentXPaths.Get("GenericName")) as XmlAttribute);
                
                pbsItem.ItemFormAndStrength = cdaDocument.GetString(pbsNode, _documentXPaths.Get("ItemFormAndStrength"));

                pbsItem.DateOfSupply = cdaDocument.GetDateTimeValue(pbsNode, _documentXPaths.Get("DateOfSupply"));
                
                pbsItem.DateOfPrescribing = cdaDocument.GetDateTimeValue(pbsNode, _documentXPaths.Get("DateOfPrescribing"));

                pbsItem.Quantity = cdaDocument.GetString(pbsNode, _documentXPaths.Get("Quantity"));

                pbsItem.NumberOfRepeats = cdaDocument.GetString(pbsNode, _documentXPaths.Get("NumberOfRepeats"));

                pbsItems.Add(pbsItem);
            }

            return pbsItems;
        }
    }
}
