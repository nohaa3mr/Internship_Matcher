using InternshipMatcher.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternshipMatcher.Infra.Configs;

public class StudentSkillConfig : IEntityTypeConfiguration<StudentSkill>
{
    public void Configure(EntityTypeBuilder<StudentSkill> builder)
    {
       builder.HasKey(x => new { x.StudentProfileID, x.SkillID });
        builder.HasOne(x => x.StudentProfile)
            .WithMany(sp => sp.StudentSkills)
            .HasForeignKey(x => x.StudentProfileID)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Skill)
            .WithMany()
            .HasForeignKey(x => x.SkillID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
