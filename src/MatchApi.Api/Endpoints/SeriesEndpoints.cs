using MatchApi.Application.Features.SeriesManagement.Commands.CreateSeries;
using MatchApi.Application.Features.SeriesManagement.Queries.GetSeries;
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

        group.MapGet("/", GetSeries)
            .WithName("GetSeries")
            .WithSummary("Gets all series")
            .Produces(StatusCodes.Status200OK);

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

    private static async Task<IResult> GetSeries(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var series = await sender.Send(
            new GetSeriesQuery(),
            cancellationToken);

        return Results.Ok(series);
    }
}

public record CreateSeriesRequest(
    string Name,
    Guid SportId,
    List<Guid> TeamIds);