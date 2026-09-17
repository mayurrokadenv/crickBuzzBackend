namespace MatchApi.Domain.DTOs;

public class SeriesPointsTableDto
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;

    public int Played { get; set; }
    public int Won { get; set; }
    public int Lost { get; set; }

    public int Points { get; set; }
}