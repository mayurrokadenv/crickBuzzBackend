using MatchApi.Domain.Entities;

namespace MatchApi.Application.Common.Interfaces;

public interface ITeamRepository
{
    Task AddAsync(Team team, CancellationToken cancellationToken);
    Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Team>> GetTeamsAsync(CancellationToken cancellationToken);
    Task DeleteTeamAsync(Guid teamId, CancellationToken cancellationToken);
    Task UpdateTeamAsync(Team team, CancellationToken cancellationToken);
    Task<bool> isTeamExixst(
    Guid sportId,
    string teamName,
    CancellationToken cancellationToken);
    Task<bool> IsTeamUsedAsync(
    Guid teamId,
    CancellationToken cancellationToken);

}
