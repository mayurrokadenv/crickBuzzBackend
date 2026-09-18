namespace MatchApi.Domain.Common;

public record ScoreUpdateDto(
    Guid FixtureId,

    // Fixture Details
    Guid SportId,
    string SportName,
    string Status,
    string? Phase,

    Guid HomeTeamId,
    string HomeTeamName,

    Guid AwayTeamId,
    string AwayTeamName,

    // Home Score
    int HomeRuns,
    int HomeWickets,
    string? HomeOvers,

    // Away Score
    int AwayRuns,
    int AwayWickets,
    string? AwayOvers,

    // Scorecards
    IReadOnlyList<ScorecardUpdateDto> Scorecards
);