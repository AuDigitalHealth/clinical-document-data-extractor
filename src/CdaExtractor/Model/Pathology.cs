using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class Pathology
    {
        // Document Details
        public string ReportName { get; set; }

        public DateTime ReportDateTime { get; set; }

        public CodableText ReportStatus { get; set; }

        public Id ReportIdentifier { get; set; }

        public string ReportIntegrityCheck { get; set; }

        public string ReportMediaType { get; set; }

        public string ReportDocName { get; set; }

        // Pathologist
        public PersonName Pathologist { get; set; }

        public string PathologistIdentifier { get; set; }

        public string OrganisationName { get; set; }

        public string OrganisationIdentifier { get; set; }

        public List<Telecom> ContactDetails { get; set; }

        // Results
        public List<PathologyTestResult> PathologyTestResults { get; set; }
    }
}