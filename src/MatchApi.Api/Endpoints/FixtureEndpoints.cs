using FluentValidation;
using MatchApi.Application.Features.Fixtures.Commands.CreateFixture;
using MatchApi.Application.Features.Fixtures.Commands.UpdateFixture;
using MatchApi.Application.Features.Fixtures.Commands.UpdateFixtureScore;
using MatchApi.Application.Features.Fixtures.Common;
using MatchApi.Application.Features.Fixtures.Queries.GetFixtureDetails;
using MatchApi.Application.Features.Fixtures.Queries.GetLiveFixtures;
using MatchApi.Application.Features.Fixtures.Queries.GetTopPerformers;
using MatchApi.Application.Features.Fixtures.Queries.SearchFixtures;
using MatchApi.Domain.Enums;
using MediatR;

namespace MatchApi.Api.Endpoints;

public static class FixtureEndpoints
{
    public static IEndpointRouteBuilder MapFixtureEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/fixtures")
            .WithTags("Fixtures");

        group.MapPost("/", CreateFixture)
            .WithName("CreateFixture")
            .WithSummary("Schedules a fixture between two teams")
            .Produces<CreateFixtureResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapGet("/live", GetLiveFixtures)
            .WithName("GetLiveFixtures")
            .WithSummary("Gets all fixtures that are currently live")
            .Produces<IReadOnlyList<FixtureDto>>(StatusCodes.Status200OK);

        group.MapGet("/all", GetAllFixtures)
            .WithName("GetAllFixtures")
            .WithSummary("Gets all fixtures ")
            .Produces<IReadOnlyList<FixtureDto>>(StatusCodes.Status200OK);

        group.MapGet("/{fixtureId:guid}", GetFixtureDetails)
            .WithName("GetFixtureDetails")
            .WithSummary("Gets the full match-center view for a fixture: info, score, commentary feed, and top performers")
            .Produces<FixtureDetailsDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{fixtureId:guid}/top-performers", GetTopPerformers)
            .WithName("GetTopPerformers")
            .WithSummary("Gets the top 4 run scorers in a fixture, by player and team")
            .Produces<IReadOnlyList<TopPerformerDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPatch("/{fixtureId:guid}", UpdateFixture)
            .WithName("UpdateFixture")
            .WithSummary("Updates a fixture's status and/or match phase (e.g. Scheduled -> Live, or set innings/half)")
            .Produces<FixtureDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);

       group.MapGet("/search", SearchFixtures)
            .WithName("SearchFixtures")
            .WithSummary("Search fixtures by team or sport");


        
        group.MapPatch("/{fixtureId:guid}/score", UpdateFixtureScore)
            .WithName("UpdateFixtureScore")
            .WithSummary("Adds to a team's existing score (and wickets, for cricket) on the fixture directly, without logging commentary")
            .Produces<FixtureDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> GetLiveFixtures(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetLiveFixturesQuery(), cancellationToken);
        return Results.Ok(response);
    }

    private static async Task<IResult> GetAllFixtures(ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetAllFixturesQuery(), cancellationToken);
        return Results.Ok(response);
    }


    private static async Task<IResult> GetFixtureDetails(
        Guid fixtureId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetFixtureDetailsQuery(fixtureId), cancellationToken);
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

    private static async Task<IResult> GetTopPerformers(
        Guid fixtureId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetTopPerformersQuery(fixtureId), cancellationToken);
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

    private static async Task<IResult> UpdateFixture(
        Guid fixtureId,
        UpdateFixtureRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new UpdateFixtureCommand(fixtureId, request.Status, request.Phase), cancellationToken);
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

    private static async Task<IResult> UpdateFixtureScore(
        Guid fixtureId,
        UpdateFixtureScoreRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(
                new UpdateFixtureScoreCommand(fixtureId, request.Side, request.BattingPlayerId,
        request.BowlingPlayerId, request.RunsDelta, request.Overs, request.WicketsDelta),
                cancellationToken);
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

    private static async Task<IResult> CreateFixture(
        CreateFixtureCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/fixtures/{response.Id}", response);
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


    private static async Task<IResult> SearchFixtures(
    string searchTerm,
    ISender sender,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new SearchFixturesQuery(searchTerm),
            cancellationToken);

        return Results.Ok(result);
    }

}

public record UpdateFixtureRequest(MatchStatus? Status, MatchPhase? Phase);

public record UpdateFixtureScoreRequest(
    FixtureSide Side,
    Guid BattingPlayerId,
    Guid BowlingPlayerId,
    int RunsDelta,
    string Overs,
    int? WicketsDelta);