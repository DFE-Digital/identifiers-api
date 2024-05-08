using Dfe.Identifiers.Domain.Models;
using Dfe.Identifiers.Infrastructure.Context;
using Dfe.Identifiers.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Identifiers.Infrastructure.Repositories;

public class ProjectsRepository(AcademisationContext context) : IProjectsRepository
{
    public async Task<List<ConversionProject>> GetConversionProjectsByIdentifier(string identifier,
        CancellationToken cancellationToken)
    {
        var query = await context.ConversionProjects.Where(project =>
            project.ApplicationReferenceNumber == identifier ||
            project.TrustReferenceNumber == identifier ||
            project.SponsorReferenceNumber == identifier
        ).ToListAsync(cancellationToken);
        return query;
    }

    public async Task<List<TransferProject>> GetTransfersProjectsByIdentifier(string identifier,
        CancellationToken cancellationToken)
    {
        var query = await context.TransferProjects.Where(project =>
            project.ProjectReference == identifier
        ).ToListAsync(cancellationToken);
        return query;
    }

    public async Task<List<FormAMatProject>> GetFormAMatProjectsByIdentifier(string identifier,
        CancellationToken cancellationToken)
    {
        var query = await context.FormAMatProjects.Where(project =>
            project.ApplicationReference == identifier ||
            project.ReferenceNumber == identifier
        ).ToListAsync(cancellationToken);
        return query;
    }
}