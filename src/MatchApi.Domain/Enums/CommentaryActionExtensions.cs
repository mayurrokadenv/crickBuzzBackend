namespace MatchApi.Domain.Enums;

public static class CommentaryActionExtensions
{
    public static int ToRuns(this CommentaryAction action) => action switch
    {
        CommentaryAction.Six => 6,
        CommentaryAction.Four => 4,
        CommentaryAction.Single => 1,
        CommentaryAction.Wide => 1,
        CommentaryAction.Wicket => 0,
        CommentaryAction.Three => 3,
        CommentaryAction.DotBall => 0,
        _ => 0
    };

    public static int ToWicketDelta(this CommentaryAction action) =>
        action == CommentaryAction.Wicket ? 1 : 0;
}
