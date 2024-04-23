using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Infrastructure.Interfaces;

public interface IProjectsRepository
{
    Task<List<ConversionProject>> GetConversionProjectsByIdentifier(string identifier,
        CancellationToken cancellationToken);

    Task<List<TransferProject>> GetTransfersProjectsByIdentifier(string identifier,
        CancellationToken cancellationToken);

    Task<List<FormAMatProject>> GetFormAMatProjectsByIdentifier(string identifier,
        CancellationToken cancellationToken);
}