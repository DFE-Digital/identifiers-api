namespace Dfe.Identifiers.Domain.Authentication;

public class ApiUser
{
    public required string UserName { get; init; }
    public required string ApiKey { get; init; }
}