namespace InternshipMatcher.Infra.DbContext;

using InternshipMatcher.Domain.Enums;
using InternshipMatcher.Domain.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class AppDbContext : DbContext, IDataProtectionKeyContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<User>()
      .Property(x => x.Role)
      .HasConversion(new EnumToStringConverter<UserRole>());

        modelBuilder.Entity<ApplicationForm>()
            .Property(x => x.ApplicationStatus)
            .HasConversion(new EnumToStringConverter<ApplicationStatus>());

    }
    public DbSet<User> Users => Set<User>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<RecruiterProfile> RecruiterProfiles => Set<RecruiterProfile>();
    public DbSet<StudentSkill> StudentSkills => Set<StudentSkill>();
    public DbSet<ApplicationForm> Applications => Set<ApplicationForm>();
    public DbSet<Internship> Internships => Set<Internship>();
    public DbSet<InternshipSkill> InternshipSkills => Set<InternshipSkill>();
    public DbSet<StudentApplication> StudentApplications => Set<StudentApplication>();
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
}
