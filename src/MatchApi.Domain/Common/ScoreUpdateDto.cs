using System;
using System.Collections.Generic;
using System.Text;

namespace MatchApi.Domain.Common
{
    public record ScoreUpdateDto(
    Guid FixtureId,
    int HomeRuns,
    int HomeWickets,
    int AwayRuns,
    int AwayWickets);
}
