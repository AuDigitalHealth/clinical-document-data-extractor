using System;
using System.Collections.Generic;
using System.Linq;
using Nehta.VendorLibrary.CdaExtractor.Configuration;

namespace Nehta.VendorLibrary.CdaExtractor
{
    public class FileDocumentXPathProvider : IDocumentXPathProvider
    {
        private readonly Dictionary<string, Dictionary<string, string>> _dictionaries;


        public FileDocumentXPathProvider() : this("config.xml")
        {            
        }

        public FileDocumentXPathProvider(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("XML configuration file must be specified for CdaExtractor");

            ConfigModel configModel = ConfigModel.Load(filePath);

            _dictionaries = configModel.Documents.ToDictionary(
                document => document.TemplateId,
                document => document.FieldMappings.ToDictionary(mapping => mapping.Name, mapping => mapping.XPath));
        }

        public IDictionary<string, string> GetDocumentXPaths(string templateId)
        {
            if (string.IsNullOrEmpty(templateId))
            {
                return null;
            }

            if (!_dictionaries.ContainsKey(templateId))
            {
                return null;
            }

            return _dictionaries[templateId];
        }
    }
}