using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class Psml
    {
        // Document Details (CDA 1A)
        public string ReportIntegrityCheck { get; set; }

        public string ReportMediaType { get; set; }

        public string ReportDocName { get; set; }
    }
}