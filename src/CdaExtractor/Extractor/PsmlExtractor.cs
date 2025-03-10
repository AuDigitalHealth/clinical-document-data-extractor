using System.Collections.Generic;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class PsmlExtractor : ICdaExtractor<Psml>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public PsmlExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public Psml Extract(CdaXmlDocument cdaDocument)
        {
            var psml = new Psml();

            var attachmentNode = cdaDocument.SelectSingleNode(_documentXPaths.Get("Attachment"));

            if (attachmentNode != null)
            {
                var attachmentIntegrityCheck = cdaDocument.SelectSingleNode(attachmentNode, _documentXPaths.Get("AttachmentIntegrityCheck"));
                if (attachmentIntegrityCheck != null) psml.ReportIntegrityCheck = attachmentIntegrityCheck.Value;

                var attachmentMediaType = cdaDocument.SelectSingleNode(attachmentNode, _documentXPaths.Get("AttachmentMediaType"));
                if (attachmentMediaType != null) psml.ReportMediaType = attachmentMediaType.Value;

                var attachmentName = cdaDocument.SelectSingleNode(attachmentNode, _documentXPaths.Get("AttachmentName"));
                if (attachmentName != null) psml.ReportDocName = attachmentName.Value;
            }

            return psml;
        }
    }
}
