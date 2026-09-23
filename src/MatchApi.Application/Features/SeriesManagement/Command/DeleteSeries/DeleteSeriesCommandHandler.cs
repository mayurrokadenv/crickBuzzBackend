using MatchApi.Application.Common.Interfaces;
using MediatR;

namespace MatchApi.Application.Features.SeriesManagement.Commands.DeleteSeries;

public class DeleteSeriesCommandHandler
    : IRequestHandler<DeleteSeriesCommand>
{
    private readonly ISeriesRepository _seriesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSeriesCommandHandler(
        ISeriesRepository seriesRepository,
        IUnitOfWork unitOfWork)
    {
        _seriesRepository = seriesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteSeriesCommand request,
        CancellationToken cancellationToken)
    {
        var series = await _seriesRepository.GetSeriesByIdAsync(
            request.SeriesId,
            cancellationToken);

        if (series is null)
            throw new InvalidOperationException("Series not found.");

        _seriesRepository.Delete(series);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}