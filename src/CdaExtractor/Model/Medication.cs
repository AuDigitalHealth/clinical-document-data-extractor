namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class Medication
    {
        public CodableText Medicine { get; set; }
        public string Directions { get; set; }
        public string DirectionsNullFlavour { get; set; }
        public string ClinicalIndications { get; set; }
        public string Comment { get; set; }
        public CodableText Status { get; set; }
        public string GenericName { get; set; }
        public string Strength { get; set; }
    }
  
}
