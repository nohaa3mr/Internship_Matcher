using InternshipMatcher.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace InternshipMatcher.Infra.Configs;

public class InternshipConfig : IEntityTypeConfiguration<Internship>
{
    public void Configure(EntityTypeBuilder<Internship> builder)
    {
        builder.HasKey(i => i.ID);
        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.HasMany(x => x.Applications)
            .WithOne(a => a.Internship);
    }
}
