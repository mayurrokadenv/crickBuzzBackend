using MatchApi.Domain.Common;
using MatchApi.Domain.Enums;

namespace MatchApi.Domain.Entities;

public class CommentaryEntry : BaseEntity
{
    public Guid FixtureId { get; set; }

    public Fixture Fixture { get; set; } = null!;

    public FixtureSide Side { get; set; }

    public Guid? PlayerId { get; set; }

    public Player? Player { get; set; }

    public CommentaryAction Action { get; set; }

    public string? Note { get; set; }
    public string? Ball { get; set; }

    public void UpdateNote(string? note)
    {
        Note = note;
    }

    public static CommentaryEntry Create(
        Guid fixtureId,
        FixtureSide side,
        Guid? playerId,
        CommentaryAction action,
        string? note, string? ball)
    {
        if (fixtureId == Guid.Empty)
            throw new InvalidOperationException("Fixture is required.");

        return new CommentaryEntry
        {
            FixtureId = fixtureId,
            Side = side,
            PlayerId = playerId,
            Action = action,
            Note = note,
            Ball= ball
        };
    }
}
