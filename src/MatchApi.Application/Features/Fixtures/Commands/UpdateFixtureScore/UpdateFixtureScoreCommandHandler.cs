using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Fixtures.Common;
using MatchApi.Domain.Common;
using MatchApi.Domain.DTOs.Cricket;
using MatchApi.Domain.Entities;
using MatchApi.Domain.Enums;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.UpdateFixtureScore;

public class UpdateFixtureScoreCommandHandler
    : IRequestHandler<UpdateFixtureScoreCommand, FixtureDto>
{
    private readonly IFixtureRepository _fixtureRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IScoreBroadcaster _scoreBroadcaster;
    private readonly IScorecardRepository _scorecardRepository;

    public UpdateFixtureScoreCommandHandler(
        IFixtureRepository fixtureRepository,
        IUnitOfWork unitOfWork,
        IScoreBroadcaster scoreBroadcaster,
        IScorecardRepository scorecardRepository)
    {
        _fixtureRepository = fixtureRepository;
        _unitOfWork = unitOfWork;
        _scoreBroadcaster = scoreBroadcaster;
        _scorecardRepository = scorecardRepository;
    }

    public async Task<FixtureDto> Handle(
        UpdateFixtureScoreCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Get Fixture
        var fixture = await _fixtureRepository.GetByIdAsync(
            request.FixtureId,
            cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        // 2. Update Fixture Score
        fixture.UpdateScore(
            request.Side,
            request.RunsDelta,
            request.WicketsDelta,
            request.Overs);

        // 3. Determine Batting and Bowling Teams
        var battingTeamId = request.Side == FixtureSide.Home
            ? fixture.HomeTeamId
            : fixture.AwayTeamId;

        var bowlingTeamId = request.Side == FixtureSide.Home
            ? fixture.AwayTeamId
            : fixture.HomeTeamId;

        // 4. Determine Innings
        var inningsNo = fixture.Phase switch
        {
            MatchPhase.FirstInnings => 1,

            MatchPhase.SecondInnings => 2,

            _ => throw new InvalidOperationException(
                "Invalid innings phase for cricket score update.")
        };

        // 5. Get Existing Scorecard
        var scorecard =
            await _scorecardRepository.GetByFixtureAndInningsAsync(
                fixture.Id,
                inningsNo,
                cancellationToken);

        // 6. Create Scorecard if it doesn't exist
        if (scorecard is null)
        {
            scorecard = Scorecard.Create(
                fixture.Id,
                inningsNo,
                battingTeamId,
                bowlingTeamId);

            await _scorecardRepository.AddAsync(
                scorecard,
                cancellationToken);
        }

        // 7. Get Batting Figure
        var battingFigure = scorecard.BattingFigures
            .FirstOrDefault(x =>
                x.PlayerId == request.BattingPlayerId);

        // 8. Create Batting Figure if player doesn't exist
        if (battingFigure is null)
        {
            battingFigure = BattingFigure.Create(
                scorecard.Id,
                request.BattingPlayerId);

            await _scorecardRepository.AddBattingFigureAsync(
                battingFigure,
                cancellationToken);
        }

        // 9. Update Batting Figure
        battingFigure.Update(
            request.RunsDelta);

        // 10. Get Bowling Figure
        var bowlingFigure = scorecard.BowlingFigures
            .FirstOrDefault(x =>
                x.PlayerId == request.BowlingPlayerId);

        // 11. Create Bowling Figure if player doesn't exist
        if (bowlingFigure is null)
        {
            bowlingFigure = BowlingFigure.Create(
                scorecard.Id,
                request.BowlingPlayerId);

            await _scorecardRepository.AddBowlingFigureAsync(
                bowlingFigure,
                cancellationToken);
        }

        // 12. Update Bowling Figure
        bowlingFigure.Update(
            request.RunsDelta,
            request.Overs);

        // 13. Save everything
        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // =========================================================
        // 14. GET ALL SCORECARDS FOR THIS FIXTURE
        // =========================================================

        var scorecards =
            await _scorecardRepository.GetByFixtureAsync(
                fixture.Id,
                cancellationToken);

        // =========================================================
        // 15. MAP SCORECARDS -> SCORECARD DTOs
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
        // 16. BROADCAST LATEST SCORE
        // =========================================================

        var scoreUpdate = new ScoreUpdateDto(
            fixture.Id,

            fixture.HomeScore.Runs,
            fixture.HomeScore.Wickets ?? 0,
            fixture.HomeScore.Overs,

            fixture.AwayScore.Runs,
            fixture.AwayScore.Wickets ?? 0,
            fixture.AwayScore.Overs);

        await _scoreBroadcaster.BroadcastAsync(
            scoreUpdate,
            cancellationToken);

        // =========================================================
        // 17. RETURN COMPLETE FIXTURE RESPONSE
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

            // IMPORTANT:
            // scorecardDtos already List<ScorecardDto>
            scorecardDtos
        );
    }
}