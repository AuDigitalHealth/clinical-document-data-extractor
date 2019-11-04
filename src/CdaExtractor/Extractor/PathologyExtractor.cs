using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class PathologyExtractor : ICdaExtractor<Pathology>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public PathologyExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public Pathology Extract(CdaXmlDocument cdaDocument)
        {
            var pathology = new Pathology();

            var pathDocNode = cdaDocument.SelectSingleNode(_documentXPaths.Get("PathologyDocument"));

            if (pathDocNode != null)
            {
                var pathologyReportName = cdaDocument.SelectSingleNode(pathDocNode, _documentXPaths.Get("PathologyReportName"));
               if (pathologyReportName != null) pathology.ReportName = pathologyReportName.InnerText;

                var pathologyReportDateTime = cdaDocument.GetDateTimeValue(pathDocNode, _documentXPaths.Get("PathologyReportDateTime"));
                if (pathologyReportDateTime != null) pathology.ReportDateTime = pathologyReportDateTime.Value;

                var pathologyReportStatus = cdaDocument.GetCodableText(pathDocNode, _documentXPaths.Get("PathologyReportStatus"));
                pathology.ReportStatus = pathologyReportStatus;

                var pathologyReportIdentifier = cdaDocument.GetId(pathDocNode, _documentXPaths.Get("PathologyReportIdentifier"));
                pathology.ReportIdentifier = pathologyReportIdentifier;

                var pathologyReportIntegrityCheck = cdaDocument.SelectSingleNode(pathDocNode, _documentXPaths.Get("PathologyReportIntegrityCheck"));
                if (pathologyReportIntegrityCheck != null) pathology.ReportIntegrityCheck = pathologyReportIntegrityCheck.Value;

                var pathologyReportMediaType = cdaDocument.SelectSingleNode(pathDocNode, _documentXPaths.Get("PathologyReportMediaType"));
                if (pathologyReportMediaType != null) pathology.ReportMediaType = pathologyReportMediaType.Value;

                var pathologyReportDocName = cdaDocument.SelectSingleNode(pathDocNode, _documentXPaths.Get("PathologyReportDocName"));
                if (pathologyReportDocName != null) pathology.ReportDocName = pathologyReportDocName.Value;


                var pathXpath = _documentXPaths.Get("Pathologist");
                var pathNode = cdaDocument.SelectSingleNode(pathXpath);

                var pathologistIdentifier = cdaDocument.SelectSingleNode(pathNode, _documentXPaths.Get("PathologistIdentifier"));
                if (pathologistIdentifier != null) pathology.PathologistIdentifier = pathologistIdentifier.Value.Replace("1.2.36.1.2001.1003.0.", "");

                var pathologistOrganisationName = cdaDocument.SelectSingleNode(pathNode, _documentXPaths.Get("PathologistOrganisationName"));
                if (pathologistOrganisationName != null) pathology.OrganisationName = pathologistOrganisationName.InnerText;

                var pathologistOrganisationIdentifier = cdaDocument.SelectSingleNode(pathNode, _documentXPaths.Get("PathologistOrganisationIdentifier"));
                if (pathologistOrganisationIdentifier != null) pathology.OrganisationIdentifier = pathologistOrganisationIdentifier.Value.Replace("1.2.36.1.2001.1003.0.", "");

                if (pathXpath != null)
                {
                    List<Telecom> pathologistContactDetails = cdaDocument.GetTelecoms(pathXpath + "/" + _documentXPaths.Get("PathologistContactDetails"));
                    if (pathologistContactDetails != null) pathology.ContactDetails = pathologistContactDetails;

                    var personName = cdaDocument.GetPersonName(pathXpath + "/" + _documentXPaths.Get("PathologistName"));
                    if (personName != null) pathology.Pathologist = personName;
                }
            }

            // 1..* results
            pathology.PathologyTestResults = new List<PathologyTestResult>();

            var nodes = cdaDocument.SelectNodes(_documentXPaths.Get("PathologyTestResults"));
            if (nodes != null && nodes.Count > 0)
            {
                pathology.PathologyTestResults =
                    (from XmlNode node in nodes
                        select new PathologyTestResult
                        {
                            TestResultName = cdaDocument.GetCodableText(node, _documentXPaths.Get("PathologyTestResultName")),
                            Discipline = cdaDocument.GetCodableText(node, _documentXPaths.Get("PathologyDiscipline")),
                            SpecimenCollectionDateTime = cdaDocument.GetDateTimeValue(node, _documentXPaths.Get("PathologyCollectionDateTime")).Value,
                            ObservationDateTime = cdaDocument.GetDateTimeValue(node, _documentXPaths.Get("PathologyObservationDateTime")).Value,
                            TestResultStatus = cdaDocument.GetCodableText(node, _documentXPaths.Get("PathologyTestResultStatus"))
                        }
                    ).ToList();
            }

            return pathology;
        }
    }
}
