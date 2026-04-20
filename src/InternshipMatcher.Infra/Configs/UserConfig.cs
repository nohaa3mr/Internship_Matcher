using InternshipMatcher.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace InternshipMatcher.Infra.Configs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.ID);
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasOne<StudentProfile>().WithOne(sp => sp.User)
            .HasForeignKey<StudentProfile>(sp => sp.UserID)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<RecruiterProfile>().WithOne(rp => rp.User).HasForeignKey<RecruiterProfile>(x=>x.UserID);

    }
}
