using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.Entities;
using MatchApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchApi.Infrastructure.Repositories
{
    public class SportRepository: ISportRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SportRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task AddSportAsync(
          Sport sport,
          CancellationToken cancellationToken)
        {
            await _context.Sports.AddAsync(sport, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<Sport>> GetSortsAsync(CancellationToken cancellationToken)
        {
            return await _context.Sports
                .OrderBy(t => t.Name).Include(t => t.Roles)
                .ToListAsync(cancellationToken);

        }
        public async Task<bool> isSportExist(string sportName, CancellationToken cancellationToken)
        {
            var sport = await _context.Sports.FirstOrDefaultAsync(s => s.Name.ToLower() == sportName.ToLower(), cancellationToken);
            return sport != null;

        }
        public async Task<Sport?> GetByIdAsync(
       Guid sportId,
       CancellationToken cancellationToken)
        {
            return await _context.Sports
                .FirstOrDefaultAsync(
                    s => s.Id == sportId,
                    cancellationToken);
        }
    }
}
