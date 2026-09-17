namespace MatchApi.Domain.DTOs;

public record FixtureCreatedSignalRDto(
    Guid FixtureId,
    Guid HomeTeamId,
    string HomeTeamName,
    Guid AwayTeamId,
    string AwayTeamName,
    Guid SportId,
    Guid? SeriesId,
    DateTime ScheduledAtUtc,
    string Status
);