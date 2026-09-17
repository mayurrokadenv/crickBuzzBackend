namespace MatchApi.Domain.DTOs;

public record FixtureUpdatedSignalRDto(
    Guid FixtureId,
    Guid HomeTeamId,
    string HomeTeamName,
    Guid AwayTeamId,
    string AwayTeamName,
    Guid SportId,
    Guid? SeriesId,
    DateTime ScheduledAtUtc,
    string Status,
    string? Phase,
    Guid? BattingTeamId,
    Guid? WinningTeamId,
    int HomeRuns,
    int HomeWickets,
    string? HomeOvers,
    int AwayRuns,
    int AwayWickets,
    string? AwayOvers
);