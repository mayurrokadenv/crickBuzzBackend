using MatchApi.Application.Features.Commentary.Common;

namespace MatchApi.Application.Features.Fixtures.Common;

public record FixtureDetailsDto(
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
    IReadOnlyList<CommentaryDto> Commentary,
    IReadOnlyList<TopPerformerDto> TopPerformers);
