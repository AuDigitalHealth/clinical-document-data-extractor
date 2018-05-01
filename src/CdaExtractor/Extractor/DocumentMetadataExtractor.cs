using System.Collections.Generic;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class DocumentMetadataExtractor : ICdaExtractor<DocumentMetadata>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public DocumentMetadataExtractor(IDictionary<string, string>  documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public DocumentMetadata Extract(CdaXmlDocument cdaDocument)
        {
            // Document code
            var documentCode = cdaDocument.GetCodableText(_documentXPaths.Get("DocumentCode"));

            // Effective time
            var effectiveDateTime = cdaDocument.GetDateTimeValue(_documentXPaths.Get("EffectiveTime"));

            // Document ID
            var documentId = cdaDocument.GetId(_documentXPaths.Get("DocumentId"));

            // Document Title from title field
            string title = null;
            var titleNode = cdaDocument.SelectSingleNode(_documentXPaths.Get("Title"));
            if (titleNode != null)
            {
                title = titleNode.InnerText;
            }

            Id linkId = cdaDocument.GetId(_documentXPaths.Get("LinkId"));

            var documentMetadata = new DocumentMetadata
            {
                TemplateId = cdaDocument.TemplateId,
                DocumentCode = documentCode,
                EffectiveTime = effectiveDateTime,
                DocumentId = documentId,
                Title = title,
                LinkId = linkId
            };

            return documentMetadata;
        }
    }
}