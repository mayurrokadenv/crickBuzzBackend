using MatchApi.Domain.DTOs;
using MatchApi.Domain.DTOs;

namespace MatchApi.Application.Common.Interfaces;

public interface IFixtureBroadcaster
{
    Task BroadcastFixtureCreatedAsync(
        FixtureCreatedSignalRDto dto,
        CancellationToken cancellationToken);

    Task BroadcastFixtureUpdatedAsync(
        FixtureUpdatedSignalRDto dto,
        CancellationToken cancellationToken);
}