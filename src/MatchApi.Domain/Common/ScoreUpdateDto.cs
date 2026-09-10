

namespace MatchApi.Domain.Common;

public record ScoreUpdateDto(
    Guid FixtureId,
    int HomeRuns,
    int HomeWickets,
    string? HomeOvers,
    int AwayRuns,
    int AwayWickets,
    string? AwayOvers,
    IReadOnlyList<ScorecardUpdateDto> Scorecards);