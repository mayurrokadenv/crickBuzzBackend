using MatchApi.Application.Common.Interfaces;
using MatchApi.Application.Features.SeriesManagement.Queries.GetPointsTable;
using MatchApi.Domain.DTOs;
using MatchApi.Domain.Enums;
using MediatR;

namespace MatchApi.Application.Features.SeriesManagement.Queries
    .GetSeriesPointsTable;

public class GetSeriesPointsTableQueryHandler
    : IRequestHandler<
        GetSeriesPointsTableQuery,
        IReadOnlyList<SeriesPointsTableDto>>
{
    private readonly ISeriesRepository _seriesRepository;

    public GetSeriesPointsTableQueryHandler(
        ISeriesRepository seriesRepository)
    {
        _seriesRepository = seriesRepository;
    }

    public async Task<IReadOnlyList<SeriesPointsTableDto>> Handle(
        GetSeriesPointsTableQuery request,
        CancellationToken cancellationToken)
    {
        var series = await _seriesRepository
            .GetSeriesByIdAsync(
                request.SeriesId,
                cancellationToken);

        if (series is null)
        {
            throw new KeyNotFoundException(
                $"Series with ID {request.SeriesId} was not found.");
        }

        var completedFixtures = series.Fixtures
            .Where(f => f.Status == MatchStatus.Completed)
            .ToList();

        var pointsTable = series.SeriesTeams
            .Select(seriesTeam =>
            {
                var teamId = seriesTeam.TeamId;

                var teamFixtures = completedFixtures
                    .Where(f =>
                        f.HomeTeamId == teamId ||
                        f.AwayTeamId == teamId)
                    .ToList();

                var played = teamFixtures.Count;

                var won = teamFixtures.Count(f =>
                    f.WinningTeamId == teamId);

                var lost = teamFixtures.Count(f =>
                    f.WinningTeamId.HasValue &&
                    f.WinningTeamId != teamId);

                var points = won * 2;

                return new SeriesPointsTableDto
                {
                    TeamId = teamId,

                    TeamName = seriesTeam.Team?.Name
                        ?? string.Empty,

                    Played = played,

                    Won = won,

                    Lost = lost,

                    Points = points
                };
            })
            .OrderByDescending(x => x.Points)
            .ThenByDescending(x => x.Won)
            .ThenBy(x => x.TeamName)
            .ToList();

        return pointsTable;
    }
}