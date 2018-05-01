using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class CodableText
    {
        public string Code { get; set; }
        public string CodeSystem { get; set; }
        public string CodeSystemName { get; set; }
        public string DisplayName { get; set; }
        public string OriginalText { get; set; }
        public string CodeSystemVersion { get; set; }

        [XmlArray("Translations")]
        [XmlArrayItem("Translation")]
        public List<CodableText> Translations { get; set; }

        public string NullFlavor { get; set; }
    }
}

