using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Nehta.VendorLibrary.CdaExtractor.Configuration;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class SubjectOfCareExtractor : ICdaExtractor<SubjectOfCare>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public SubjectOfCareExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public SubjectOfCare Extract(CdaXmlDocument cdaDocument)
        {
            var subjectOfCare = new SubjectOfCare();

            var patientNode = cdaDocument.SelectSingleNode(_documentXPaths.Get("SubjectOfCare"));

            var identifier = cdaDocument.SelectSingleNode(patientNode, _documentXPaths.Get("SubjectOfCareIhi"));

            string ihi = null;
            if (identifier != null && identifier.Attributes != null && identifier.Attributes["root"] != null)
            {
                ihi =  identifier.Attributes["root"].InnerText;
            }

            if (!String.IsNullOrWhiteSpace(ihi) && ihi.Contains("."))
            {
                ihi = ihi.Split('.').Last();
            }

            subjectOfCare.Ihi = ihi;

            var gender = cdaDocument.GetCodableText(patientNode, _documentXPaths.Get("SubjectOfCareAdministrativeGenderCode"));

            if (gender != null)
            {
                subjectOfCare.AdmnistrativeGenderCode = gender.Code;
            }

            var birthTime = cdaDocument.GetDateTimeValue(patientNode, _documentXPaths.Get("SubjectOfCareBirthTime"));

            subjectOfCare.BirthTime = birthTime == null ? null as DateTime? : birthTime.Value;

            var personName = cdaDocument.GetPersonName(_documentXPaths.Get("SubjectOfCarePersonName"));
            subjectOfCare.PersonName = personName;

            subjectOfCare.MedicareNumber = cdaDocument.GetString(_documentXPaths.Get("MedicareNumberEntityIdentifier"));

            if (string.IsNullOrWhiteSpace(subjectOfCare.MedicareNumber))
            {
                subjectOfCare.MedicareNumber = cdaDocument.GetString(_documentXPaths.Get("MedicareNumberEntitlements"));
            }

            subjectOfCare.DvaNumber = cdaDocument.GetString(_documentXPaths.Get("DvaNumber"));

            return subjectOfCare;
        }
    }
}
