using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nehta.VendorLibrary.CdaExtractor.Configuration
{
    //A class that encapsulates access to configuration items bootstraped from the config.xml file
    //singleton implementation as per http://msdn.microsoft.com/en-us/library/ff650316.aspx
    public sealed class Config
    {
        private static readonly Config instance = new Config();
       
        private readonly Dictionary<string, Dictionary<string, string>> _dictionaries;

        private Config()
        {
            _dictionaries = GetDictionList();
        }

        public static Config Instance
        {
            get { return instance; }
        }

        public IDictionary<string, string> GetDocumentXPaths(string templateId)
        {
            if (!_dictionaries.ContainsKey(templateId))
            {
                return null;
            }

            return _dictionaries[templateId];
        }


        private Dictionary<string, Dictionary<string, string>> GetDictionList()
        {

            ConfigModel m = ConfigModel.Load();

            var dictionaries = m.Documents.ToDictionary(
                  document => document.TemplateId,
                  document => document.FieldMappings.ToDictionary(mapping => mapping.Name, mapping => mapping.XPath)
                  );

            return dictionaries;
        }
    } 
}
