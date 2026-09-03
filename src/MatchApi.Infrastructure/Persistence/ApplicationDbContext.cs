using System.Reflection;
using MatchApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatchApi.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Fixture> Fixtures => Set<Fixture>();
    public DbSet<Sport> Sports => Set<Sport>();
    public DbSet<SportRole> SportRoles => Set<SportRole>();
    public DbSet<CommentaryEntry> CommentaryEntries => Set<CommentaryEntry>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<Scorecard> Scorecards => Set<Scorecard>();
    public DbSet<BattingFigure> BattingFigures => Set<BattingFigure>();
    public DbSet<BowlingFigure> BowlingFigures => Set<BowlingFigure>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
