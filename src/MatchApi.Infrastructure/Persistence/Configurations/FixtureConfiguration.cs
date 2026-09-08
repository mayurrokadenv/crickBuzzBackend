using MatchApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchApi.Infrastructure.Persistence.Configurations;

public class FixtureConfiguration : IEntityTypeConfiguration<Fixture>
{
    public void Configure(EntityTypeBuilder<Fixture> builder)
    {
        builder.ToTable("Fixtures");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.SportId)
            .IsRequired();

        builder.HasOne(f => f.Sport)
            .WithMany()
            .HasForeignKey(f => f.SportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(f => f.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(f => f.Phase)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(f => f.ScheduledAtUtc)
            .IsRequired();

        builder.HasOne(f => f.Series)
    .WithMany(f => f.Fixtures)
    .HasForeignKey(f => f.SeriesId)
    .OnDelete(DeleteBehavior.SetNull);

        builder.OwnsOne(f => f.HomeScore, score =>
        {
            score.Property(s => s.Runs).HasColumnName("HomeScore");
            score.Property(s => s.Wickets).HasColumnName("HomeWickets");
        });

        builder.OwnsOne(f => f.AwayScore, score =>
        {
            score.Property(s => s.Runs).HasColumnName("AwayScore");
            score.Property(s => s.Wickets).HasColumnName("AwayWickets");
        });

        builder.HasOne(f => f.HomeTeam)
            .WithMany()
            .HasForeignKey(f => f.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.AwayTeam)
            .WithMany()
            .HasForeignKey(f => f.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.CommentaryEntries)
            .WithOne(c => c.Fixture)
            .HasForeignKey(c => c.FixtureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
