using System.Collections.Generic;

namespace Nehta.VendorLibrary.CdaExtractor.Model
{
    public class CdaDocument
    {
        public DocumentMetadata DocumentMetadata { get; set; }
        public Author Author { get; set; }
        public SubjectOfCare SubjectOfCare { get; set; }
        public List<AdverseReaction> AdverseReactions { get; set; }
        public List<Medication> Medications { get; set; }
        public List<PharmaceuticalBenefitItem> PharmaceuticalBenefitItems { get; set; }
        public List<MedicalHistory> MedicalHistoryItems { get; set; }
        public List<Immunisation> ImmunisationItems { get; set; }
        public ConsumerNote ConsumerNote { get; set; }
        public AdvanceCareInformation AdvanceCareInformation { get; set; }
    }
}