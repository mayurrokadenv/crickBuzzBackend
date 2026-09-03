namespace MatchApi.Application.Features.Fixtures.Common;

public record FixtureScorecardDto(
    Guid Id,
    Guid FixtureId,
    int InningsNo,
    Guid BattingTeamId,
    Guid BowlingTeamId,
    List<BattingFigureDto> BattingFigures,
    List<BowlingFigureDto> BowlingFigures
);

public record BattingFigureDto(
    Guid Id,
    Guid PlayerId,
    string PlayerName,
    int Runs,
    int Balls,
    int Fours,
    int Sixes,
    decimal StrikeRate
);

public record BowlingFigureDto(
    Guid Id,
    Guid PlayerId,
    string PlayerName,
    string Overs,
    int Maidens,
    int Runs,
    int Wickets,
    int NoBalls,
    int Wides,
    decimal Economy
);