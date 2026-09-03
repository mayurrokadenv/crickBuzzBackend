using MatchApi.Domain.Entities;

namespace MatchApi.Application.Common.Interfaces;

public interface IScorecardRepository
{
    Task<Scorecard?> GetByFixtureAndInningsAsync(
        Guid fixtureId,
        int inningsNo,
        CancellationToken cancellationToken);

    Task AddAsync(
        Scorecard scorecard,
        CancellationToken cancellationToken);

    Task AddBattingFigureAsync(
      BattingFigure figure,
      CancellationToken cancellationToken);

    Task AddBowlingFigureAsync(
        BowlingFigure figure,
        CancellationToken cancellationToken);
    Task<List<Scorecard>> GetByFixtureAsync(
    Guid fixtureId,
    CancellationToken cancellationToken);
}