using MatchApi.Domain.DTOs;
using MediatR;

namespace MatchApi.Application.Features.SeriesManagement.Queries.GetPointsTable;

public record GetSeriesPointsTableQuery(Guid SeriesId)
    : IRequest<IReadOnlyList<SeriesPointsTableDto>>;