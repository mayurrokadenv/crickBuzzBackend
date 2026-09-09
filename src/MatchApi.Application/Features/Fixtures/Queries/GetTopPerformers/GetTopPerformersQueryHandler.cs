using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Fixtures.Common;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Queries.GetTopPerformers;

public class GetTopPerformersQueryHandler
    : IRequestHandler<GetTopPerformersQuery, IReadOnlyList<TopPerformerDto>>
{
    private const int TopPerformerCount = 4;

    private readonly IFixtureRepository _fixtureRepository;
    private readonly IScorecardRepository _scorecardRepository;

    public GetTopPerformersQueryHandler(
        IFixtureRepository fixtureRepository,
        IScorecardRepository scorecardRepository)
    {
        _fixtureRepository = fixtureRepository;
        _scorecardRepository = scorecardRepository;
    }

    public async Task<IReadOnlyList<TopPerformerDto>> Handle(
        GetTopPerformersQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Validate Fixture
        _ = await _fixtureRepository.GetByIdAsync(
            request.FixtureId,
            cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        // 2. Get both scorecards for this fixture
        var scorecards = await _scorecardRepository.GetByFixtureAsync(
            request.FixtureId,
            cancellationToken);

        // 3. Get batting figures from all innings/scorecards
        return scorecards
            .SelectMany(s => s.BattingFigures)
            .Where(b => b.Player is not null)
            .Select(b => new TopPerformerDto(
                b.PlayerId,
                b.Player!.Name,
                b.Player.TeamId,
                b.Player.Team?.Name ?? string.Empty,
                b.Runs
            ))
            .OrderByDescending(x => x.RunsScored)
            .Take(TopPerformerCount)
            .ToList();
    }
}