using MatchApi.Domain.Entities;

namespace MatchApi.Application.Common.Interfaces;

public interface IFixtureRepository
{
    Task AddAsync(Fixture fixture, CancellationToken cancellationToken);

    Task<Fixture?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Fixture>> GetLiveAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Fixture>> GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<Fixture>> SearchAsync(string searchTerm, CancellationToken cancellationToken);

}
