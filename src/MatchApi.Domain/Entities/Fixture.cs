using MatchApi.Domain.Common;
using MatchApi.Domain.Enums;

namespace MatchApi.Domain.Entities;

public class Fixture : BaseEntity
{
    public Guid HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = null!;

    public Guid AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;

    public Guid SportId { get; set; }
    public Sport Sport { get; set; } = null!;

    public DateTime ScheduledAtUtc { get; set; }

    public MatchStatus Status { get; set; } = MatchStatus.Scheduled;

    public MatchPhase? Phase { get; set; }

    public Score HomeScore { get; set; } = null!;
    public Score AwayScore { get; set; } = null!;

    public string TotalOvers { get; set; } = null!;

    public ICollection<CommentaryEntry> CommentaryEntries { get; set; } = new List<CommentaryEntry>();

    public static Fixture Create(Team homeTeam, Team awayTeam, DateTime scheduledAtUtc, string totalOvers)
    {
        if (homeTeam.Id == awayTeam.Id)
            throw new InvalidOperationException("A team cannot play against itself.");

        if (homeTeam.SportId != awayTeam.SportId)
            throw new InvalidOperationException("Both teams must play the same sport.");

        var tracksWickets = homeTeam.Sport?.Name == "Cricket";

        return new Fixture
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            SportId = homeTeam.SportId,
            ScheduledAtUtc = scheduledAtUtc,
            Status = MatchStatus.Scheduled,
            HomeScore = Score.Zero(tracksWickets),
            AwayScore = Score.Zero(tracksWickets),
            TotalOvers = totalOvers,
        };
    }

    public CommentaryEntry AddCommentary(FixtureSide side, Guid playerId, CommentaryAction action, string? note)
    {
        if (Sport.Name != Enums.Sport.Cricket.ToString() && Sport.Name != Enums.Sport.Football.ToString())
        {
            throw new InvalidOperationException("Ball-by-ball commentary is only supported for cricket fixtures.");
        }

        ScoreFor(side).Apply(action.ToRuns(), action.ToWicketDelta());

        var entry = CommentaryEntry.Create(Id, side, playerId, action, note);
        CommentaryEntries.Add(entry);
        return entry;
    }

    // Backs the Score Control panel: admin nudges RUNS/WKTS directly (no player attached),
    // and it still leaves a trail in the commentary feed per the "also logs a commentary entry" UX.
    public CommentaryEntry AdjustScore(FixtureSide side, int runsDelta, int wicketsDelta, string? note = null)
    {
        if (wicketsDelta != 0 && Sport.Name != Enums.Sport.Cricket.ToString())
        {
            throw new InvalidOperationException("Wickets can only be adjusted for cricket fixtures.");
        }

        ScoreFor(side).Apply(runsDelta, wicketsDelta);

        var action = wicketsDelta != 0
            ? CommentaryAction.Wicket
            : runsDelta switch
            {
                6 => CommentaryAction.Six,
                4 => CommentaryAction.Four,
                _ => CommentaryAction.Single
            };

        var entry = CommentaryEntry.Create(Id, side, playerId: null, action, note ?? "Score updated manually");
        CommentaryEntries.Add(entry);
        return entry;
    }

    // Adds to a team's existing score/wickets (e.g. from an admin edit form) without touching
    // the commentary feed at all - unlike AdjustScore, which always logs a CommentaryEntry.
    public void UpdateScore(FixtureSide side, int runsDelta, int? wicketsDelta ,string? overs)
    {
        if (wicketsDelta is not null && wicketsDelta != 0 && Sport.Name != Enums.Sport.Cricket.ToString())
        {
            throw new InvalidOperationException("Wickets can only be updated for cricket fixtures.");
        }

        ScoreFor(side).Apply(runsDelta, wicketsDelta ?? 0,overs);
    }

    public void UpdateStatus(MatchStatus newStatus)
    {
        var isValidTransition = (Status, newStatus) switch
        {
            (MatchStatus.Scheduled, MatchStatus.Live) => true,
            (MatchStatus.Live, MatchStatus.Completed) => true,
            _ => false
        };

        if (!isValidTransition)
        {
            throw new InvalidOperationException($"Cannot change fixture status from {Status} to {newStatus}.");
        }

        Status = newStatus;
    }

    public void SetPhase(MatchPhase phase)
    {
        //if (Status != MatchStatus.Live)
        //{
        //    throw new InvalidOperationException("The match phase can only be updated while the fixture is live.");
        //}

        var isCricket = Sport.Name == Enums.Sport.Cricket.ToString();
        var isCricketPhase = phase is MatchPhase.FirstInnings or MatchPhase.SecondInnings;

        if (isCricket && !isCricketPhase)
        {
            throw new InvalidOperationException("Only First Innings or Second Innings are valid phases for a cricket fixture.");
        }

        if (!isCricket && isCricketPhase)
        {
            throw new InvalidOperationException("Innings phases are only valid for cricket fixtures.");
        }

        Phase = phase;
    }

    private Score ScoreFor(FixtureSide side) => side == FixtureSide.Home ? HomeScore : AwayScore;
}
