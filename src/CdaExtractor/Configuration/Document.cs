using System.Collections.Generic;

namespace Nehta.VendorLibrary.CdaExtractor.Configuration
{
    public class Document
    {
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public List<Field> FieldMappings { get; set; }
    }
}