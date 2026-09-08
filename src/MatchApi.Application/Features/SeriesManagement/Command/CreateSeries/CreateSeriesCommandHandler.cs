using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.Entities;
using MediatR;

namespace MatchApi.Application.Features.SeriesManagement.Commands.CreateSeries;

public class CreateSeriesCommandHandler
    : IRequestHandler<CreateSeriesCommand, Guid>
{
    private readonly ISeriesRepository _seriesRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSeriesCommandHandler(
        ISeriesRepository seriesRepository,
        ITeamRepository teamRepository,
        IUnitOfWork unitOfWork)
    {
        _seriesRepository = seriesRepository;
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateSeriesCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate team selection
        if (request.TeamIds == null || request.TeamIds.Count < 2)
        {
            throw new InvalidOperationException(
                "A series must have at least two teams.");
        }

        // 2. Prevent duplicate teams
        if (request.TeamIds.Distinct().Count() != request.TeamIds.Count)
        {
            throw new InvalidOperationException(
                "A team cannot be added to a series more than once.");
        }

        // 3. Get selected teams
        var teams = new List<Team>();

        foreach (var teamId in request.TeamIds)
        {
            var team = await _teamRepository.GetByIdAsync(
                teamId,
                cancellationToken);

            if (team is null)
            {
                throw new InvalidOperationException(
                    $"Team with id '{teamId}' was not found.");
            }

            teams.Add(team);
        }

        // 4. Validate all teams belong to the selected sport
        if (teams.Any(t => t.SportId != request.SportId))
        {
            throw new InvalidOperationException(
                "All selected teams must belong to the selected sport.");
        }

        // 5. Create Series
        var series = new Series
        {
            Name = request.Name,
            SportId = request.SportId
        };

        // 6. Create SeriesTeam records
        foreach (var team in teams)
        {
            series.SeriesTeams.Add(new SeriesTeam
            {
                SeriesId = series.Id,
                SeriesName = series.Name,
                TeamId = team.Id
            });
        }

        // 7. Save Series + SeriesTeams
        await _seriesRepository.AddAsync(
            series,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 8. Return newly created Series ID
        return series.Id;
    }
}