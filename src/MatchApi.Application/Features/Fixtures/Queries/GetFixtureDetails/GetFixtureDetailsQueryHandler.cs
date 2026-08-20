using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Commentary.Common;
using MatchApi.Application.Features.Fixtures.Common;
using MatchApi.Domain.Enums;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Queries.GetFixtureDetails;

public class GetFixtureDetailsQueryHandler : IRequestHandler<GetFixtureDetailsQuery, FixtureDetailsDto>
{
    private const int TopPerformerCount = 4;

    private readonly IFixtureRepository _fixtureRepository;
    private readonly ICommentaryRepository _commentaryRepository;

    public GetFixtureDetailsQueryHandler(IFixtureRepository fixtureRepository, ICommentaryRepository commentaryRepository)
    {
        _fixtureRepository = fixtureRepository;
        _commentaryRepository = commentaryRepository;
    }

    public async Task<FixtureDetailsDto> Handle(GetFixtureDetailsQuery request, CancellationToken cancellationToken)
    {
        var fixture = await _fixtureRepository.GetByIdAsync(request.FixtureId, cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        var entries = await _commentaryRepository.GetByFixtureIdAsync(request.FixtureId, cancellationToken);

        var fixtureName = $"{fixture.HomeTeam?.Name} v {fixture.AwayTeam?.Name}";
        var sportName = fixture.Sport?.Name ?? string.Empty;

        var commentary = entries
            .OrderByDescending(e => e.CreatedAtUtc)
            .Select(e => new CommentaryDto(
                e.Id,
                e.FixtureId,
                e.Side.ToString(),
                e.PlayerId ?? Guid.Empty,
                e.Player?.Name ?? string.Empty,
                e.Action.ToString(),
                e.Note,
                e.Ball,
                e.CreatedAtUtc,
                fixture.HomeScore.Runs,
                fixture.HomeScore.Wickets,
                fixture.AwayScore.Runs,
                fixture.AwayScore.Wickets,
                fixtureName,
                sportName))
            .ToList();

        // Grouped by the scalar PlayerId (not the Player navigation) because entries are loaded
        // AsNoTracking without identity resolution: each row materializes its own Player instance,
        // so grouping by the object itself would split one player's entries across several groups.
        var topPerformers = entries
            .Where(e => e.PlayerId is not null && e.Player is not null)
            .GroupBy(e => e.PlayerId!.Value)
            .Select(g =>
            {
                var player = g.First().Player!;
                return new TopPerformerDto(
                    player.Id,
                    player.Name,
                    player.TeamId,
                    player.Team?.Name ?? string.Empty,
                    g.Sum(e => e.Action.ToRuns()));
            })
            .OrderByDescending(p => p.RunsScored)
            .Take(TopPerformerCount)
            .ToList();

        return new FixtureDetailsDto(
            fixture.Id,
            fixture.HomeTeamId,
            fixture.HomeTeam?.Name ?? string.Empty,
            fixture.AwayTeamId,
            fixture.AwayTeam?.Name ?? string.Empty,
            sportName,
            fixture.ScheduledAtUtc,
            fixture.Status.ToString(),
            fixture.Phase?.ToString(),
            fixture.HomeScore.Runs,
            fixture.HomeScore.Wickets,
            fixture.HomeScore.Overs,
            fixture.AwayScore.Runs,
            fixture.AwayScore.Wickets,
            fixture.AwayScore.Overs,
            fixture.TotalOvers,
            commentary,
            topPerformers);
    }
}
