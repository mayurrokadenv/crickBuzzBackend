using System;
using System.Collections.Generic;
using System.Text;
using MatchApi.Domain.Common;

namespace MatchApi.Application.Common.Interfaces
{
    public interface IScoreBroadcaster
    {
        Task BroadcastAsync(
            ScoreUpdateDto score,
            CancellationToken cancellationToken);
    }
}
