namespace Dfe.Identifiers.Domain.Models
{
    public class Trust
    {
        // Primary key
        public long? SK { get; set; }
        public long? TrustTypeId { get; set; }
        public long? RegionId { get; set; }
        public long? TrustBandingId { get; set; }
        public long? TrustStatusId { get; set; }
        public string? GroupUID { get; set; }
        public string? GroupID { get; set; }
        public string? RID { get; set; }
        public string? Name { get; set; }
        public string? CompaniesHouseNumber { get; set; }
        public string? TrustStatus { get; set; }
        public string? UKPRN { get; set; }
        public string? UPIN { get; set; }
        public TrustType? TrustType { get; set; }
    }
}