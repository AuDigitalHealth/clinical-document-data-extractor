using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Helper;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor
{
    public class CdaXmlDocument
    {
        private const string TemplateIdXpath = "/cda:ClinicalDocument/cda:templateId/@root[starts-with(., '1.2.36.1.2001.1001.101') or starts-with(., '1.2.36.1.2001.1001.100.1002')]";
        private const string CdaNamespace = "urn:hl7-org:v3";
        private const string CdaExtensionNamespace = "http://ns.electronichealth.net.au/Ci/Cda/Extensions/3.0";

        private readonly XmlDocument _xmlDocument;
        private readonly XmlNamespaceManager _xmlNamespaceManager;
        public string TemplateId { get; private set; }

        public CdaXmlDocument(XmlDocument cdaDocument)
        {
            _xmlDocument = cdaDocument;
            _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace("cda", CdaNamespace);
            _xmlNamespaceManager.AddNamespace("ext", CdaExtensionNamespace);

            SetTemplateId();
        }

        private void SetTemplateId()
        {
            var templateIdRootAttrib = SelectSingleNode(TemplateIdXpath) as XmlAttribute;
            if (templateIdRootAttrib == null)
            {
                throw new ArgumentException("CDA document does not contain a valid template ID");
            }

            TemplateId = templateIdRootAttrib.Value;
        }

        public XmlNode SelectSingleNode(string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return null;
            }
            return _xmlDocument.SelectSingleNode(xpath, _xmlNamespaceManager);
        }

        public XmlNodeList SelectNodes(string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return null;
            }
            return _xmlDocument.SelectNodes(xpath, _xmlNamespaceManager);
        }

        public XmlNode SelectSingleNode(XmlNode node, string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return null;
            }
            return node.SelectSingleNode(xpath, _xmlNamespaceManager);
        }

        public XmlNodeList SelectNodes(XmlNode node, string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return null;
            }
            return node.SelectNodes(xpath, _xmlNamespaceManager);
        }

        public CodableText GetCodableText(string xpath)
        {
            var codeElem = SelectSingleNode(xpath) as XmlElement;
            if (codeElem == null)
            {
                return null;
            }
            return GetCodableText(codeElem);
        }

        public CodableText GetCodableText(XmlNode node, string xPath)
        {
            var codeElem = SelectSingleNode(node, xPath) as XmlElement;
            if (codeElem == null)
            {
                return null;
            }
            return GetCodableText(codeElem);
        }

        public CodableText GetCodableText(XmlElement codeElement)
        {
            if (codeElement == null)
                return null;

            var codableText = new CodableText();

            // Get 'originalText'
            var originalText  = codeElement.GetElementsByTagName("originalText");
            if (originalText.Count == 1)
            {
                codableText.OriginalText = originalText[0].InnerText;
            }

            codableText.Code = GetString(codeElement.Attributes["code"]);
            codableText.CodeSystem = GetString(codeElement.Attributes["codeSystem"]);
            codableText.CodeSystemName = GetString(codeElement.Attributes["codeSystemName"]);
            codableText.CodeSystemVersion = GetString(codeElement.Attributes["codeSystemVersion"]);
            codableText.DisplayName = GetString(codeElement.Attributes["displayName"]);
            codableText.NullFlavor = GetString(codeElement.Attributes["nullFlavor"]);

            // Get 'translations'
            var translationNodes = codeElement.GetElementsByTagName("translation");
            if (translationNodes.Count > 0)
            {
                List<CodableText> translations = new List<CodableText>();
                foreach (XmlElement translationElem in codeElement.GetElementsByTagName("translation"))
                {
                    translations.Add(GetCodableText(translationElem));
                }

                codableText.Translations = translations;
            }
           
            return codableText;
        }

        public IList<CodableText> GetListOfCodableText(string xpath)
        {
            if (string.IsNullOrEmpty(xpath)) return null;

            var nodes = SelectNodes(xpath);

            if (nodes == null) return null;


            return (from XmlElement node in nodes select GetCodableText(node)).ToList();
        }

        public string GetString(XmlAttribute attribute)
        {
            return attribute == null ? null : attribute.Value;
        }

        public String GetString(string xpath)
        {
            var node = SelectSingleNode(xpath);
            return node == null ? null : node.Value;
        }

        public String GetString(XmlNode node, string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
            {
                return null;
            }

            var textNode = node.SelectSingleNode(xpath, _xmlNamespaceManager);
            return textNode != null ? textNode.InnerText : null;
        }

        public IList<String> GetListOfString(string xpath)
        {
            var nodes = SelectNodes(xpath);

            if (nodes == null || nodes.Count == 0) return null;

            return (from XmlNode node in nodes select node.Value).ToList();
        }


        public CodableText GetRelativeCode(XmlNode node, string xpath)
        {
            if (string.IsNullOrEmpty(xpath) || node == null) return null;
            var innerNode = SelectSingleNode(node, xpath) as XmlElement;
            return GetCodableText(innerNode);
        }

        public List<CodableText> GetListOfRelativeCodableText(XmlNode node, string xpath)
        {
            if (string.IsNullOrEmpty(xpath) || node == null) return null;

            var nodes = SelectNodes(node,xpath);

            if (nodes == null) return null;


            return (from XmlElement innerNode in nodes select GetCodableText(innerNode)).ToList();
        }

        public Id GetId(string xpath)
        {
            var node = SelectSingleNode(xpath) as XmlElement;
            
            if (node == null) return null;
            if(node.HasAttribute("nullFlavor") ) return null;

            return new Id
            {
                Root =  GetString(node.Attributes["root"]),
                Extension = GetString(node.Attributes["extension"])
            };
        }

        public Id GetId(XmlNode node, string xpath)
        {
            var textNode = node.SelectSingleNode(xpath, _xmlNamespaceManager) as XmlElement;

            if (textNode == null) return null;
            if (textNode.HasAttribute("nullFlavor")) return null;

            return new Id
            {
                Root = GetString(textNode.Attributes["root"]),
                Extension = GetString(textNode.Attributes["extension"])
            };
        }

        #region DateTime / Interval Functions

        public DateTime? GetDateTimeValue(string xpath)
        {
            return string.IsNullOrWhiteSpace(xpath) ? null : DateTimeHelper.ConvertISO8601DateTimeStringToDateTime(GetString(xpath));
        }

        public DateTime? GetDateTimeValue(XmlNode node, string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
            {
                return null;
            }

            var value = GetString(node, xpath);
            return value == null ? null : DateTimeHelper.ConvertISO8601DateTimeStringToDateTime(value);
        }

        public Interval GetInterval(string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return null;
            }
            else
            {
                var intervalNode = SelectSingleNode(xpath);
                return intervalNode == null ? null : GetInterval(intervalNode);
            }
        }

        public Interval GetInterval(XmlNode node, string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return null;
            }
            else
            {
                var intervalNode = SelectSingleNode(node, xpath);
                return intervalNode == null ? null : GetInterval(intervalNode);
            }
        }

        public Interval GetInterval(XmlNode node)
        {
            if (node == null)
            {
                return null;
            }
            else
            {
                var interval = new Interval();

                // Low element
                var low = GetString(node, "cda:low/@value");
                if (!string.IsNullOrWhiteSpace(low))
                {
                    interval.Start = DateTimeHelper.ConvertISO8601DateTimeStringToDateTime(low);
                }

                // High element
                var high = GetString(node, "cda:high/@value");
                if (!string.IsNullOrWhiteSpace(high))
                {
                    interval.End = DateTimeHelper.ConvertISO8601DateTimeStringToDateTime(high);
                }

                return interval;
            }
        }

        public List<Telecom> GetTelecoms(string telecomXpath)
        {
            var telecomNodes = SelectNodes(telecomXpath);

            var telecoms = new List<Telecom>();

            if (telecomNodes != null)
            {
                for (int x = 0; x < telecomNodes.Count; x++)
                {
                    var node = telecomNodes[x];
                
                    telecoms.Add(new Telecom()
                    {
                        Use = node.Attributes["use"].Value,
                        Value = node.Attributes["value"].Value
                    });
                }
                
            }

            return telecoms;
        }

        public PersonName GetPersonName(string nameXpath)
        {
            var nameNodes = SelectNodes(nameXpath); // NOTE selects the first name it finds
            if (nameNodes == null)
            {
                return null;
            }

            XmlNode nameNode = GetPreferredName(nameNodes);
            if (nameNode == null)
            {
                return null;
            }

            var cdaPersonName = new PersonName();

            var familyElem = SelectSingleNode(nameNode, "cda:family") as XmlElement;
            if (familyElem != null)
            {
                cdaPersonName.Family = familyElem.InnerText;
            }

            var givenNamesElem = SelectNodes(nameNode, "cda:given");
            if (givenNamesElem != null && givenNamesElem.Count > 0)
            {
                cdaPersonName.Given = givenNamesElem[0].InnerText; // NOTE takes the first given name
            }

            var prefixElem = SelectSingleNode(nameNode, "cda:prefix");
            if (prefixElem != null)
            {
                cdaPersonName.Prefix = prefixElem.InnerText;
            }

            return cdaPersonName;
        }

        private static XmlNode GetPreferredName(XmlNodeList nameNodes)
        {
            if (nameNodes == null)
            {
                return null;
            }

            // Try to find a name with the usage 'L'
            foreach (XmlElement nameElement in nameNodes)
            {
                var useAttrib = nameElement.Attributes["use"];
                if (useAttrib != null && useAttrib.Value.StartsWith("L"))
                {
                    return nameElement;
                }
            }

            // No names found with usage 'L' so just return the first
            return nameNodes[0];
        }

        #endregion
    }

    public static class Extensions
    {
        //Get the value specified by the key or null
        //will nto throw Exception
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue ret;
            // Ignore return value
            dictionary.TryGetValue(key, out ret);
            return ret;
        }
    }
}