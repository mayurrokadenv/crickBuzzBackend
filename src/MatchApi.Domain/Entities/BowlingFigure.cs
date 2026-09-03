using System;
using System.Collections.Generic;
using System.Text;

namespace MatchApi.Domain.Entities
{
    public class BowlingFigure
    {
        public Guid Id { get; private set; }

        public Guid ScorecardId { get; private set; }

        public Guid PlayerId { get; private set; }

        public string Overs { get; private set; } = "0";

        public int Maidens { get; private set; }

        public int Runs { get; private set; }

        public int Wickets { get; private set; }

        public int NoBalls { get; private set; }

        public int Wides { get; private set; }

        public decimal Economy { get; private set; }

        // Navigation
        public Scorecard Scorecard { get; private set; } = null!;
        public Player Player { get; private set; } = null!;

        public static BowlingFigure Create(
       Guid scorecardId,
       Guid playerId)
        {
            return new BowlingFigure
            {
                Id = Guid.NewGuid(),
                ScorecardId = scorecardId,
                PlayerId = playerId,
                Overs = "0"
            };
        }

        public void Update(int runs, string overs)
        {
            Runs += runs;
            Overs = overs;

            var oversParts = overs.Split('.');

            if (oversParts.Length == 2 &&
                int.TryParse(oversParts[0], out var completedOvers) &&
                int.TryParse(oversParts[1], out var balls))
            {
                var totalBalls = completedOvers * 6 + balls;

                Economy = totalBalls == 0
                    ? 0
                    : Math.Round((decimal)Runs / (totalBalls / 6m), 2);
            }
        }
    }
}
