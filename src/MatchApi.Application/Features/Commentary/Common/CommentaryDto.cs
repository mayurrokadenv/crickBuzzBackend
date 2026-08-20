namespace MatchApi.Application.Features.Commentary.Common;

public record CommentaryDto(
    Guid Id,
    Guid FixtureId,
    string Side,
    Guid PlayerId,
    string PlayerName,
    string Action,
    string? Note,
    string? Ball,
    DateTime CreatedAtUtc,
    int HomeScore,
    int? HomeWickets,
    int AwayScore,
    int? AwayWickets,
    string FixtureName,    
    string SportName);
