using System;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class SubjectOfCare
    {
        public string Ihi { get; set; }
        //public string FamilyName { get; set; }
        //public string[] GivenName { get; set; }
        public string AdmnistrativeGenderCode { get; set; }
        public DateTime? BirthTime { get; set; }
        public string MedicareNumber { get; set; }
        public string DvaNumber { get; set; }
        public PersonName PersonName { get; set; }
    }
}
