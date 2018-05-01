using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class AdverseReaction
    {
        public string AdverseReactionId { get; set; }
        public CodableText SubstanceAgent { get; set; }
        public CodableText AdverseReactionType { get; set; }

        [XmlArray("Manifestations")]
        [XmlArrayItem("Manifestation")]
        public List<CodableText> Manifestations { get; set; }
    }
}