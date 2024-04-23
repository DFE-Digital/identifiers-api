using Dfe.Identifiers.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfe.Identifiers.Infrastructure.Context;

public class MstrContext : DbContext
{
    const string DEFAULT_SCHEMA = "mstr";

    public MstrContext()
    {

    }

    public MstrContext(DbContextOptions<MstrContext> options) : base(options)
    {

    }

    public DbSet<Trust> Trusts { get; set; } = null!;
    public DbSet<TrustType> TrustTypes { get; set; } = null!;
    public DbSet<Establishment> Establishments { get; set; } = null!;
    public DbSet<EstablishmentType> EstablishmentTypes { get; set; } = null!;
    public DbSet<EducationEstablishmentTrust> EducationEstablishmentTrusts { get; set; } = null!;
    public DbSet<LocalAuthority> LocalAuthorities { get; set; } = null!;
    
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
        modelBuilder.Entity<Trust>(ConfigureTrust);
        modelBuilder.Entity<TrustType>(ConfigureTrustType);

        modelBuilder.Entity<Establishment>(ConfigureEstablishment);
        modelBuilder.Entity<EstablishmentType>(ConfigureEstablishmentType);
        modelBuilder.Entity<EducationEstablishmentTrust>(ConfigureEducationEstablishmentTrust);
        modelBuilder.Entity<LocalAuthority>(ConfigureLocalAuthority);

        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureEstablishment(EntityTypeBuilder<Establishment> establishmentConfiguration)
    {
        establishmentConfiguration.HasKey(e => e.SK);

        establishmentConfiguration.ToTable("EducationEstablishment", DEFAULT_SCHEMA);
        
       
        establishmentConfiguration.Property(e => e.EstablishmentName).HasColumnName("EstablishmentName");
        establishmentConfiguration.Property(e => e.EstablishmentNumber).HasColumnName("EstablishmentNumber");
        establishmentConfiguration.Property(e => e.EstablishmentGroupTypeId).HasColumnName("FK_EstablishmentGroupType");
        establishmentConfiguration.Property(e => e.EstablishmentStatusId).HasColumnName("FK_EstablishmentStatus");
        establishmentConfiguration.Property(e => e.EstablishmentTypeId).HasColumnName("FK_EstablishmentType");
        establishmentConfiguration.Property(e => e.LocalAuthorityId).HasColumnName("FK_LocalAuthority");
        establishmentConfiguration.Property(e => e.RegionId).HasColumnName("FK_Region");
        establishmentConfiguration.Property(e => e.GORregion).HasColumnName("GORregion");
        establishmentConfiguration.Property(e => e.PK_CDM_ID).HasColumnName("PK_CDM_ID");
        establishmentConfiguration.Property(e => e.PK_GIAS_URN).HasColumnName("PK_GIAS_URN").HasConversion<int?>();
        establishmentConfiguration.Property(e => e.UKPRN).HasColumnName("UKPRN");
        establishmentConfiguration.Property(e => e.URN).HasColumnName("URN");
        establishmentConfiguration.Property(e => e.URNAtCurrentFullInspection).HasColumnName("URN at Current full inspection");
        establishmentConfiguration.Property(e => e.URNAtPreviousFullInspection).HasColumnName("URN at Previous full inspection");
        establishmentConfiguration.Property(e => e.URNAtSection8Inspection).HasColumnName("URN at Section 8 inspection");
        establishmentConfiguration.Property(e => e.GORregionCode).HasColumnName("GORregion(code)");

        establishmentConfiguration
            .HasOne(x => x.EstablishmentType)
            .WithMany()
            .HasForeignKey(x => x.EstablishmentTypeId)
            .IsRequired(false);

        establishmentConfiguration
            .HasOne(x => x.LocalAuthority)
            .WithMany()
            .HasForeignKey(x => x.LocalAuthorityId)
            .IsRequired(false);
    }

    void ConfigureTrust(EntityTypeBuilder<Trust> trustConfiguration)
    {
        trustConfiguration.HasKey(e => e.SK);
        
        trustConfiguration.ToTable("Trust", DEFAULT_SCHEMA);

        trustConfiguration.Property(e => e.TrustTypeId).HasColumnName("FK_TrustType");
        trustConfiguration.Property(e => e.RegionId).HasColumnName("FK_Region");
        trustConfiguration.Property(e => e.TrustBandingId).HasColumnName("FK_TrustBanding");
        trustConfiguration.Property(e => e.TrustStatusId).HasColumnName("FK_TrustStatus");
        trustConfiguration.Property(e => e.GroupUID).HasColumnName("Group UID").IsRequired();
        trustConfiguration.Property(e => e.GroupID).HasColumnName("Group ID");
        trustConfiguration.Property(e => e.RID).HasColumnName("RID");
        trustConfiguration.Property(e => e.Name).HasColumnName("Name").IsRequired();
        trustConfiguration.Property(e => e.CompaniesHouseNumber).HasColumnName("Companies House Number");
        trustConfiguration.Property(e => e.TrustStatus).HasColumnName("Trust Status");
        trustConfiguration.Property(e => e.UKPRN).HasColumnName("UKPRN");
        trustConfiguration.Property(e => e.UPIN).HasColumnName("UPIN");

        trustConfiguration
            .HasOne(x => x.TrustType)
            .WithMany()
            .HasForeignKey(x => x.TrustTypeId);
    }

    private void ConfigureTrustType(EntityTypeBuilder<TrustType> trustTypeConfiguration)
    {
        trustTypeConfiguration.HasKey(e => e.SK);

        trustTypeConfiguration.ToTable("Ref_TrustType", DEFAULT_SCHEMA);

        trustTypeConfiguration.HasData(new TrustType() { SK = 30, Code = "06", Name = "Multi-academy trust" });
        trustTypeConfiguration.HasData(new TrustType() { SK = 32, Code = "10", Name = "Single-academy trust" });
    }
    private void ConfigureEducationEstablishmentTrust(EntityTypeBuilder<EducationEstablishmentTrust> entityBuilder)
    {
        entityBuilder.HasKey(e => e.SK);
        entityBuilder.ToTable("EducationEstablishmentTrust", DEFAULT_SCHEMA);

        entityBuilder.Property(e => e.EducationEstablishmentId).HasColumnName("FK_EducationEstablishment");
        entityBuilder.Property(e => e.TrustId).HasColumnName("FK_Trust");
    }

    private void ConfigureLocalAuthority(EntityTypeBuilder<LocalAuthority> localAuthorityConfiguration)
    {
        localAuthorityConfiguration.HasKey(e => e.SK);
        localAuthorityConfiguration.ToTable("Ref_LocalAuthority", DEFAULT_SCHEMA);

        localAuthorityConfiguration.HasData(new LocalAuthority() { SK = 1, Code = "202", Name = "Barnsley" });
        localAuthorityConfiguration.HasData(new LocalAuthority() { SK = 2, Code = "203", Name = "Birmingham" });
        localAuthorityConfiguration.HasData(new LocalAuthority() { SK = 3, Code = "204", Name = "Bradford" });
    }

    private void ConfigureEstablishmentType(EntityTypeBuilder<EstablishmentType> establishmentTypeConfiguration)
    {
        establishmentTypeConfiguration.HasKey(e => e.SK);
        establishmentTypeConfiguration.ToTable("Ref_EducationEstablishmentType", DEFAULT_SCHEMA);

        establishmentTypeConfiguration.HasData(new EstablishmentType() { SK = 224, Code = "35", Name = "Free schools" });
        establishmentTypeConfiguration.HasData(new EstablishmentType() { SK = 228, Code = "18", Name = "Further education" });
    }

}
