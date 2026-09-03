using MatchApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchApi.Infrastructure.Persistence.Configurations;

public class BowlingFigureConfiguration : IEntityTypeConfiguration<BowlingFigure>
{
    public void Configure(EntityTypeBuilder<BowlingFigure> builder)
    {
        builder.ToTable("BowlingFigures");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.ScorecardId)
            .IsRequired();

        builder.Property(b => b.PlayerId)
            .IsRequired();

        builder.Property(b => b.Overs)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(b => b.Maidens)
            .IsRequired();

        builder.Property(b => b.Runs)
            .IsRequired();

        builder.Property(b => b.Wickets)
            .IsRequired();

        builder.Property(b => b.NoBalls)
            .IsRequired();

        builder.Property(b => b.Wides)
            .IsRequired();

        builder.Property(b => b.Economy)
            .HasPrecision(6, 2)
            .IsRequired();

        builder.HasOne(b => b.Scorecard)
            .WithMany(s => s.BowlingFigures)
            .HasForeignKey(b => b.ScorecardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Player)
       .WithMany()
       .HasForeignKey(b => b.PlayerId)
       .OnDelete(DeleteBehavior.Restrict);

        // One bowling figure per player per scorecard
        builder.HasIndex(b => new { b.ScorecardId, b.PlayerId })
            .IsUnique();
    }
}