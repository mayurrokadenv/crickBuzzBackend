using MatchApi.Api.Hubs;
using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.DTOs;
using MatchApi.Domain.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace MatchApi.Api.Realtime;

public class SignalRFixtureBroadcaster : IFixtureBroadcaster
{
    private readonly IHubContext<CommentaryHub> _hubContext;

    public SignalRFixtureBroadcaster(
        IHubContext<CommentaryHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task BroadcastFixtureCreatedAsync(
        FixtureCreatedSignalRDto fixture,
        CancellationToken cancellationToken)
    {
        return _hubContext
            .Clients
            .Group(CommentaryHub.GroupName(fixture.FixtureId))
            .SendAsync(
                "FixtureCreated",
                fixture,
                cancellationToken);
    }

    public Task BroadcastFixtureUpdatedAsync(
        FixtureUpdatedSignalRDto fixture,
        CancellationToken cancellationToken)
    {
        return _hubContext
            .Clients
            .Group(CommentaryHub.GroupName(fixture.FixtureId))
            .SendAsync(
                "FixtureUpdated",
                fixture,
                cancellationToken);
    }
}