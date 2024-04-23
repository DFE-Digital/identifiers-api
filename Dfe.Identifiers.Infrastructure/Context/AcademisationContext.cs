using Dfe.Identifiers.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfe.Identifiers.Infrastructure.Context;

public class AcademisationContext : DbContext
{
    const string DEFAULT_SCHEMA = "academisation";

    public AcademisationContext()
    {

    }

    public AcademisationContext(DbContextOptions<AcademisationContext> options) : base(options)
    {

    }

    public DbSet<ConversionProject> ConversionProjects { get; set; } = null!;
    public DbSet<TransferProject> TransferProjects { get; set; } = null!;
    public DbSet<FormAMatProject> FormAMatProjects { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=sip;Integrated Security=true;TrustServerCertificate=True");
        }
        optionsBuilder.LogTo(Console.WriteLine);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConversionProject>(ConfigureConversionProjects);
        modelBuilder.Entity<TransferProject>(ConfigureTransferProject);
        modelBuilder.Entity<FormAMatProject>(ConfigureFormAMatProject);

        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureFormAMatProject(EntityTypeBuilder<FormAMatProject> projectConfiguration)
    {
        projectConfiguration.HasKey(e => e.Id);

        projectConfiguration.ToTable("FormAMatProject", DEFAULT_SCHEMA);

        projectConfiguration.Property(e => e.ApplicationReference).HasColumnName("ApplicationReference");
        projectConfiguration.Property(e => e.ReferenceNumber).HasColumnName("ReferenceNumber");
    }

    private void ConfigureTransferProject(EntityTypeBuilder<TransferProject> projectConfiguration)
    {
        projectConfiguration.HasKey(e => e.Id);

        projectConfiguration.ToTable("TransferProject", DEFAULT_SCHEMA);

        projectConfiguration.Property(e => e.URN).HasColumnName("Urn");
        projectConfiguration.Property(e => e.ProjectReference).HasColumnName("ProjectReference");
        projectConfiguration.Property(e => e.OutgoingTrustUKPRN).HasColumnName("OutgoingTrustUkprn");
    }

    private void ConfigureConversionProjects(EntityTypeBuilder<ConversionProject> projectConfiguration)
    {
        projectConfiguration.HasKey(e => e.Id);

        projectConfiguration.ToTable("Project", DEFAULT_SCHEMA);

        projectConfiguration.Property(e => e.ApplicationReferenceNumber).HasColumnName("ApplicationReferenceNumber");
        projectConfiguration.Property(e => e.URN).HasColumnName("Urn");
        projectConfiguration.Property(e => e.SchoolName).HasColumnName("SchoolName");
        projectConfiguration.Property(e => e.FormAMatProjectId).HasColumnName("FormAMatProjectId");
        projectConfiguration.Property(e => e.TrustReferenceNumber).HasColumnName("TrustReferenceNumber");
        projectConfiguration.Property(e => e.SponsorReferenceNumber).HasColumnName("SponsorReferenceNumber");
        
        projectConfiguration
            .HasOne(x => x.FormAMatProject)
            .WithMany()
            .HasForeignKey(x => x.FormAMatProjectId)
            .IsRequired(false);

    }
}
