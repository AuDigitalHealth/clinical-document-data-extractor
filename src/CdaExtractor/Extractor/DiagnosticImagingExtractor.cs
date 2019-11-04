using System.Collections.Generic;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class DiagnosticImagingExtractor : ICdaExtractor<DiagnosticImaging>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public DiagnosticImagingExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public DiagnosticImaging Extract(CdaXmlDocument cdaDocument)
        {
            var di = new DiagnosticImaging();

            var diDocNode = cdaDocument.SelectSingleNode(_documentXPaths.Get("DIDocument"));

            var diReportDateTime = cdaDocument.GetDateTimeValue(diDocNode, _documentXPaths.Get("DIReportDateTime"));
            if (diReportDateTime != null) di.ReportDateTime = diReportDateTime.Value;

            var diReportDescription = cdaDocument.SelectSingleNode(diDocNode, _documentXPaths.Get("DIReportDescription"));
            if (diReportDescription != null) di.ReportDescription = diReportDescription.InnerText;

            var diReportStatus = cdaDocument.GetCodableText(diDocNode, _documentXPaths.Get("DIReportStatus"));
            di.ReportStatus = diReportStatus;

            var diReportIntegrityCheck = cdaDocument.SelectSingleNode(diDocNode, _documentXPaths.Get("DIReportIntegrityCheck"));
            if (diReportIntegrityCheck != null) di.ReportIntegrityCheck = diReportIntegrityCheck.Value;

            var diReportMediaType = cdaDocument.SelectSingleNode(diDocNode, _documentXPaths.Get("DIReportMediaType"));
            if (diReportMediaType != null) di.ReportMediaType = diReportMediaType.Value;

            var diReportDocName = cdaDocument.SelectSingleNode(diDocNode, _documentXPaths.Get("DIReportDocName"));
            if (diReportDocName != null) di.ReportDocName = diReportDocName.Value;

            var accessionNumber = cdaDocument.GetId(_documentXPaths.Get("AccessionNumber"));
            di.AccessionNumber = accessionNumber;

            var radXpath = _documentXPaths.Get("Radiologist");
            var radNode = cdaDocument.SelectSingleNode(radXpath);

            var radiologistIdentifier = cdaDocument.SelectSingleNode(radNode, _documentXPaths.Get("RadiologistIdentifier"));
            if (radiologistIdentifier != null) di.RadiologistIdentifier = radiologistIdentifier.Value.Replace("1.2.36.1.2001.1003.0.", "");

            var radiologistOrganisationName = cdaDocument.SelectSingleNode(radNode, _documentXPaths.Get("RadiologistOrganisationName"));
            if (radiologistOrganisationName != null) di.OrganisationName = radiologistOrganisationName.InnerText;

            var radiologistOrganisationIdentifier = cdaDocument.SelectSingleNode(radNode, _documentXPaths.Get("RadiologistOrganisationIdentifier"));
            if (radiologistOrganisationIdentifier != null) di.OrganisationIdentifier = radiologistOrganisationIdentifier.Value.Replace("1.2.36.1.2001.1003.0.", "");

            if (radXpath != null)
            {
                List<Telecom> radiologistContactDetails = cdaDocument.GetTelecoms(radXpath + "/" + _documentXPaths.Get("RadiologistContactDetails"));
                if (radiologistContactDetails != null) di.ContactDetails = radiologistContactDetails;

                var personName = cdaDocument.GetPersonName(radXpath + "/" + _documentXPaths.Get("RadiologistName"));
                if (personName != null) di.Radiologist = personName;
            }

            // 1..* results
            di.ImagingExaminationResults = new List<ImagingExaminationResult>();

            var diTestResults = cdaDocument.SelectNodes(_documentXPaths.Get("DITestResults"));
            if (diTestResults != null && diTestResults.Count > 0)
            {
                foreach (XmlNode results in diTestResults)
                {
                    ImagingExaminationResult ier = new ImagingExaminationResult();
                    ier.ExamResultName = cdaDocument.GetCodableText(results, _documentXPaths.Get("DIExamResultName"));
                    ier.Modality = cdaDocument.GetCodableText(results, _documentXPaths.Get("DIModality"));
                    ier.AnatomicalRegion = cdaDocument.GetCodableText(results, _documentXPaths.Get("DIAnatomicalRegion"));
                    ier.OverallResultStatus = cdaDocument.GetCodableText(results, _documentXPaths.Get("DIOverallResultStatus"));
                    ier.ImageDateTime = cdaDocument.GetDateTimeValue(results, _documentXPaths.Get("DIImageDateTime")).Value;
                    ier.ExaminationProcedure = cdaDocument.SelectSingleNode(results, _documentXPaths.Get("DIExaminationProcedure")).InnerText;
                    ier.ObservationDateTime = cdaDocument.GetDateTimeValue(results, _documentXPaths.Get("DIObservationDateTime")).Value;
                    ier.AnatomicalSites = new List<AnatomicalSite>();

                    var targetSiteCode = cdaDocument.SelectNodes(results, _documentXPaths.Get("DIAnatomicalSite"));

                    foreach (XmlNode anatomicalSite in targetSiteCode)
                    {
                        AnatomicalSite anSite = new AnatomicalSite();
                        anSite.NameOfLocation = cdaDocument.GetCodableText(anatomicalSite as XmlElement);
                        anSite.Side = cdaDocument.GetCodableText(anatomicalSite, _documentXPaths.Get("DIAnatomicalSiteSide"));
                        anSite.Description = anatomicalSite != null ? anSite.NameOfLocation.OriginalText : null;

                        ier.AnatomicalSites.Add(anSite);
                    }

                    di.ImagingExaminationResults.Add(ier);
                }
            }

            return di;
        }
    }
}
