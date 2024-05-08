namespace Dfe.Identifiers.Domain.Models;

public class ConversionProject
{
    public int? Id { get; set; }
    public int URN { get; set; }
    public string? SchoolName { get; set; }
    public string? ApplicationReferenceNumber { get; set; }
    public string? TrustReferenceNumber { get; set; }
    public string? SponsorReferenceNumber { get; set; }
    
    // Foreign Key
    public int? FormAMatProjectId { get; set; }
    
    public FormAMatProject? FormAMatProject { get; set; }
}