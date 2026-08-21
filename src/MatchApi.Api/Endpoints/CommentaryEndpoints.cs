using FluentValidation;
using MatchApi.Application.Features.Commentary.Commands.CreateCommentary;
using MatchApi.Application.Features.Commentary.Commands.UpdateCommentary;
using MatchApi.Application.Features.Commentary.Common;
using MatchApi.Application.Features.Commentary.Queries.GetCommentary;
using MatchApi.Domain.Enums;
using MediatR;

namespace MatchApi.Api.Endpoints;

public static class CommentaryEndpoints
{
    public static IEndpointRouteBuilder MapCommentaryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/fixtures/{fixtureId:guid}/commentary")
            .WithTags("Commentary");

        group.MapPost("/", CreateCommentary)
            .WithName("CreateCommentary")
            .WithSummary("Adds a ball-by-ball commentary entry to a live fixture and broadcasts it in real time via SignalR")
            .Produces<CommentaryDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/", GetCommentary)
            .WithName("GetCommentary")
            .WithSummary("Gets all commentary entries for a fixture, most recent first")
            .Produces<IReadOnlyList<CommentaryDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPatch("/{commentaryId:guid}", UpdateCommentary)
            .WithName("UpdateCommentary")
            .WithSummary("Updates the note/comment text of an existing commentary entry (nothing else is editable)")
            .Produces<CommentaryDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> GetCommentary(
        Guid fixtureId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetCommentaryQuery(fixtureId), cancellationToken);
            return Results.Ok(response);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    private static async Task<IResult> UpdateCommentary(
        Guid fixtureId,
        Guid commentaryId,
        UpdateCommentaryRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new UpdateCommentaryCommand(fixtureId, commentaryId, request.Note), cancellationToken);
            return Results.Ok(response);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    private static async Task<IResult> CreateCommentary(
        Guid fixtureId,
        CreateCommentaryRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateCommentaryCommand(
                fixtureId,
                request.Side,
                request.PlayerId,
                request.Action,
                 request.CurrentBall,
                request.Note);

            var response = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/fixtures/{fixtureId}/commentary/{response.Id}", response);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }
}

public record CreateCommentaryRequest(FixtureSide Side, Guid PlayerId, CommentaryAction Action,string? CurrentBall, string? Note);

public record UpdateCommentaryRequest(string? Note);
