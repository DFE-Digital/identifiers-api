namespace Dfe.Identifiers.Api.Models
{
    public class EducationEstablishmentTrust
    {
        public int SK { get; set; } 

        // Foreign keys
        public int TrustId { get; set; }
        public int EducationEstablishmentId { get; set; }
    }

}
