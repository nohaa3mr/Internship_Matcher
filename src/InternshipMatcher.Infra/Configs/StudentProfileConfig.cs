using InternshipMatcher.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipMatcher.Infra.Configs;

public class StudentProfileConfig :IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder.HasKey(sp => sp.ID);
        builder.Property(sp => sp.FullName)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasOne(sp => sp.User)
            .WithOne()
            .HasForeignKey<StudentProfile>(sp => sp.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sp => sp.StudentSkills)
            .WithOne(ss => ss.StudentProfile)
            .HasForeignKey(ss => ss.StudentProfileID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}