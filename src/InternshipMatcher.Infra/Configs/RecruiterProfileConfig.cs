using InternshipMatcher.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Infra.Configs;

public class RecruiterProfileConfig : IEntityTypeConfiguration<RecruiterProfile>
{
    public void Configure(EntityTypeBuilder<RecruiterProfile> builder)
    {
        builder.HasKey(r => r.ID);
        builder.Property(r => r.FullName)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasOne(r => r.User).WithOne()
            .HasForeignKey<RecruiterProfile>(r => r.UserID)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.Position).IsRequired();
        builder.Property(r => r.CompanyName)
            .IsRequired()
            .HasMaxLength(100);
    }
}
