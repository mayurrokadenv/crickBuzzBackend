namespace MatchApi.Application.Features.Fixtures.Common;

public record FixtureDto(
    Guid Id,
    Guid HomeTeamId,
    string HomeTeamName,
    Guid AwayTeamId,
    string AwayTeamName,
    string Sport,
    DateTime ScheduledAtUtc,
    string Status,
    string? Phase,
    int HomeScore,
    int? HomeWickets,
    string? HomeOvers,
    int AwayScore,
    int? AwayWickets,
    string? AwayOvers,
    string TotalOvers,
    Guid SportId);
