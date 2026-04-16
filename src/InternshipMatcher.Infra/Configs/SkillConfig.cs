using InternshipMatcher.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipMatcher.Infra.Configs;

public class SkillConfig : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasKey(s => s.ID);
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(x=>x.Name).IsUnique();
    }
}
