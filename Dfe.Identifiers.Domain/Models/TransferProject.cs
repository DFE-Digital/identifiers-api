namespace Dfe.Identifiers.Domain.Models;

public class TransferProject
{
    public int? Id { get; set; }
    public int URN { get; set; }
    public string? ProjectReference { get; set; }
    public long? OutgoingTrustUKPRN { get; set; }
}