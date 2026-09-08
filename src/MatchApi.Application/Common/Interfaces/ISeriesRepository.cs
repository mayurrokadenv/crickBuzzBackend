using MatchApi.Domain.Entities;

namespace MatchApi.Application.Common.Interfaces;

public interface ISeriesRepository
{
    Task AddAsync(
        Series series,
        CancellationToken cancellationToken);
}