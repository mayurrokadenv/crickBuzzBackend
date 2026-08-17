

public class CricbuzzMatchListDto
{
    public List<CricbuzzMatchItemDto> Matches { get; set; } = [];
    public long ResponseLastUpdated { get; set; }
    public string? Source { get; set; }
}

public class CricbuzzMatchItemDto
{
    public CricbuzzMatchDto Match { get; set; } = new();
}

public class CricbuzzMatchDto
{
    public CricbuzzMatchInfoDto MatchInfo { get; set; } = new();
    public CricbuzzMatchScoreDto? MatchScore { get; set; }
}

public class CricbuzzMatchInfoDto
{
    public long MatchId { get; set; }
    public long SeriesId { get; set; }
    public string? SeriesName { get; set; }
    public string? MatchDesc { get; set; }
    public string? MatchFormat { get; set; }
    public long StartDate { get; set; }
    public long EndDate { get; set; }
    public string? State { get; set; }
    public string? Status { get; set; }

    public CricbuzzTeamDto Team1 { get; set; } = new();
    public CricbuzzTeamDto Team2 { get; set; } = new();
    public CricbuzzVenueDto VenueInfo { get; set; } = new();

    public long? CurrBatTeamId { get; set; }
    public string? StateTitle { get; set; }
    public string? MatchType { get; set; }
    public string? ShortStatus { get; set; }
}

public class CricbuzzTeamDto
{
    public long TeamId { get; set; }
    public string? TeamName { get; set; }
    public string? TeamSName { get; set; }
    public long ImageId { get; set; }
}

public class CricbuzzVenueDto
{
    public long Id { get; set; }
    public string? Ground { get; set; }
    public string? City { get; set; }
    public string? Timezone { get; set; }
}

public class CricbuzzMatchScoreDto
{
    public CricbuzzTeamScoreDto? Team1Score { get; set; }
    public CricbuzzTeamScoreDto? Team2Score { get; set; }
}

public class CricbuzzTeamScoreDto
{
    public CricbuzzInningsScoreDto? Inngs1 { get; set; }
    public CricbuzzInningsScoreDto? Inngs2 { get; set; }
}

public class CricbuzzInningsScoreDto
{
    public int InningsId { get; set; }
    public int Runs { get; set; }
    public int? Wickets { get; set; }
    public double Overs { get; set; }
    public bool? IsDeclared { get; set; }
}