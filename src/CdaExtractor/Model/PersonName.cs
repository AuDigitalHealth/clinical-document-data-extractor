using System.Text;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class PersonName
    {
        public string Prefix { get; set; }
        public string Family { get; set; }
        public string Given { get; set; }

        public override string ToString()
        {
            var nameBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(Given))
            {
                nameBuilder.Append(Given);
            }
            if (nameBuilder.Length > 0)
            {
                nameBuilder.Append(" ");
            }
            nameBuilder.Append(Family);

            return nameBuilder.ToString();            
        }
    }
}