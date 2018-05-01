using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Nehta.VendorLibrary.CdaExtractor.Configuration
{
    [Serializable]
    public class ConfigModel
    {

        public List<Document> Documents { get; set; }

        public static ConfigModel Load()
        {
            return Load("config.xml");
        }

        public static ConfigModel Load(string configFilename)
        {
            if(String.IsNullOrWhiteSpace(configFilename))
                throw new ArgumentException("XML configuration file must be specified for CdaExtractor");

            var configModel = new ConfigModel();
            var xmlSerializer = new XmlSerializer(configModel.GetType());

            var streamReader = new StreamReader(configFilename);
            object deserializedConfigModel = xmlSerializer.Deserialize(streamReader.BaseStream);
            streamReader.Close();

            return (ConfigModel)deserializedConfigModel;
        }
    }
}