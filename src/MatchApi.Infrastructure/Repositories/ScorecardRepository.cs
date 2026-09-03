using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.Entities;
using MatchApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchApi.Infrastructure.Repositories;

public class ScorecardRepository : IScorecardRepository
{
    private readonly ApplicationDbContext _context;

    public ScorecardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Scorecard?> GetByFixtureAndInningsAsync(
        Guid fixtureId,
        int inningsNo,
        CancellationToken cancellationToken)
    {
        return await _context.Scorecards
    .Include(x => x.BattingFigures)
        .ThenInclude(x => x.Player)
    .Include(x => x.BowlingFigures)
        .ThenInclude(x => x.Player)
    .FirstOrDefaultAsync(
        x => x.FixtureId == fixtureId &&
             x.InningsNo == inningsNo,
        cancellationToken);
    }

    public async Task AddAsync(
        Scorecard scorecard,
        CancellationToken cancellationToken)
    {
        await _context.Scorecards.AddAsync(
            scorecard,
            cancellationToken);
    }

    public async Task AddBattingFigureAsync(
    BattingFigure figure,
    CancellationToken cancellationToken)
    {
        await _context.BattingFigures.AddAsync(
            figure,
            cancellationToken);
    }

    public async Task AddBowlingFigureAsync(
        BowlingFigure figure,
        CancellationToken cancellationToken)
    {
        await _context.BowlingFigures.AddAsync(
            figure,
            cancellationToken);
    }

    public async Task<List<Scorecard>> GetByFixtureAsync(
    Guid fixtureId,
    CancellationToken cancellationToken)
    {
        return await _context.Scorecards
    .Include(x => x.BattingFigures)
        .ThenInclude(x => x.Player)
    .Include(x => x.BowlingFigures)
        .ThenInclude(x => x.Player)
    .Where(x => x.FixtureId == fixtureId)
    .OrderBy(x => x.InningsNo)
    .ToListAsync(cancellationToken);
    }
}