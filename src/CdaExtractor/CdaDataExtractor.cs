using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Extractor;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor
{
    public class CdaDataExtractor : ICdaDataExtractor
    {
        private const string DefaultFilename = "config.xml";

        private readonly IDocumentXPathProvider _documentXPathProvider;


        public CdaDataExtractor() : this(new FileDocumentXPathProvider(DefaultFilename))
        {
        }

        public CdaDataExtractor(string filepath) : this(new FileDocumentXPathProvider(filepath))
        {
        }

        public CdaDataExtractor(IDocumentXPathProvider documentXPathProvider)
        {
            _documentXPathProvider = documentXPathProvider;
        }

        public CdaDocument Extract(XmlDocument cdaDocument)
        {
            if (cdaDocument == null) throw new ArgumentException("'cdaDocument' cannot be null");

            var cdaXmlDocument = new CdaXmlDocument(cdaDocument);

            IDictionary<string, string> documentXPaths = _documentXPathProvider.GetDocumentXPaths(cdaXmlDocument.TemplateId);
            if (documentXPaths == null)
            {
                throw new ArgumentException("Document with template ID '" + cdaXmlDocument.TemplateId + "' not supported");
            }

            // Extractors
            var authorOrgExtractor = new AuthorExtractor(documentXPaths);
            var author = authorOrgExtractor.Extract(cdaXmlDocument);

            var medicationsExtractor = new MedicationsExtractor(documentXPaths);
            var medications = medicationsExtractor.Extract(cdaXmlDocument);

            var adverseReactionsEx = new AdverseReactionsExtractor(documentXPaths);
            var adverseReactions = adverseReactionsEx.Extract(cdaXmlDocument);

            var alertsEx = new AlertsExtractor(documentXPaths);
            var alerts = alertsEx.Extract(cdaXmlDocument);

            var interventionsEx = new InterventionExtractor(documentXPaths);
            var interventions = interventionsEx.Extract(cdaXmlDocument);

            var documentMetadataExtractor = new DocumentMetadataExtractor(documentXPaths);
            var documentMetadata = documentMetadataExtractor.Extract(cdaXmlDocument);

            var medicalHistoryExtractor = new MedicalHistoryExtractor(documentXPaths);
            var medicalHistoryData = medicalHistoryExtractor.Extract(cdaXmlDocument);

            var subjectOfCareExtractor = new SubjectOfCareExtractor(documentXPaths);
            var subjectOfCare = subjectOfCareExtractor.Extract(cdaXmlDocument);

            var immunisationsEx = new ImmunisationsExtractor(documentXPaths);
            var immunisations = immunisationsEx.Extract(cdaXmlDocument);

            var immunisationRegisterEx = new ImmunisationRegisterExtractor(documentXPaths);
            var immunisationRegister = immunisationRegisterEx.Extract(cdaXmlDocument);

            var consumerNoteEx = new ConsumerNoteExtractor(documentXPaths);
            var consumerNote = consumerNoteEx.Extract(cdaXmlDocument);

            var advanceCareInformationExtractor = new AdvanceCareInformationExtractor(documentXPaths);
            var advanceCareInformation = advanceCareInformationExtractor.Extract(cdaXmlDocument);

            var pbsExtractor = new PharmaceuticalBenefitItemExtractor(documentXPaths);
            var pbs = pbsExtractor.Extract(cdaXmlDocument);

            var diagnosticImagingExtractor = new DiagnosticImagingExtractor(documentXPaths);
            var diagnosticImaging = diagnosticImagingExtractor.Extract(cdaXmlDocument);

            var pathologyExtractor = new PathologyExtractor(documentXPaths);
            var pathology = pathologyExtractor.Extract(cdaXmlDocument);

            var psmlExtractor = new PsmlExtractor(documentXPaths);
            var psml = psmlExtractor.Extract(cdaXmlDocument);

            var clinicalSynopsisExtractor= new ClinicalSynopsisExtractor(documentXPaths);
            var clinicalSynopsis = clinicalSynopsisExtractor.Extract(cdaXmlDocument);
            

            var document = new CdaDocument
            {
                Author = author,
                Medications = medications,
                AdverseReactions = adverseReactions,
                Alerts = alerts,
                Interventions = interventions,
                MedicalHistoryItems = medicalHistoryData,
                DocumentMetadata = documentMetadata,
                SubjectOfCare = subjectOfCare,
                ImmunisationItems = immunisations,
                ImmunisationRegisterItems = immunisationRegister,
                ConsumerNote = consumerNote,
                AdvanceCareInformation = advanceCareInformation,
                PharmaceuticalBenefitItems = pbs,
                Pathology = pathology,
                DiagnosticImaging = diagnosticImaging,
                Psml = psml,
                ClinicalSynopsis = clinicalSynopsis
            };

            return document;
        }

        public List<CdaDocument> Extract(IList<XmlDocument> cdaDocuments)
        {
            if (cdaDocuments == null || cdaDocuments.Count == 0) throw new ArgumentException("'cdaDocuments' cannot be null or empty");

            return cdaDocuments.AsParallel().Select(Extract).ToList(); 
        }
    }
}