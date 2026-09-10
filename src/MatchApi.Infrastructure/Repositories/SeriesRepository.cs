using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.Entities;
using MatchApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchApi.Infrastructure.Repositories;

public class SeriesRepository : ISeriesRepository
{
    private readonly ApplicationDbContext _context;

    public SeriesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Series series,
        CancellationToken cancellationToken)
    {
        await _context.Series.AddAsync(
            series,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Series>> GetAllAsync(
    CancellationToken cancellationToken)
    {
        return await _context.Series
            .AsNoTracking()
            .Include(x => x.Sport)
            .Include(x => x.SeriesTeams)
                .ThenInclude(x => x.Team)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}