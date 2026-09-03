using MatchApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchApi.Infrastructure.Persistence.Configurations;

public class ScorecardConfiguration : IEntityTypeConfiguration<Scorecard>
{
    public void Configure(EntityTypeBuilder<Scorecard> builder)
    {
        builder.ToTable("Scorecards");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FixtureId)
            .IsRequired();

        builder.Property(s => s.InningsNo)
            .IsRequired();

        builder.Property(s => s.BattingTeamId)
            .IsRequired();

        builder.Property(s => s.BowlingTeamId)
            .IsRequired();

        builder.HasOne(s => s.Fixture)
            .WithMany(f => f.Scorecards)
            .HasForeignKey(s => s.FixtureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Team>()
            .WithMany()
            .HasForeignKey(s => s.BattingTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Team>()
            .WithMany()
            .HasForeignKey(s => s.BowlingTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // One scorecard per innings of a fixture
        builder.HasIndex(s => new { s.FixtureId, s.InningsNo })
            .IsUnique();
    }
}