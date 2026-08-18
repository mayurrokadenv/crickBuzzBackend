using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Fixtures.Common;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.UpdateFixture;

public class UpdateFixtureCommandHandler : IRequestHandler<UpdateFixtureCommand, FixtureDto>
{
    private readonly IFixtureRepository _fixtureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFixtureCommandHandler(IFixtureRepository fixtureRepository, IUnitOfWork unitOfWork)
    {
        _fixtureRepository = fixtureRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FixtureDto> Handle(UpdateFixtureCommand request, CancellationToken cancellationToken)
    {
        var fixture = await _fixtureRepository.GetByIdAsync(request.FixtureId, cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        // Status is applied before Phase so both can be set in a single call
        // (e.g. Scheduled -> Live together with setting the opening phase).
        if (request.Status is not null)
        {
            fixture.UpdateStatus(request.Status.Value);
        }

        if (request.Phase is not null)
        {
            fixture.SetPhase(request.Phase.Value);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
