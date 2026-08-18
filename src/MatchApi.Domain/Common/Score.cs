namespace MatchApi.Domain.Common;

public class Score
{
    public int Runs { get; private set; }
    public int? Wickets { get; private set; }
    public string Overs { get; private set; } = "0.0";

    private Score()
    {
    }

    public static Score Zero(bool tracksWickets) => new()
    {
        Runs = 0,
        Wickets = tracksWickets ? 0 : null ,
        Overs = "0.0"
    };

    public void Apply(int runsDelta, int wicketsDelta, string? overs = null)
    {
        Runs += runsDelta;

        if (wicketsDelta != 0)
        {
            Wickets = (Wickets ?? 0) + wicketsDelta;
        }
        if (overs is not null)
        {
            Overs = overs;
        }
    }
}
