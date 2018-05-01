using System;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    /// <summary>
    /// Medical History
    /// </summary>
    public class MedicalHistory
    {
        public MedicalHistoryType MedicalHistoryItemType { get; set; }

        public CodableText MedicalHistoryItem { get; set; }

        public CodableText ProblemDiagnosisType { get; set; }

        public Interval Interval { get; set; }

        public string Comment { get; set; }
    }
}
