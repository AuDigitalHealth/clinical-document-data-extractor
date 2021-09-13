using System;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class ImmunisationRegister
    {
        public DateTime? DateTime { get; set; }
        public CodableText Immunisation { get; set; }
        public CodableText BrandName { get; set; }
    }
}