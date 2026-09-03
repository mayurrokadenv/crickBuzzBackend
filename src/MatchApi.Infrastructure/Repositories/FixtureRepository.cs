using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.Entities;
using MatchApi.Domain.Enums;
using MatchApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchApi.Infrastructure.Repositories;

public class FixtureRepository : IFixtureRepository
{
    private readonly ApplicationDbContext _context;

    public FixtureRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Fixture fixture, CancellationToken cancellationToken)
    {
        await _context.Fixtures.AddAsync(fixture, cancellationToken);
    }

    public Task<Fixture?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Fixtures
            .Include(f => f.Sport).Include(f => f.HomeTeam)
            .Include(f => f.AwayTeam)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Fixture>> GetLiveAsync(
     CancellationToken cancellationToken)
    {
        return await _context.Fixtures
            .AsNoTracking()

            .Include(f => f.Sport)
            .Include(f => f.HomeTeam)
            .Include(f => f.AwayTeam)

            .Include(f => f.Scorecards)
                .ThenInclude(s => s.BattingFigures)
                    .ThenInclude(b => b.Player)

            .Include(f => f.Scorecards)
                .ThenInclude(s => s.BowlingFigures)
                    .ThenInclude(b => b.Player)

            .Where(f => f.Status == MatchStatus.Live)

            .OrderBy(f => f.ScheduledAtUtc)

            .ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<Fixture>> SearchAsync(
    string searchTerm,
    CancellationToken cancellationToken)
    {
        searchTerm = searchTerm.Trim().ToLower();

        return await _context.Fixtures
            .AsNoTracking()
            .Include(f => f.HomeTeam)
            .Include(f => f.AwayTeam)
            .Include(f => f.Sport)
            .Where(f =>
                f.HomeTeam.Name.ToLower().Contains(searchTerm) ||
                f.AwayTeam.Name.ToLower().Contains(searchTerm) ||
                f.Sport.Name.ToLower().Contains(searchTerm))
            .OrderBy(f => f.ScheduledAtUtc)
            .ToListAsync(cancellationToken);
    }
   
}
