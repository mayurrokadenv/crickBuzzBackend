using MatchApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchApi.Infrastructure.Persistence.Configurations;

public class SeriesTeamConfiguration : IEntityTypeConfiguration<SeriesTeam>
{
    public void Configure(EntityTypeBuilder<SeriesTeam> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SeriesName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(x => x.Series)
            .WithMany(x => x.SeriesTeams)
            .HasForeignKey(x => x.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Team)
            .WithMany()
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}