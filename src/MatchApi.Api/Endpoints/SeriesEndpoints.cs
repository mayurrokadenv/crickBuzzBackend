using MatchApi.Application.Features.SeriesManagement.Commands.CreateSeries;
using MediatR;

namespace MatchApi.Api.Endpoints;

public static class SeriesEndpoints
{
    public static IEndpointRouteBuilder MapSeriesEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/series")
            .WithTags("Series");

        group.MapPost("/", CreateSeries)
            .WithName("CreateSeries")
            .WithSummary("Creates a series with selected teams")
            .Produces<Guid>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> CreateSeries(
        CreateSeriesRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var seriesId = await sender.Send(
                new CreateSeriesCommand(
                    request.Name,
                    request.SportId,
                    request.TeamIds),
                cancellationToken);

            return Results.Ok(seriesId);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(
                ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }
}

public record CreateSeriesRequest(
    string Name,
    Guid SportId,
    List<Guid> TeamIds);