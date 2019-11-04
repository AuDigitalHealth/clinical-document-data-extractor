using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class ImagingExaminationResult
    {
        public CodableText ExamResultName { get; set; }
        public CodableText Modality { get; set; }
        public List<AnatomicalSite> AnatomicalSites { get; set; }
        public CodableText AnatomicalRegion { get; set; }
        public CodableText OverallResultStatus { get; set; }
        public DateTime ImageDateTime { get; set; }
        public string ExaminationProcedure { get; set; }
        public DateTime ObservationDateTime { get; set; }
    }

    public class AnatomicalSite
    {
        public CodableText NameOfLocation { get; set; }
        public CodableText Side { get; set; }
        public string Description { get; set; }
    }
}