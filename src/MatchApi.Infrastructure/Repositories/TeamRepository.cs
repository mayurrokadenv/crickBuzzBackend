using Azure.Core;
using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.Entities;
using MatchApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MatchApi.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public TeamRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }
    public async Task AddAsync(Team team, CancellationToken cancellationToken)
    {
        await _context.Teams.AddAsync(team, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    public Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Teams.Include(p => p.Sport).Include(s => s.Players).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
    public async Task<List<Team>> GetTeamsAsync(CancellationToken cancellationToken)
    {
        return await _context.Teams
            .OrderBy(t => t.Name).Include(t => t.Players).ThenInclude(p => p.SportRole).Include(p => p.Sport)
            .ToListAsync(cancellationToken);
    }
    public async Task DeleteTeamAsync(Guid teamId, CancellationToken cancellationToken)
    {
        var team = await _context.Teams
               .FirstOrDefaultAsync(
                   t => t.Id == teamId,
                   cancellationToken);

        if (team == null)
            throw new KeyNotFoundException("Team not found.");

        _context.Teams.Remove(team);

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateTeamAsync(Team request, CancellationToken cancellationToken)
    {
        var teamToUpdate = await _context.Teams
             .FirstOrDefaultAsync(
                 t => t.Id == request.Id,
                 cancellationToken);

        if (teamToUpdate == null)
            throw new KeyNotFoundException("Team not found.");

        teamToUpdate.Name = request.Name;
        teamToUpdate.SportId = request.SportId;
        teamToUpdate.ColorHex = request.ColorHex;

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> isTeamExixst(
    Guid sportId,
    string teamName,
    CancellationToken cancellationToken)
    {
        var team = await _context.Teams
            .FirstOrDefaultAsync(x => x.SportId == sportId && x.Name.ToLower() == teamName.ToLower(), cancellationToken);
        return team != null;
    }

    public async Task<bool> IsTeamUsedAsync(
    Guid teamId,
    CancellationToken cancellationToken)
    {
        var isUsedInSeries = await _context.SeriesTeams
            .AnyAsync(
                x => x.TeamId == teamId,
                cancellationToken);

        if (isUsedInSeries)
            return true;

        var isUsedInFixture = await _context.Fixtures
            .AnyAsync(
                x => x.HomeTeamId == teamId ||
                     x.AwayTeamId == teamId,
                cancellationToken);

        return isUsedInFixture;
    }
}
