using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class PharmaceuticalBenefitItem
    {
        public CodableText ItemCode { get; set; }
        public string ManufacturerCode { get; set; }
        public string Brand { get; set; }
        public string ItemGenericName { get; set; }
        public string ItemFormAndStrength { get; set; }
        public DateTime? DateOfSupply { get; set; }
        public DateTime? DateOfPrescribing { get; set; }
        public string Quantity { get; set; }
        public string NumberOfRepeats { get; set; }
    }
}
