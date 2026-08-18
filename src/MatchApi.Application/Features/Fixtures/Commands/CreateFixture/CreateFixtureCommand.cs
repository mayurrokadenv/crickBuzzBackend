using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.CreateFixture;

public record CreateFixtureCommand(
    Guid HomeTeamId,
    Guid AwayTeamId,
    string TotalOvers,
    DateTime ScheduledAtUtc) : IRequest<CreateFixtureResponse>;
