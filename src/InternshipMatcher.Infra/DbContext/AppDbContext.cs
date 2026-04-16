namespace InternshipMatcher.Infra.DbContext;

using InternshipMatcher.Domain.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext, IDataProtectionKeyContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<RecruiterProfile> RecruiterProfiles => Set<RecruiterProfile>();
    public DbSet<StudentSkill> StudentSkills => Set<StudentSkill>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Internship> Internships => Set<Internship>();
    public DbSet<InternshipSkill> InternshipSkills => Set<InternshipSkill>();

    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
}
