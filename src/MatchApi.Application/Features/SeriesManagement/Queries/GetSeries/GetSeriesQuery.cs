using MatchApi.Domain.DTOs;
using MediatR;

namespace MatchApi.Application.Features.SeriesManagement.Queries.GetSeries;

public record GetSeriesQuery
    : IRequest<IReadOnlyList<SeriesDto>>;