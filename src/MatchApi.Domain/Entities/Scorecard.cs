using System;
using System.Collections.Generic;
using System.Text;

namespace MatchApi.Domain.Entities
{
    public class Scorecard
    {
        public Guid Id { get; private set; }

        public Guid FixtureId { get; private set; }

        public int InningsNo { get; private set; }

        public Guid BattingTeamId { get; private set; }

        public Guid BowlingTeamId { get; private set; }

        // Navigation
        public Fixture Fixture { get; private set; }

        public ICollection<BattingFigure> BattingFigures { get; private set; }
            = new List<BattingFigure>();

        public ICollection<BowlingFigure> BowlingFigures { get; private set; }
            = new List<BowlingFigure>();

        public static Scorecard Create(
        Guid fixtureId,
        int inningsNo,
        Guid battingTeamId,
        Guid bowlingTeamId)
        {
            return new Scorecard
            {
                Id = Guid.NewGuid(),
                FixtureId = fixtureId,
                InningsNo = inningsNo,
                BattingTeamId = battingTeamId,
                BowlingTeamId = bowlingTeamId
            };
        }
    }
}
