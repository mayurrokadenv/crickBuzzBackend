using MatchApi.Application.Common.Interfaces;
using MediatR;

namespace MatchApi.Application.Features.Players.Queries.GetPlayers
{
    public class GetPlayersQueryHandler
     : IRequestHandler<GetPlayersQuery, List<GetPlayersResponse>>
    {
        private readonly IPlayerRepository _repository;

        public GetPlayersQueryHandler(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetPlayersResponse>> Handle(
            GetPlayersQuery request,
            CancellationToken cancellationToken)
        {
            var players = (await _repository.GetPlayersByTeamIdAsync(
      request.TeamId,
      cancellationToken))
      .OrderByDescending(player => player.CreatedAtUtc)
      .ToList();

            return players.Select(player => new GetPlayersResponse
            {
                Id = player.Id,
                TeamId = player.TeamId,
                PlayerName = player.Name,
                SportRoleId = player.SportRoleId,
                SportRole = new SportRoleResponse
                {
                    RoleName = player.SportRole.RoleName,
                    Description = player.SportRole.Description
                },
            }).ToList();
        }
    }
}
