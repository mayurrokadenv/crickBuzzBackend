using MediatR;

namespace MatchApi.Application.Features.SeriesManagement.Commands.CreateSeries;

public record CreateSeriesCommand(
    string Name,
    Guid SportId,
    List<Guid> TeamIds
) : IRequest<Guid>;