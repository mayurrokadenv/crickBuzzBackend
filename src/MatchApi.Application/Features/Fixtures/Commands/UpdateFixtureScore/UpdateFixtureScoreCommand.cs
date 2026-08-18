using MatchApi.Application.Features.Fixtures.Common;
using MatchApi.Domain.Enums;
using MediatR;

namespace MatchApi.Application.Features.Fixtures.Commands.UpdateFixtureScore;

public record UpdateFixtureScoreCommand(
    Guid FixtureId,
    FixtureSide Side,
    int RunsDelta,
    string Overs,
    int? WicketsDelta) : IRequest<FixtureDto>;
