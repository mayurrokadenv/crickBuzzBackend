using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Commentary.Common;
using MediatR;

namespace MatchApi.Application.Features.Commentary.Commands.UpdateCommentary;

public class UpdateCommentaryCommandHandler : IRequestHandler<UpdateCommentaryCommand, CommentaryDto>
{
    private readonly ICommentaryRepository _commentaryRepository;
    private readonly IFixtureRepository _fixtureRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommentaryBroadcaster _broadcaster;

    public UpdateCommentaryCommandHandler(
        ICommentaryRepository commentaryRepository,
        IFixtureRepository fixtureRepository,
        IUnitOfWork unitOfWork,
        ICommentaryBroadcaster broadcaster)
    {
        _commentaryRepository = commentaryRepository;
        _fixtureRepository = fixtureRepository;
        _unitOfWork = unitOfWork;
        _broadcaster = broadcaster;
    }

    public async Task<CommentaryDto> Handle(UpdateCommentaryCommand request, CancellationToken cancellationToken)
    {
        var entry = await _commentaryRepository.GetByIdAsync(request.CommentaryId, cancellationToken)
            ?? throw new InvalidOperationException("Commentary entry not found.");

        if (entry.FixtureId != request.FixtureId)
        {
            throw new InvalidOperationException("Commentary entry does not belong to the specified fixture.");
        }

        var fixture = await _fixtureRepository.GetByIdAsync(request.FixtureId, cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        // Only the note/comment text can be changed here - Side, PlayerId, Action, and the
        // score they already produced are immutable once recorded.
        entry.UpdateNote(request.Note);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var fixtureName = $"{fixture.HomeTeam?.Name} v {fixture.AwayTeam?.Name}";

        var dto = new CommentaryDto(
            entry.Id,
            entry.FixtureId,
            entry.Side.ToString(),
            entry.PlayerId ?? Guid.Empty,
            entry.Player?.Name ?? string.Empty,
            entry.Action.ToString(),
            entry.Note,
            entry.Ball,
            entry.CreatedAtUtc,
            fixture.HomeScore.Runs,
            fixture.HomeScore.Wickets,
            fixture.AwayScore.Runs,
            fixture.AwayScore.Wickets,
            fixtureName,
            fixture.Sport?.Name ?? string.Empty);

        await _broadcaster.BroadcastUpdateAsync(dto, cancellationToken);

        return dto;
    }
}
