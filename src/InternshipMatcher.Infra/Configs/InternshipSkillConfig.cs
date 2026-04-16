using InternshipMatcher.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipMatcher.Infra.Configs;

public class InternshipSkillConfig : IEntityTypeConfiguration<InternshipSkill>
{
    public void Configure(EntityTypeBuilder<InternshipSkill> builder)
    {
        builder.HasKey(x => new { x.InternshipID, x.SkillID });
        builder.HasOne(x => x.Internship)
            .WithMany(i => i.InternshipSkills)
            .HasForeignKey(x => x.InternshipID)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Skill)
            .WithMany()
            .HasForeignKey(x => x.SkillID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
