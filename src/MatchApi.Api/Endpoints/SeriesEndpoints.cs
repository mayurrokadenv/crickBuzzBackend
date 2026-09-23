using MatchApi.Application.Features.SeriesManagement.Commands.CreateSeries;
using MatchApi.Application.Features.SeriesManagement.Commands.DeleteSeries;
using MatchApi.Application.Features.SeriesManagement.Queries.GetPointsTable;
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

        group.MapGet("/{seriesId:guid}/points-table", GetPointsTable)
            .WithName("GetSeriesPointsTable")
            .WithSummary("Gets points table for a series")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{seriesId:guid}", DeleteSeries)
            .WithName("DeleteSeries")
            .WithSummary("Deletes a series")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);


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

    private static async Task<IResult> GetPointsTable(
    Guid seriesId,
    ISender sender,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetSeriesPointsTableQuery(seriesId),
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteSeries(
    Guid seriesId,
    ISender sender,
    CancellationToken cancellationToken)
    {
        try
        {
            await sender.Send(
                new DeleteSeriesCommand(seriesId),
                cancellationToken);

            return Results.NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(
                ex.Message,
                statusCode: StatusCodes.Status404NotFound);
        }
    }
}

public record CreateSeriesRequest(
    string Name,
    Guid SportId,
    List<Guid> TeamIds);