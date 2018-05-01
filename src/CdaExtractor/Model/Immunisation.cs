using System;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class Immunisation
    {
        public CodableText Medicine { get; set; }
        public string SequenceNumber { get; set; }
        public DateTime? DateTime { get; set; }
    }
}