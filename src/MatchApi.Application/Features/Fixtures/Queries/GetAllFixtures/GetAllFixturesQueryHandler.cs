using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Fixtures.Common;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Queries.GetLiveFixtures;

public class GetAllFixturesQueryHandler
    : IRequestHandler<GetAllFixturesQuery, IReadOnlyList<FixtureDto>>
{
    private readonly IFixtureRepository _fixtureRepository;

    public GetAllFixturesQueryHandler(
        IFixtureRepository fixtureRepository)
    {
        _fixtureRepository = fixtureRepository;
    }

    public async Task<IReadOnlyList<FixtureDto>> Handle(
        GetAllFixturesQuery request,
        CancellationToken cancellationToken)
    {
        var fixtures =
            await _fixtureRepository.GetAllAsync(
                cancellationToken);

        return fixtures
            .Select(f => new FixtureDto(
                f.Id,
                f.HomeTeamId,
                f.HomeTeam?.Name ?? string.Empty,
                f.AwayTeamId,
                f.AwayTeam?.Name ?? string.Empty,
                f.Sport?.Name ?? string.Empty,
                f.ScheduledAtUtc,
                f.Status.ToString(),
                f.Phase?.ToString(),

                f.HomeScore.Runs,
                f.HomeScore.Wickets,
                f.HomeScore.Overs,

                f.AwayScore.Runs,
                f.AwayScore.Wickets,
                f.AwayScore.Overs,

                f.TotalOvers,
                f.SportId,

                f.Scorecards
                    .Select(s => new FixtureScorecardDto(
                        s.Id,
                        s.FixtureId,
                        s.InningsNo,
                        s.BattingTeamId,
                        s.BowlingTeamId,

                        s.BattingFigures
                            .Select(b => new BattingFigureDto(
                                b.Id,
                                b.PlayerId,
                                b.Player.Name,
                                b.Runs,
                                b.Balls,
                                b.Fours,
                                b.Sixes,
                                b.StrikeRate))
                            .ToList(),

                        s.BowlingFigures
                            .Select(b => new BowlingFigureDto(
                                b.Id,
                                b.PlayerId,
                                b.Player.Name,
                                b.Overs,
                                b.Maidens,
                                b.Runs,
                                b.Wickets,
                                b.NoBalls,
                                b.Wides,
                                b.Economy))
                            .ToList()
                    ))
                    .ToList()
            ))
            .ToList();
    }
}