using MatchApi.Api.Hubs;
using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.Common;
using Microsoft.AspNetCore.SignalR;

namespace MatchApi.Api.Realtime
{
    public class SignalRScoreBroadcaster : IScoreBroadcaster
    {
        private readonly IHubContext<CommentaryHub> _hubContext;

        public SignalRScoreBroadcaster(
            IHubContext<CommentaryHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task BroadcastAsync(
            ScoreUpdateDto score,
            CancellationToken cancellationToken)
        {
            return _hubContext
                .Clients
                .Group(CommentaryHub.GroupName(score.FixtureId))
                .SendAsync(
                    "ScoreUpdated",
                    score,
                    cancellationToken);
        }
    }
}
