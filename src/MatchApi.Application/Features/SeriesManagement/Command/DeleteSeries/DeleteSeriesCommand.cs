using MediatR;

namespace MatchApi.Application.Features.SeriesManagement.Commands.DeleteSeries;

public record DeleteSeriesCommand(Guid SeriesId) : IRequest;