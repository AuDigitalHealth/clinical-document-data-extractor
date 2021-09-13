using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class DocumentMetadataExtractor : ICdaExtractor<DocumentMetadata>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        private DateTime _effectiveDateTime;
        private DateTime _eventStartDateTime;
        private DateTime _eventEndDateTime;

        public DocumentMetadataExtractor(IDictionary<string, string>  documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public DocumentMetadata Extract(CdaXmlDocument cdaDocument)
        {
            // Document code
            var documentCode = cdaDocument.GetCodableText(_documentXPaths.Get("DocumentCode"));

            // Effective time - if this fails, there is a problem!
            var effectiveTime = cdaDocument.GetDateTimeValue(_documentXPaths.Get("EffectiveTime"));
            _effectiveDateTime = effectiveTime.HasValue ? effectiveTime.Value : DateTime.Now;

            // For Certain Documents get a different DateTime
            SetDateTimes(cdaDocument, documentCode.Code);

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
                EffectiveTime = _effectiveDateTime,
                EventStartDateTime = _eventStartDateTime,
                EventEndDateTime = _eventEndDateTime,
                DocumentId = documentId,
                Title = title,
                LinkId = linkId
            };

            return documentMetadata;
        }

        void SetDateTimes(CdaXmlDocument cdaDocument, string documentTypeCode)
        {
            // Set service start and stop time
            if (documentTypeCode == "51852-2")
            {
                // If document is Specialist Letter
                _eventStartDateTime = _effectiveDateTime;
                _eventEndDateTime = _effectiveDateTime;
            }
            else if (documentTypeCode == "100.16764")
            {
                // If document is Prescription Record
                var authorTime = cdaDocument.GetDateTimeValue("/cda:ClinicalDocument/cda:author/cda:time/@value");

                _eventStartDateTime = authorTime.HasValue?  authorTime.Value : _effectiveDateTime;
                _eventEndDateTime = _eventStartDateTime;
            }
            else if (documentTypeCode == "100.16765")
            {
                // If document is PCEHR Dispense Record
                var supplyTime = cdaDocument.GetDateTimeValue("/cda:ClinicalDocument/cda:component/cda:structuredBody/" +
                    "cda:component/cda:section[cda:code/@code='102.16210' and cda:code/@codeSystem='1.2.36.1.2001.1001.101']/cda:entry/" +
                    "cda:substanceAdministration/cda:entryRelationship/cda:supply/cda:_effectiveDateTime/@value");

                _eventStartDateTime = supplyTime.HasValue ? supplyTime.Value : _effectiveDateTime;
                _eventEndDateTime = _eventStartDateTime;
            }
            else if (documentTypeCode == "100.32001")
            {
                // R5: If document is Pathology Report - Could contain multiple records
                // Need to get LATEST Date - if multiple returned

                XmlNodeList nList = cdaDocument.SelectNodes("/cda:ClinicalDocument/cda:component/cda:structuredBody/cda:component" +
                                                                                     "/cda:section[cda:code/@code='101.20018']/cda:component" +
                                                                                     "/cda:section[cda:code/@code='102.16144']/cda:entry/cda:observation" +
                                                                                     "/cda:entryRelationship/cda:observation[cda:code/@code='102.16156']" +
                                                                                     "/cda:effectiveTime/@value");

                var firstCollectionDateTime = "";
                var lastCollectionDateTime = "";
                if (nList.Count == 1)
                {
                    firstCollectionDateTime = nList[0].Value;
                    lastCollectionDateTime = nList[0].Value;
                }
                else if (nList.Count > 1)
                {
                    string[] sDates = new string[nList.Count];
                    for (int i = 0; i < nList.Count; i++)
                    {
                        sDates[i] = nList[i].Value;
                    }
                    Array.Sort(sDates);
                    //Get last entry
                    firstCollectionDateTime = sDates[0];
                    lastCollectionDateTime = sDates[nList.Count - 1];
                }

                _eventStartDateTime = !string.IsNullOrWhiteSpace(firstCollectionDateTime)
                    ? cdaDocument.GetDateTime(firstCollectionDateTime).Value
                    : _effectiveDateTime;
                _eventEndDateTime = !string.IsNullOrWhiteSpace(lastCollectionDateTime)
                    ? cdaDocument.GetDateTime(lastCollectionDateTime).Value
                    : _effectiveDateTime;
            }
            else if (documentTypeCode == "100.16957")
            {
                // R5: If document is Diagnostic Imaging - Could contain multiple records
                // Need to get LATEST Date - if multiple returned

                XmlNodeList nList = cdaDocument.SelectNodes("/cda:ClinicalDocument/cda:component/cda:structuredBody/cda:component" +
                                                                                  "/cda:section/cda:component/cda:section/cda:entry/cda:observation" +
                                                                                  "/cda:entryRelationship/cda:act[cda:code/@code='102.16511']/cda:entryRelationship" +
                                                                                  "/cda:observation/cda:effectiveTime/@value");
                var firstImagingDateTime = "";
                var lastImagingDateTime = "";
                if (nList.Count == 1)
                {
                    firstImagingDateTime = nList[0].Value;
                    lastImagingDateTime = nList[0].Value;
                }
                else if (nList.Count > 1)
                {
                    string[] sDates = new string[nList.Count];
                    for (int i = 0; i < nList.Count; i++)
                    {
                        sDates[i] = nList[i].Value;
                    }
                    Array.Sort(sDates);
                    //Get last entry
                    firstImagingDateTime = sDates[0];
                    lastImagingDateTime = sDates[nList.Count - 1];
                }

                _eventStartDateTime = !string.IsNullOrWhiteSpace(firstImagingDateTime)
                    ? cdaDocument.GetDateTime(firstImagingDateTime).Value
                    : _effectiveDateTime;
                _eventEndDateTime = !string.IsNullOrWhiteSpace(lastImagingDateTime)
                    ? cdaDocument.GetDateTime(lastImagingDateTime).Value
                    : _effectiveDateTime;
            }
            else if (documentTypeCode == "100.16975")
            {
                // Advance Care Planning Document
                var authorTime = cdaDocument.GetDateTimeValue("/cda:ClinicalDocument/cda:component/cda:structuredBody/cda:component/cda:section[cda:code/@code='101.16973']/cda:entry/cda:act/cda:author/cda:time/@value");
                _eventStartDateTime = authorTime.Value;
                _eventEndDateTime = _eventStartDateTime;
            }
            else
            {
                // For other document types set service start and stop time to encompassingEncounter/_effectiveDateTime if available
                var startTime1 = cdaDocument.GetDateTimeValue("/cda:ClinicalDocument/cda:componentOf/cda:encompassingEncounter/cda:_effectiveDateTime/@value");
                var startTime2 = cdaDocument.GetDateTimeValue("/cda:ClinicalDocument/cda:componentOf/cda:encompassingEncounter/cda:_effectiveDateTime/cda:low/@value");
                var stopTime = cdaDocument.GetDateTimeValue("/cda:ClinicalDocument/cda:componentOf/cda:encompassingEncounter/cda:_effectiveDateTime/cda:high/@value");

                _eventStartDateTime = startTime1.HasValue
                                       ? startTime1.Value
                                       : startTime2.HasValue
                                             ? startTime2.Value
                                             : _effectiveDateTime;

                _eventEndDateTime = stopTime.HasValue
                                      ? stopTime.Value
                                      : startTime1.HasValue
                                            ? startTime1.Value
                                            : _effectiveDateTime;

            }
        }

    }
}