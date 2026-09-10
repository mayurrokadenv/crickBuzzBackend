using MatchApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatchApi.Domain.Entities
{
    public class BattingFigure
    {
        public Guid Id { get; private set; }

        public Guid ScorecardId { get; private set; }

        public Guid PlayerId { get; private set; }

        public int Runs { get; private set; }

        public int Balls { get; private set; }

        public int Fours { get; private set; }

        public int Sixes { get; private set; }

        public decimal StrikeRate { get; private set; }

        // Navigation
        public Scorecard Scorecard { get; private set; } = null!;
        public Player Player { get; private set; } = null!;
        public bool Out { get; private set; }

        public static BattingFigure Create(
      Guid scorecardId,
      Guid playerId)
        {
            return new BattingFigure
            {
                Id = Guid.NewGuid(),
                ScorecardId = scorecardId,
                PlayerId = playerId
            };
        }

        public void Update(int runs, CommentaryAction action)
        {
            Runs += runs;
            Balls++;

            if (action == CommentaryAction.Wicket)
            {
                Out = true;
            }

            if (action == CommentaryAction.Four)
            {
                Fours++;
            }

            if (action == CommentaryAction.Six)
            {
                Sixes++;
            }

            StrikeRate = Balls == 0
                ? 0
                : Math.Round((decimal)Runs / Balls * 100, 2);
        }
    }
}
