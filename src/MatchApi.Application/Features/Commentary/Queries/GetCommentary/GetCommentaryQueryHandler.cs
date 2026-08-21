using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.Commentary.Common;
using MediatR;

namespace MatchApi.Application.Features.Commentary.Queries.GetCommentary;

public class GetCommentaryQueryHandler : IRequestHandler<GetCommentaryQuery, IReadOnlyList<CommentaryDto>>
{
    private readonly IFixtureRepository _fixtureRepository;
    private readonly ICommentaryRepository _commentaryRepository;

    public GetCommentaryQueryHandler(IFixtureRepository fixtureRepository, ICommentaryRepository commentaryRepository)
    {
        _fixtureRepository = fixtureRepository;
        _commentaryRepository = commentaryRepository;
    }

    public async Task<IReadOnlyList<CommentaryDto>> Handle(GetCommentaryQuery request, CancellationToken cancellationToken)
    {
        var fixture = await _fixtureRepository.GetByIdAsync(request.FixtureId, cancellationToken)
            ?? throw new InvalidOperationException("Fixture not found.");

        var entries = await _commentaryRepository.GetByFixtureIdAsync(request.FixtureId, cancellationToken);

        var fixtureName = $"{fixture.HomeTeam?.Name} v {fixture.AwayTeam?.Name}";
        var sportName = fixture.Sport?.Name ?? string.Empty;

        return entries
            .OrderByDescending(e => e.CreatedAtUtc)
            .Select(e => new CommentaryDto(
                e.Id,
                e.FixtureId,
                e.Side.ToString(),
                e.PlayerId ?? Guid.Empty,
                e.Player?.Name ?? string.Empty,
                e.Action.ToString(),
                e.Note,
                e.Ball,
                e.CreatedAtUtc,
                fixture.HomeScore.Runs,
                fixture.HomeScore.Wickets,
                fixture.AwayScore.Runs,
                fixture.AwayScore.Wickets,
                fixtureName,
                sportName))
            .ToList();
    }
}
