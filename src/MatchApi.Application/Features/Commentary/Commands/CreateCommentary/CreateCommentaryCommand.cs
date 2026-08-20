using MatchApi.Application.Features.Commentary.Common;
using MatchApi.Domain.Enums;
using MediatR;

namespace MatchApi.Application.Features.Commentary.Commands.CreateCommentary;

public record CreateCommentaryCommand(
    Guid FixtureId,
    FixtureSide Side,
    Guid PlayerId,
    CommentaryAction Action,
    string? CurrentBall,
    string? Note) : IRequest<CommentaryDto>;
