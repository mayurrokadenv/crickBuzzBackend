using MatchApi.Application.Features.Fixtures.Common;
using MatchApi.Domain.Enums;
using MediatR;

public record UpdateFixtureCommand(Guid FixtureId,MatchStatus? Status,MatchPhase? Phase,Guid? BattingTeamId) : IRequest<FixtureDto>;