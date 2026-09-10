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
    private readonly IScorecardRepository _scorecardRepository;

    public GetFixtureDetailsQueryHandler(IFixtureRepository fixtureRepository, ICommentaryRepository commentaryRepository,IScorecardRepository scorecardRepository)
    {
        _fixtureRepository = fixtureRepository;
        _commentaryRepository = commentaryRepository;
        _scorecardRepository = scorecardRepository;
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


        var scorecards = await _scorecardRepository.GetByFixtureAsync(fixture.Id,cancellationToken);

        var topPerformers = scorecards
    .SelectMany(s => s.BattingFigures)
    .Where(b => b.Player != null)
    .Select(b => new TopPerformerDto(
        b.Player.Id,
        b.Player.Name,
        b.Player.TeamId,
        b.Player.Team?.Name ?? string.Empty,
        b.Runs)).OrderByDescending(p => p.RunsScored)
    .Take(TopPerformerCount)
    .ToList();

        var scorecardDtos = scorecards
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
            fixture.BattingTeamId,
            commentary,
            topPerformers,
            scorecardDtos);
    }
}
