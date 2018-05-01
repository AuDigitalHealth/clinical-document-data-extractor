using System.Collections.Generic;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class ConsumerNoteExtractor : ICdaExtractor<ConsumerNote>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public ConsumerNoteExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public ConsumerNote Extract(CdaXmlDocument cdaDocument)
        {
            var noteTitleNode = cdaDocument.SelectSingleNode(_documentXPaths.Get("NoteTitle"));
            var noteDescription = cdaDocument.SelectSingleNode(_documentXPaths.Get("NoteDescription"));

            if (noteTitleNode == null && noteDescription == null)
            {
                return null;
            }

            var consumerNote = new ConsumerNote
            {
                Title = noteTitleNode != null ? noteTitleNode.InnerText : null,
                Description = noteDescription != null ? noteDescription.InnerText : null,
            };

            return consumerNote;
        }
    }
}

 