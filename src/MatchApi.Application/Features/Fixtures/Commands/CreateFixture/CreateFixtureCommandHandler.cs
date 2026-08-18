using MatchApi.Application.Common.Interfaces;
using MediatR;
using DomainFixture = MatchApi.Domain.Entities.Fixture;

namespace MatchApi.Application.Features.Fixtures.Commands.CreateFixture;

public class CreateFixtureCommandHandler : IRequestHandler<CreateFixtureCommand, CreateFixtureResponse>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IFixtureRepository _fixtureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFixtureCommandHandler(
        ITeamRepository teamRepository,
        IFixtureRepository fixtureRepository,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _fixtureRepository = fixtureRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateFixtureResponse> Handle(CreateFixtureCommand request, CancellationToken cancellationToken)
    {
        var homeTeam = await _teamRepository.GetByIdAsync(request.HomeTeamId, cancellationToken)
            ?? throw new InvalidOperationException("Home team not found.");

        var awayTeam = await _teamRepository.GetByIdAsync(request.AwayTeamId, cancellationToken)
            ?? throw new InvalidOperationException("Away team not found.");

        var fixture = DomainFixture.Create(homeTeam, awayTeam, request.ScheduledAtUtc,request.TotalOvers);

        await _fixtureRepository.AddAsync(fixture, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateFixtureResponse(
            fixture.Id,
            homeTeam.Id,
            homeTeam.Name,
            awayTeam.Id,
            awayTeam.Name,
            fixture.SportId,
            fixture.ScheduledAtUtc,
            fixture.Status.ToString());
    }
}
