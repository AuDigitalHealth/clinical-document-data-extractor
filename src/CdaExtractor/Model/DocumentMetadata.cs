using System;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class DocumentMetadata
    {
        public string TemplateId { get; set; }
        public Id DocumentId { get; set; }
        public CodableText DocumentCode { get; set; }
        public string Title { get; set; }
        public DateTime? EffectiveTime { get; set; }
        public Id LinkId { get; set; }
        public DateTime? EventStartDateTime { get; set; }
        public DateTime? EventEndDateTime { get; set; }
    }
}