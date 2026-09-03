using MatchApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchApi.Infrastructure.Persistence.Configurations;

public class BattingFigureConfiguration : IEntityTypeConfiguration<BattingFigure>
{
    public void Configure(EntityTypeBuilder<BattingFigure> builder)
    {
        builder.ToTable("BattingFigures");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.ScorecardId)
            .IsRequired();

        builder.Property(b => b.PlayerId)
            .IsRequired();

        builder.Property(b => b.Runs)
            .IsRequired();

        builder.Property(b => b.Balls)
            .IsRequired();

        builder.Property(b => b.Fours)
            .IsRequired();

        builder.Property(b => b.Sixes)
            .IsRequired();

        builder.Property(b => b.StrikeRate)
            .HasPrecision(6, 2)
            .IsRequired();

        builder.HasOne(b => b.Scorecard)
            .WithMany(s => s.BattingFigures)
            .HasForeignKey(b => b.ScorecardId)
            .OnDelete(DeleteBehavior.Cascade);

        // Player relationship
        builder.HasOne(b => b.Player)
            .WithMany()
            .HasForeignKey(b => b.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        // One batting figure per player per scorecard
        builder.HasIndex(b => new { b.ScorecardId, b.PlayerId })
            .IsUnique();
    }
}