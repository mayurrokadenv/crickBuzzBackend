namespace MatchApi.Domain.DTOs.Cricket;

public class CricbuzzScorecardResponseDto
{
    public List<CricbuzzScorecardInningsDto> ScoreCard { get; set; } = [];

    // Existing Match Info DTO wala MatchHeader reuse hoga
    public CricbuzzMatchHeaderDto? MatchHeader { get; set; }

    public string? Status { get; set; }

    public List<object> Videos { get; set; } = [];

    public long ResponseLastUpdated { get; set; }

    public bool IsMatchComplete { get; set; }
    public string? Source { get; set; }
}

public class CricbuzzScorecardInningsDto
{
    public long MatchId { get; set; }

    public int InningsId { get; set; }

    public long TimeScore { get; set; }

    public CricbuzzBatTeamDetailsDto? BatTeamDetails { get; set; }

    public CricbuzzBowlTeamDetailsDto? BowlTeamDetails { get; set; }

    public CricbuzzScoreDetailsDto? ScoreDetails { get; set; }

    public CricbuzzExtrasDataDto? ExtrasData { get; set; }

    public Dictionary<string, CricbuzzPowerPlayDto> PpData { get; set; } = [];

    public Dictionary<string, CricbuzzWicketDataDto> WicketsData { get; set; } = [];

    public Dictionary<string, CricbuzzPartnershipDataDto> PartnershipsData { get; set; } = [];
}


// ============================================================
// BATTING TEAM
// ============================================================

public class CricbuzzBatTeamDetailsDto
{
    public long BatTeamId { get; set; }

    public string? BatTeamName { get; set; }

    public string? BatTeamShortName { get; set; }

    public Dictionary<string, CricbuzzScorecardBatsmanDto> BatsmenData { get; set; } = [];
}

public class CricbuzzScorecardBatsmanDto
{
    public long BatId { get; set; }

    public string? BatName { get; set; }

    public string? BatShortName { get; set; }

    public bool IsCaptain { get; set; }

    public bool IsKeeper { get; set; }

    public int Runs { get; set; }

    public int Balls { get; set; }

    public int Dots { get; set; }

    public int Fours { get; set; }

    public int Sixes { get; set; }

    public int Mins { get; set; }

    public double StrikeRate { get; set; }

    public string? OutDesc { get; set; }

    public long BowlerId { get; set; }

    public long FielderId1 { get; set; }

    public long FielderId2 { get; set; }

    public long FielderId3 { get; set; }

    public int Ones { get; set; }

    public int Twos { get; set; }

    public int Threes { get; set; }

    public int Fives { get; set; }

    public int Boundaries { get; set; }

    public int Sixers { get; set; }

    public string? WicketCode { get; set; }

    public bool IsOverseas { get; set; }

    public string? InMatchChange { get; set; }

    public string? PlayingXIChange { get; set; }
}


// ============================================================
// BOWLING TEAM
// ============================================================

public class CricbuzzBowlTeamDetailsDto
{
    public long BowlTeamId { get; set; }

    public string? BowlTeamName { get; set; }

    public string? BowlTeamShortName { get; set; }

    public Dictionary<string, CricbuzzScorecardBowlerDto> BowlersData { get; set; } = [];
}

public class CricbuzzScorecardBowlerDto
{
    public long BowlerId { get; set; }

    public string? BowlName { get; set; }

    public string? BowlShortName { get; set; }

    public bool IsCaptain { get; set; }

    public bool IsKeeper { get; set; }

    public double Overs { get; set; }

    public int Maidens { get; set; }

    public int Runs { get; set; }

    public int Wickets { get; set; }

    public double Economy { get; set; }

    public int No_Balls { get; set; }

    public int Wides { get; set; }

    public int Dots { get; set; }

    public int Balls { get; set; }

    public double RunsPerBall { get; set; }

    public bool IsOverseas { get; set; }

    public string? InMatchChange { get; set; }

    public string? PlayingXIChange { get; set; }
}


// ============================================================
// SCORE DETAILS
// ============================================================

public class CricbuzzScoreDetailsDto
{
    public int BallNbr { get; set; }

    public double Overs { get; set; }

    public double RevisedOvers { get; set; }

    public double RunRate { get; set; }

    public int Runs { get; set; }

    public int Wickets { get; set; }

    public double RunsPerBall { get; set; }

    public bool IsDeclared { get; set; }

    public bool IsFollowOn { get; set; }
}


// ============================================================
// EXTRAS
// ============================================================

public class CricbuzzExtrasDataDto
{
    public int Byes { get; set; }

    public int LegByes { get; set; }

    public int NoBalls { get; set; }

    public int Penalty { get; set; }

    public int Total { get; set; }

    public int Wides { get; set; }
}


// ============================================================
// POWERPLAY
// ============================================================

public class CricbuzzPowerPlayDto
{
    public int PpId { get; set; }

    public double PpOversFrom { get; set; }

    public double PpOversTo { get; set; }

    public string? PpType { get; set; }

    public int RunsScored { get; set; }
}


// ============================================================
// WICKETS
// ============================================================

public class CricbuzzWicketDataDto
{
    public long BatId { get; set; }

    public string? BatName { get; set; }

    public int WktNbr { get; set; }

    public double WktOver { get; set; }

    public int WktRuns { get; set; }

    public int BallNbr { get; set; }
}


// ============================================================
// PARTNERSHIPS
// ============================================================

public class CricbuzzPartnershipDataDto
{
    public long Bat1Id { get; set; }

    public string? Bat1Name { get; set; }

    public int Bat1Runs { get; set; }

    public long Bat2Id { get; set; }

    public string? Bat2Name { get; set; }

    public int Bat2Runs { get; set; }

    public int TotalRuns { get; set; }

    public int TotalBalls { get; set; }

    public int Bat1Ones { get; set; }

    public int Bat1Twos { get; set; }

    public int Bat1Threes { get; set; }

    public int Bat1Fives { get; set; }

    public int Bat1Boundaries { get; set; }

    public int Bat1Sixers { get; set; }

    public int Bat2Ones { get; set; }

    public int Bat2Twos { get; set; }

    public int Bat2Threes { get; set; }

    public int Bat2Fives { get; set; }

    public int Bat2Boundaries { get; set; }

    public int Bat2Sixers { get; set; }

    public int Bat1Fours { get; set; }

    public int Bat1Sixes { get; set; }

    public int Bat2Fours { get; set; }

    public int Bat2Sixes { get; set; }

    public int Bat1Balls { get; set; }

    public int Bat2Balls { get; set; }
}