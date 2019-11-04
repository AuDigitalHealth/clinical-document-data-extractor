using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class DiagnosticImaging
    {
        // Document Details
        public DateTime ReportDateTime { get; set; }

        public string ReportDescription { get; set; }

        public CodableText ReportStatus { get; set; }

        public string ReportIntegrityCheck { get; set; }

        public string ReportMediaType { get; set; }

        public string ReportDocName { get; set; }

        // Radiologist
        public PersonName Radiologist { get; set; }

        public string RadiologistIdentifier { get; set; }

        public string OrganisationName { get; set; }

        public string OrganisationIdentifier { get; set; }

        public List<Telecom> ContactDetails { get; set; }

        // Results
        public Id AccessionNumber { get; set; }

        public List<ImagingExaminationResult> ImagingExaminationResults { get; set; }
    }
}