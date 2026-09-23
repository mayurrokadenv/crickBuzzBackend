public record ScorecardUpdateDto(
    Guid Id,
    Guid FixtureId,
    int InningsNo,
    Guid BattingTeamId,
    Guid BowlingTeamId,
    Guid? WinningTeamId,
    IReadOnlyList<BattingFigureUpdateDto> BattingFigures,
    IReadOnlyList<BowlingFigureUpdateDto> BowlingFigures);

public record BattingFigureUpdateDto(
    Guid Id,
    Guid PlayerId,
    string PlayerName,
    int Runs,
    int Balls,
    int Fours,
    int Sixes,
    decimal StrikeRate,
    bool Out);

public record BowlingFigureUpdateDto(
    Guid Id,
    Guid PlayerId,
    string PlayerName,
    string Overs,
    int Maidens,
    int Runs,
    int Wickets,
    int NoBalls,
    int Wides,
    decimal Economy);