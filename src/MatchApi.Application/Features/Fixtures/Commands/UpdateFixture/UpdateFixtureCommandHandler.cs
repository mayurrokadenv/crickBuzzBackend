using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Fixtures.Common;
using MatchApi.Domain.DTOs;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.UpdateFixture;

public class UpdateFixtureCommandHandler
    : IRequestHandler<UpdateFixtureCommand, FixtureDto>
{
    private readonly IFixtureRepository _fixtureRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IScorecardRepository _scorecardRepository;
    private readonly IFixtureBroadcaster _fixtureBroadcaster;
    private readonly ISportRepository _sportRepository;

    public UpdateFixtureCommandHandler(
        IFixtureRepository fixtureRepository,
        IUnitOfWork unitOfWork,
        IScorecardRepository scorecardRepository, IFixtureBroadcaster fixtureBroadcaster, ISportRepository sportRepository)
    {
        _fixtureRepository = fixtureRepository;
        _unitOfWork = unitOfWork;
        _scorecardRepository = scorecardRepository;
        _fixtureBroadcaster = fixtureBroadcaster;
        _sportRepository = sportRepository;
    }

    public async Task<FixtureDto> Handle(
        UpdateFixtureCommand request,
        CancellationToken cancellationToken)
    {
        var fixture = await _fixtureRepository.GetByIdAsync(
            request.FixtureId,
            cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        // Get sport using Fixture.SportId
        var sport = await _sportRepository.GetByIdAsync(
            fixture.SportId,
            cancellationToken);

        if (request.Status is not null)
        {
            // Batting team is required only for Cricket
            if (sport?.Name.Equals(
                    "Cricket",
                    StringComparison.OrdinalIgnoreCase) == true
                && request.BattingTeamId is null)
            {
                throw new InvalidOperationException(
                    "Please select a batting team.");
            }

            fixture.UpdateStatus(request.Status.Value,request.BattingTeamId,request.WinningTeamId);
        }

        if (request.Phase is not null)
        {
            fixture.SetPhase(request.Phase.Value);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var fixtureUpdatedDto = new FixtureUpdatedSignalRDto(
            fixture.Id,
            fixture.HomeTeamId,
            fixture.HomeTeam?.Name ?? string.Empty,
            fixture.AwayTeamId,
            fixture.AwayTeam?.Name ?? string.Empty,
            fixture.SportId,
            fixture.SeriesId,
            fixture.ScheduledAtUtc,
            fixture.Status.ToString(),
            fixture.Phase?.ToString(),
            fixture.BattingTeamId,
            fixture.WinningTeamId,
            fixture.HomeScore.Runs,
            fixture.HomeScore.Wickets ?? 0,
            fixture.HomeScore.Overs,
            fixture.AwayScore.Runs,
            fixture.AwayScore.Wickets ?? 0,
            fixture.AwayScore.Overs
        );

        await _fixtureBroadcaster.BroadcastFixtureUpdatedAsync(
            fixtureUpdatedDto,
            cancellationToken);

        var scorecards =
            await _scorecardRepository.GetByFixtureAsync(
                fixture.Id,
                cancellationToken);

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
                        b.Player?.Name ?? string.Empty,
                        b.Runs,
                        b.Balls,
                        b.Fours,
                        b.Sixes,
                        b.StrikeRate,
                        b.Out))
                    .ToList(),

                s.BowlingFigures
                    .Select(b => new BowlingFigureDto(
                        b.Id,
                        b.PlayerId,
                        b.Player?.Name ?? string.Empty,
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

        var inningsScorecards = new InningsScorecardsDto(
            scorecardDtos.FirstOrDefault(s => s.InningsNo == 1),
            scorecardDtos.FirstOrDefault(s => s.InningsNo == 2)
        );

        return new FixtureDto(
            fixture.Id,
            fixture.HomeTeamId,
            fixture.HomeTeam?.Name ?? string.Empty,
            fixture.AwayTeamId,
            fixture.AwayTeam?.Name ?? string.Empty,
            sport?.Name ?? string.Empty,
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
            fixture.SportId,
            fixture.BattingTeamId,
            fixture.SeriesId,
            fixture.WinningTeamId,
            inningsScorecards
        );
    }
}