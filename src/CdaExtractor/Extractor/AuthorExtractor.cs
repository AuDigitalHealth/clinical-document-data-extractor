using System.Collections.Generic;
using Nehta.VendorLibrary.CdaExtractor.Model;

namespace Nehta.VendorLibrary.CdaExtractor.Extractor
{
    public class AuthorExtractor : ICdaExtractor<Author>
    {
        private readonly IDictionary<string, string> _documentXPaths;

        public AuthorExtractor(IDictionary<string, string> documentXPaths)
        {
            _documentXPaths = documentXPaths;
        }

        public Author Extract(CdaXmlDocument cdaDocument)
        {
            // Author name
            PersonName authorName = null;
            PersonName personName = cdaDocument.GetPersonName(_documentXPaths.Get("AuthorPersonName"));
            if (personName != null)
            {
                authorName = personName;
            }
            
            // Telecom
            List<Telecom> telecoms = cdaDocument.GetTelecoms(_documentXPaths.Get("AuthorTelecoms"));
            
            // Author HPII
            string hpii = null;
            var idRootAttrib = cdaDocument.SelectSingleNode(_documentXPaths.Get("AuthorHpii"));  
            if (idRootAttrib != null)
            {
                hpii = idRootAttrib.Value;
                hpii = hpii.Substring(hpii.Length - 16);
            }

            // Author IHI
            string ihi = null;
            var ihiRootAttrib = cdaDocument.SelectSingleNode(_documentXPaths.Get("AuthorIhi"));
            if (ihiRootAttrib != null)
            {
                ihi = ihiRootAttrib.Value;
                ihi = ihi.Substring(ihi.Length - 16);
            }
            
            // Organisation name
            string organisationName = null;
            var organisationNameElem = cdaDocument.SelectSingleNode(_documentXPaths.Get("AuthorOrganisationName"));
            if (organisationNameElem != null)
            {
                organisationName = organisationNameElem.InnerText;
            }

            // Organisation HPIO
            string hpio = null;
            var organisationIdAttrib = cdaDocument.SelectSingleNode(_documentXPaths.Get("AuthorOrganisationHpio"));
            if (organisationIdAttrib != null)
            {
                hpio = organisationIdAttrib.Value;
                hpio = hpio.Substring(hpio.Length - 16);
            }

            var authorOrg = new Author
            {
                AuthorName = authorName,
                AuthorHpii = hpii,
                AuthorIhi = ihi,
                OrganisationName = organisationName,
                OrganisationHpio = hpio,
                Telecoms = telecoms
                
            };

            return authorOrg;
        }
    }
}