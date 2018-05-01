using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class MedicalHistoryExtractor : ICdaExtractor<List<MedicalHistory>>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public MedicalHistoryExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public List<MedicalHistory> Extract(CdaXmlDocument cdaDocument)
        {
            var medicalHistoryItems = new List<MedicalHistory>();

            var problemDiagnosis = LoadProblemDiagnosis(cdaDocument);
            if (problemDiagnosis != null && problemDiagnosis.Any())
                medicalHistoryItems.AddRange(problemDiagnosis.ToArray());

            var procedures = LoadProcedures(cdaDocument);
            if (procedures != null && procedures.Any())
                medicalHistoryItems.AddRange(procedures.ToArray());

            var otherMedicalHistoryItem = LoadOtherMedicalHistoryItems(cdaDocument);
            if (otherMedicalHistoryItem != null && otherMedicalHistoryItem.Any())
                medicalHistoryItems.AddRange(otherMedicalHistoryItem.ToArray());

            return medicalHistoryItems.Any() ? medicalHistoryItems : null;
        }

        private IList<MedicalHistory> LoadOtherMedicalHistoryItems(CdaXmlDocument cdaDocument)
        {
            IList<MedicalHistory> otherMedicalHistoryList = new List<MedicalHistory>();

            var otherMedicalHistoryItemNodes = cdaDocument.SelectNodes(_documentXPaths.Get("OtherMedicalHistoryItem"));

            if (otherMedicalHistoryItemNodes != null)
            foreach (XmlElement historyItemNode in otherMedicalHistoryItemNodes)
            {
                var otherMedicalHistoryItem = new MedicalHistory
                {
                    MedicalHistoryItemType = MedicalHistoryType.OtherMedicalHistoryItem
                };

                // Medical History Item Description
                var description = cdaDocument.GetString(historyItemNode, _documentXPaths.Get("OtherMedicalHistoryItemDescription"));
                if (!string.IsNullOrWhiteSpace(description))
                {
                    otherMedicalHistoryItem.MedicalHistoryItem = new CodableText { OriginalText = description };
                }

                // Medical History Item Time Interval
                otherMedicalHistoryItem.Interval = cdaDocument.GetInterval(historyItemNode, _documentXPaths.Get("OtherMedicalHistoryItemTimeInterval"));

                // Medical History Item Comment
                otherMedicalHistoryItem.Comment = cdaDocument.GetString(historyItemNode, _documentXPaths.Get("OtherMedicalHistoryItemComment"));

                otherMedicalHistoryList.Add(otherMedicalHistoryItem);
            }

            return otherMedicalHistoryList.Any() ? otherMedicalHistoryList : null;
        }

        private IList<MedicalHistory> LoadProcedures(CdaXmlDocument cdaDocument)
        {
            IList<MedicalHistory> proceduresList = new List<MedicalHistory>();

            //ProcedureStartDateTime
            var proceduresNodes = cdaDocument.SelectNodes(_documentXPaths.Get("Procedures"));

            if (proceduresNodes != null)
            foreach (XmlElement procedureNode in proceduresNodes)
            {
                var proceduresItem = new MedicalHistory
                {
                    MedicalHistoryItemType = MedicalHistoryType.Procedure
                };

                // Procedure name    
                proceduresItem.MedicalHistoryItem = cdaDocument.GetRelativeCode(procedureNode, _documentXPaths.Get("ProcedureName"));

                // Procedure Comment
                proceduresItem.Comment = cdaDocument.GetString(procedureNode, _documentXPaths.Get("ProcedureComment"));

                // Date/time started
                var startedDateTime = cdaDocument.GetDateTimeValue(procedureNode, _documentXPaths.Get("ProcedureStartDateTime"));

                if (startedDateTime.HasValue)
                {
                    proceduresItem.Interval = new Interval
                    {
                        Start = startedDateTime
                    };
                }

                proceduresList.Add(proceduresItem);
            }

            return proceduresList.Any() ? proceduresList : null;
        }

        private IList<MedicalHistory> LoadProblemDiagnosis(CdaXmlDocument cdaDocument)
        {
            IList<MedicalHistory> problemDiagnosiList = new List<MedicalHistory>();

            var problemDiagnosisNodes = cdaDocument.SelectNodes(_documentXPaths.Get("ProblemDiagnosis"));

            if (problemDiagnosisNodes != null)
            foreach (XmlElement problemNode in problemDiagnosisNodes)
            {
                var proceduresItem = new MedicalHistory
                {
                    MedicalHistoryItemType = MedicalHistoryType.ProblemDiagnosis
                };

                // Problem Diagnosis Identification
                proceduresItem.MedicalHistoryItem = cdaDocument.GetRelativeCode(problemNode, _documentXPaths.Get("ProblemDiagnosisIdentification"));

                // Problem Diagnosis Comment
                proceduresItem.Comment = cdaDocument.GetString(problemNode, _documentXPaths.Get("ProblemDiagnosisComment"));

                // Medical History Type
                proceduresItem.ProblemDiagnosisType = cdaDocument.GetRelativeCode(problemNode, _documentXPaths.Get("ProblemDiagnosisType"));

                // Problem Diagnosis Date Of Onset
                var onsetDateTime = cdaDocument.GetDateTimeValue(problemNode, _documentXPaths.Get("ProblemDiagnosisDateOfOnset"));

                // Note: Date Of Resolution Remission has been mapped to a Date in the SCS even though it is an Interval.
                //       There therefore is a need to check the LOW, HIGH and VALUE in that order and return the interval high value for the MedicalHistory Interval.

                DateTime? dateOfResolution = null;
                // Get Date Of Resolution Interval
                var dateOfResolutionInterval = cdaDocument.GetInterval(problemNode, _documentXPaths.Get("ProblemDiagnosisDateOfResolutionRemission"));

                if (dateOfResolutionInterval != null)
                {
                    if (dateOfResolutionInterval.Start.HasValue)
                        dateOfResolution = dateOfResolutionInterval.Start;

                    if (dateOfResolutionInterval.End.HasValue)
                        dateOfResolution = dateOfResolutionInterval.End;
                }

                if (!dateOfResolution.HasValue)
                {
                    // Get Date Of Resolution Attribute Value
                    dateOfResolution = cdaDocument.GetDateTimeValue(problemNode, string.Format("{0}{1}", _documentXPaths.Get("ProblemDiagnosisDateOfResolutionRemission"), "/@value"));
                }

                if (onsetDateTime.HasValue || dateOfResolution.HasValue)
                {
                    proceduresItem.Interval = new Interval
                    {
                        Start = onsetDateTime.HasValue ? onsetDateTime : null,
                        End = dateOfResolution.HasValue ? dateOfResolution : null
                    };
                }

                problemDiagnosiList.Add(proceduresItem);
            }

            return problemDiagnosiList.Any() ? problemDiagnosiList : null;
        }
    }
}
