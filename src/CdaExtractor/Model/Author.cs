using System.Collections.Generic;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class Author
    {
        public PersonName AuthorName { get; set; }
        public string AuthorHpii { get; set; }
        public string AuthorIhi { get; set; }
        public List<Telecom> Telecoms { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationHpio { get; set; }
    }
}