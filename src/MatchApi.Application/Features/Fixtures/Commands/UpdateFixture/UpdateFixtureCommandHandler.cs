using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Fixtures.Common;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.UpdateFixture;

public class UpdateFixtureCommandHandler
    : IRequestHandler<UpdateFixtureCommand, FixtureDto>
{
    private readonly IFixtureRepository _fixtureRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IScorecardRepository _scorecardRepository;

    public UpdateFixtureCommandHandler(
        IFixtureRepository fixtureRepository,
        IUnitOfWork unitOfWork,
        IScorecardRepository scorecardRepository)
    {
        _fixtureRepository = fixtureRepository;
        _unitOfWork = unitOfWork;
        _scorecardRepository = scorecardRepository;
    }

    public async Task<FixtureDto> Handle(
        UpdateFixtureCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Get Fixture
        var fixture = await _fixtureRepository.GetByIdAsync(
            request.FixtureId,
            cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        // 2. Update Status
        if (request.Status is not null)
        {
            fixture.UpdateStatus(request.Status.Value);
        }

        // 3. Update Phase
        if (request.Phase is not null)
        {
            fixture.SetPhase(request.Phase.Value);
        }

        // 4. Save Fixture changes
        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // =========================================================
        // 5. GET ALL SCORECARDS FOR THIS FIXTURE
        // =========================================================

        var scorecards =
            await _scorecardRepository.GetByFixtureAsync(
                fixture.Id,
                cancellationToken);

        // =========================================================
        // 6. MAP SCORECARDS -> DTOs
        // =========================================================

        var scorecardDtos = scorecards
            .Select(s => new FixtureScorecardDto(
                s.Id,
                s.FixtureId,
                s.InningsNo,
                s.BattingTeamId,
                s.BowlingTeamId,

                // Batting Figures
                s.BattingFigures
                    .Select(b => new BattingFigureDto(
                        b.Id,
                        b.PlayerId,
                        b.Player?.Name ?? string.Empty,
                        b.Runs,
                        b.Balls,
                        b.Fours,
                        b.Sixes,
                        b.StrikeRate))
                    .ToList(),

                // Bowling Figures
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

        // =========================================================
        // 7. RETURN COMPLETE FIXTURE RESPONSE
        // =========================================================

        return new FixtureDto(
            fixture.Id,

            fixture.HomeTeamId,
            fixture.HomeTeam?.Name ?? string.Empty,

            fixture.AwayTeamId,
            fixture.AwayTeam?.Name ?? string.Empty,

            fixture.Sport?.Name ?? string.Empty,

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

            scorecardDtos
        );
    }
}