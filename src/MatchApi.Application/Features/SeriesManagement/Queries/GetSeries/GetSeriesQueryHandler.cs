using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.DTOs;
using MediatR;

namespace MatchApi.Application.Features.SeriesManagement.Queries.GetSeries;

public class GetSeriesQueryHandler
    : IRequestHandler<GetSeriesQuery, IReadOnlyList<SeriesDto>>
{
    private readonly ISeriesRepository _seriesRepository;

    public GetSeriesQueryHandler(
        ISeriesRepository seriesRepository)
    {
        _seriesRepository = seriesRepository;
    }

    public async Task<IReadOnlyList<SeriesDto>> Handle(
        GetSeriesQuery request,
        CancellationToken cancellationToken)
    {
        var series = await _seriesRepository.GetAllAsync(
            cancellationToken);

        return series
            .Select(s => new SeriesDto
            {
                Id = s.Id,
                Name = s.Name,
                SportId = s.SportId,
                SportName = s.Sport?.Name ?? string.Empty,

                Teams = s.SeriesTeams
                    .Select(st => new SeriesTeamDto
                    {
                        TeamId = st.TeamId,
                        TeamName = st.Team?.Name ?? string.Empty
                    })
                    .ToList()
            })
            .ToList();
    }
}