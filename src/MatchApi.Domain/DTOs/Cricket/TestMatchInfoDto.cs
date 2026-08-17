namespace MatchApi.Domain.DTOs.Cricket;

public class TestMatchInfoDto
{
    public Dictionary<string, CricbuzzMatchCommentaryDto> MatchCommentary { get; set; } = new();

    public CricbuzzMiniScoreDto? Miniscore { get; set; }

    public bool EnableNoContent { get; set; }

    public CricbuzzMatchHeaderDto? MatchHeader { get; set; }

    public List<object> MatchVideos { get; set; } = [];

    public string? Page { get; set; }

    public long ResponseLastUpdated { get; set; }
    public string? Source { get; set; }
}


// ============================================================
// COMMENTARY
// ============================================================

public class CricbuzzMatchCommentaryDto
{
    public long MatchId { get; set; }

    public string? CommType { get; set; }

    public string? CommText { get; set; }

    public int InningsId { get; set; }

    public List<string> Event { get; set; } = [];

    // Kuch commentary items me ballMetric absent hai
    public double? BallMetric { get; set; }

    public string? TeamName { get; set; }

    public long Timestamp { get; set; }

    public CricbuzzOverSeparatorDto? OverSeparator { get; set; }

    public CricbuzzCommentaryPlayerDto? BatsmanDetails { get; set; }

    public CricbuzzCommentaryPlayerDto? BowlerDetails { get; set; }
}

public class CricbuzzOverSeparatorDto
{
    public long Timestamp { get; set; }

    public int OverNumber { get; set; }

    public string? OverSummary { get; set; }

    public int OverRuns { get; set; }

    public CricbuzzOverTeamDto? BatTeamObj { get; set; }

    public CricbuzzOverPlayerDto? BatStrikerObj { get; set; }

    public CricbuzzOverPlayerDto? BatNonStrikerObj { get; set; }

    public CricbuzzOverPlayerDto? BowlerObj { get; set; }
}

public class CricbuzzOverTeamDto
{
    public string? TeamName { get; set; }

    public string? TeamScore { get; set; }
}

public class CricbuzzOverPlayerDto
{
    public long PlayerId { get; set; }

    public string? PlayerName { get; set; }

    public string? PlayerScore { get; set; }
}

public class CricbuzzCommentaryPlayerDto
{
    public long PlayerId { get; set; }

    public string? PlayerName { get; set; }
}


// ============================================================
// MINISCORE
// ============================================================

public class CricbuzzMiniScoreDto
{
    public int InningsId { get; set; }

    public CricbuzzBatTeamDto? BatTeam { get; set; }

    public string? Status { get; set; }

    public CricbuzzBatsmanDto? BatsmanStriker { get; set; }

    public CricbuzzBatsmanDto? BatsmanNonStriker { get; set; }

    public CricbuzzBowlerDto? BowlerStriker { get; set; }

    public CricbuzzBowlerDto? BowlerNonStriker { get; set; }

    public double Overs { get; set; }

    public int? Target { get; set; }

    public CricbuzzPartnershipDto? PartnerShip { get; set; }

    public double CurrentRunRate { get; set; }

    public double RequiredRunRate { get; set; }

    public double RunsPerBall { get; set; }

    public double RequiredRunsPerBall { get; set; }

    public CricbuzzMatchScoreDetailsDto? MatchScoreDetails { get; set; }

    public string? LastWicket { get; set; }

    public int RemRunsToWin { get; set; }

    public int OversRem { get; set; }

    public long ResponseLastUpdated { get; set; }

    public List<CricbuzzLatestPerformanceDto> LatestPerformance { get; set; } = [];

    public string? RecentOvsStats { get; set; }

    public string? Event { get; set; }

    public CricbuzzTeamScoreObjectDto? BatTeamScoreObj { get; set; }

    public CricbuzzTeamScoreObjectDto? BowlTeamScoreObj { get; set; }

    public CricbuzzMatchUdrsDto? MatchUdrs { get; set; }
}

public class CricbuzzBatTeamDto
{
    public long TeamId { get; set; }

    public int TeamScore { get; set; }

    public int TeamWkts { get; set; }
}

public class CricbuzzBatsmanDto
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public int Runs { get; set; }

    public int Balls { get; set; }

    public int Fours { get; set; }

    public int Sixes { get; set; }

    public string? StrikeRate { get; set; }

    public string? PlayerUrl { get; set; }

    public string? PlayerMatchHighlightsUrl { get; set; }
}

public class CricbuzzBowlerDto
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public double Overs { get; set; }

    public int Maidens { get; set; }

    public double Economy { get; set; }

    public int Runs { get; set; }

    public int Wickets { get; set; }

    public string? PlayerUrl { get; set; }

    public string? PlayerMatchHighlightsUrl { get; set; }
}

public class CricbuzzPartnershipDto
{
    public int Balls { get; set; }

    public int Runs { get; set; }
}

public class CricbuzzLatestPerformanceDto
{
    public int Runs { get; set; }

    public int Wkts { get; set; }

    public string? Label { get; set; }
}


// ============================================================
// MATCH SCORE DETAILS
// ============================================================

public class CricbuzzMatchScoreDetailsDto
{
    public long MatchId { get; set; }

    public List<CricbuzzInningsScoreDto> InningsScoreList { get; set; } = [];

    public bool IsMatchNotCovered { get; set; }

    public string? MatchFormat { get; set; }

    public string? CustomStatus { get; set; }

    public string? State { get; set; }
}

public class CricbuzzInningsScoreDto
{
    public int InningsId { get; set; }

    public long BatTeamId { get; set; }

    public string? BatTeamName { get; set; }

    public int Score { get; set; }

    public int Wickets { get; set; }

    public double Overs { get; set; }

    public bool IsDeclared { get; set; }

    public bool IsFollowOn { get; set; }

    public int BallNbr { get; set; }
}

public class CricbuzzTeamScoreObjectDto
{
    public string? TeamName { get; set; }

    public List<CricbuzzInningsScoreDto> TeamInningsArray { get; set; } = [];
}


// ============================================================
// UDRS
// ============================================================

public class CricbuzzMatchUdrsDto
{
    public long MatchId { get; set; }

    public int InningsId { get; set; }

    public string? Timestamp { get; set; }

    public long Team1Id { get; set; }

    public int Team1Remaining { get; set; }

    public int Team1Successful { get; set; }

    public int Team1Unsuccessful { get; set; }

    public long Team2Id { get; set; }

    public int Team2Remaining { get; set; }

    public int Team2Successful { get; set; }

    public int Team2Unsuccessful { get; set; }
}


// ============================================================
// MATCH HEADER
// ============================================================

public class CricbuzzMatchHeaderDto
{
    public long MatchId { get; set; }

    public string? MatchDescription { get; set; }

    public string? MatchFormat { get; set; }

    public string? MatchType { get; set; }

    public bool Complete { get; set; }

    public bool Domestic { get; set; }

    public long MatchStartTimestamp { get; set; }

    public string? MatchStartTimeIST { get; set; }

    public string? MatchStartTimeGMT { get; set; }

    public string? MatchStartTimeLocal { get; set; }

    public string? MatchCompleteTimeIST { get; set; }

    public string? MatchCompleteTimeGMT { get; set; }

    public string? MatchCompleteTimeLocal { get; set; }

    public bool DayNight { get; set; }

    public int Year { get; set; }

    public string? State { get; set; }

    public string? Status { get; set; }

    public CricbuzzTossResultDto? TossResults { get; set; }

    public CricbuzzMatchResultDto? Result { get; set; }

    public CricbuzzRevisedTargetDto? RevisedTarget { get; set; }

    public List<CricbuzzPlayerDto> PlayersOfTheMatch { get; set; } = [];

    public List<CricbuzzPlayerDto> PlayersOfTheSeries { get; set; } = [];

    public List<CricbuzzMatchTeamInfoDto> MatchTeamInfo { get; set; } = [];

    public CricbuzzMatchTeamDto? Team1 { get; set; }

    public CricbuzzMatchTeamDto? Team2 { get; set; }

    public string? SeriesDesc { get; set; }

    public long SeriesId { get; set; }

    public string? SeriesName { get; set; }

    public string? AlertType { get; set; }

    public bool IsMatchNotCovered { get; set; }

    public bool LivestreamEnabled { get; set; }
}

public class CricbuzzTossResultDto
{
    public long TossWinnerId { get; set; }

    public string? TossWinnerName { get; set; }

    public string? Decision { get; set; }
}


// Empty object {} live response me aa raha hai.
// Completed match me properties populate ho sakti hain.
public class CricbuzzMatchResultDto
{
    public string? ResultType { get; set; }

    public string? WinningTeam { get; set; }

    public long? WinningTeamId { get; set; }

    public int? WinningMargin { get; set; }

    public bool? WinByRuns { get; set; }

    public bool? WinByInnings { get; set; }
}


// Live response me revisedTarget: {} hai.
public class CricbuzzRevisedTargetDto
{
    public int? Reason { get; set; }

    public int? RevisedTarget { get; set; }

    public double? RevisedOvers { get; set; }
}

public class CricbuzzMatchTeamInfoDto
{
    public long BattingTeamId { get; set; }

    public string? BattingTeamShortName { get; set; }

    public long BowlingTeamId { get; set; }

    public string? BowlingTeamShortName { get; set; }
}

public class CricbuzzMatchTeamDto
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public List<CricbuzzPlayerDto> PlayerDetails { get; set; } = [];

    public string? ShortName { get; set; }
}

public class CricbuzzPlayerDto
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? FullName { get; set; }

    public string? NickName { get; set; }

    public bool Captain { get; set; }

    public bool Keeper { get; set; }

    public bool Substitute { get; set; }

    public long FaceImageId { get; set; }
}