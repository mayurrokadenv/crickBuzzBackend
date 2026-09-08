using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.Entities;
using MatchApi.Infrastructure.Persistence;

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
}