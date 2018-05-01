/*
 * Copyright 2012 NEHTA
 *
 * Licensed under the NEHTA Open Source (Apache) License; you may not use this
 * file except in compliance with the License. A copy of the License is in the
 * 'license.txt' file, which should be provided with this work.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Nehta.VendorLibrary.CdaExtractor;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace CdaExtractor.Sample
{
    /// <summary>
    /// Sample to demonstrate how extract CDA xml data from CDA document(s) 
    /// </summary>
    public class CdaExtractorSample
    {
        /// <summary>
        /// Load a single CDA Document and extract the data and serialize to a XML file
        /// </summary>
        public void LoadSingleCdaDocument()
        {
            try
            {
                XmlDocument cdaDocument = LoadDocument("CdaDocumentPath.xml");
                ICdaDataExtractor cdaDataExtractor = new CdaDataExtractor();
                CdaDocument uberModel = cdaDataExtractor.Extract(cdaDocument);

                // serialize to a XML file
                var result = Serialize(uberModel);
                File.WriteAllBytes("uber.xml", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Load a multiple CDA Documents and extract the Metadata to a list and serialize to a XML file
        /// </summary>
        public void LoadMultipleCdaDocuments()
        {
            try
            {
                List<XmlDocument> cdaDocuments = LoadDocuments(new List<string> { "CdaDocumentPath1.xml", "CdaDocumentPath2.xml" });
                ICdaDataExtractor cdaDataExtractor = new CdaDataExtractor();
                List<CdaDocument> uberModel = cdaDataExtractor.Extract(cdaDocuments);

                // serialize to a XML file
                var result = Serialize(uberModel);
                File.WriteAllBytes("uber_multiple.xml", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region Helper Functions

        private static List<XmlDocument> LoadDocuments(IEnumerable<string> cdaDocuments)
        {
            return cdaDocuments.Select(LoadDocument).ToList();
        }

        private static XmlDocument LoadDocument(string path)
        {
            var document = new XmlDocument();
            document.Load(path);
            return document;
        }

        private static byte[] Serialize<T>(T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            var memoryStream = new MemoryStream();
            serializer.Serialize(memoryStream, data);
            return memoryStream.ToArray();
        }

        #endregion

    }
}