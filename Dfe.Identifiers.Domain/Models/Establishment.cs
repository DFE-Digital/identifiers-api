namespace Dfe.Identifiers.Domain.Models
{
    public class Establishment
    {
        // Primary key
        public long? SK { get; set; }
        
        // Foreign keys
        public string? PK_GIAS_URN { get; set; }
        public long? PK_CDM_ID { get; set; }
        public int? URN { get; set; }
        public long? LocalAuthorityId { get; set; }
        public long? EstablishmentTypeId { get; set; }
        public long? EstablishmentGroupTypeId { get; set; }
        public long? EstablishmentStatusId { get; set; }
        public long? RegionId { get; set; }
        public int? EstablishmentNumber { get; set; }
        public string? EstablishmentName { get; set; }
        public string? UKPRN { get; set; }
        public int? URNAtCurrentFullInspection { get; set; }
        public int? URNAtPreviousFullInspection { get; set; }
        public int? URNAtSection8Inspection { get; set; }
        public string? GORregion { get; set; }
        public string? GORregionCode { get; set; }

        public LocalAuthority? LocalAuthority { get; set; }
        public EstablishmentType? EstablishmentType { get; set; }
    }
}