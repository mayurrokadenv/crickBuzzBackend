using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Fixtures.Common;
using MatchApi.Domain.Common;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.UpdateFixtureScore;

public class UpdateFixtureScoreCommandHandler : IRequestHandler<UpdateFixtureScoreCommand, FixtureDto>
{
    private readonly IFixtureRepository _fixtureRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IScoreBroadcaster _scoreBroadcaster;

    public UpdateFixtureScoreCommandHandler(
        IFixtureRepository fixtureRepository,
        IUnitOfWork unitOfWork , IScoreBroadcaster scoreBroadcaster)
    {
        _fixtureRepository = fixtureRepository;
        _unitOfWork = unitOfWork;
        _scoreBroadcaster = scoreBroadcaster;
    }

    public async Task<FixtureDto> Handle(UpdateFixtureScoreCommand request, CancellationToken cancellationToken)
    {
        var fixture = await _fixtureRepository.GetByIdAsync(request.FixtureId, cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        
        fixture.UpdateScore(request.Side, request.RunsDelta, request.WicketsDelta,request.Overs);

        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Create latest score snapshot
        var scoreUpdate = new ScoreUpdateDto(
    fixture.Id,
    fixture.HomeScore.Runs,
    fixture.HomeScore.Wickets ?? 0,
    fixture.HomeScore.Overs,
    fixture.AwayScore.Runs,
    fixture.AwayScore.Wickets ?? 0,
    fixture.AwayScore.Overs);

        // 4. Broadcast latest score to connected users
        await _scoreBroadcaster.BroadcastAsync(scoreUpdate, cancellationToken);

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
      fixture.SportId);
    }
}
