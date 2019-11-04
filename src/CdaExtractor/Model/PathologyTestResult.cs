using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class PathologyTestResult
    {
        public CodableText TestResultName { get; set; }
        public CodableText Discipline{ get; set; }

        public DateTime SpecimenCollectionDateTime { get; set; }
        public DateTime ObservationDateTime { get; set; }

        public CodableText TestResultStatus { get; set; }

    }
}