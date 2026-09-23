using MatchApi.Application.Common.Interfaces;
using MatchApi.Domain.DTOs;
using MediatR;

namespace MatchApi.Application.Features.Teams.Queries.GetTeams
{
    public class GetTeamsQueryHandler
     : IRequestHandler<GetTeamsQuery, List<GetTeamsResponse>>
    {
        private readonly ITeamRepository _repository;

        public GetTeamsQueryHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetTeamsResponse>> Handle(
            GetTeamsQuery request,
            CancellationToken cancellationToken)
        {
            var teams = (await _repository.GetTeamsAsync(cancellationToken))
                        .OrderByDescending(team => team.CreatedAtUtc).ToList();

            return teams.Select(team => new GetTeamsResponse
            {
                Id = team.Id,
                TeamName = team.Name,
                SportId = team.SportId,
                Sport = new GetSportResponse() { 
                Name = team.Sport.Name,
                Description = team.Sport.Description
                },
                Color = team.ColorHex,
                players = team.Players
    .OrderByDescending(player => player.CreatedAtUtc)
    .Select(player => new GetPlayerResponse()
    {
                    Role = player.SportRole.RoleName,
                    PlayerName = player.Name,
                    PlayerId = player.Id,
                    RoleId = player.SportRoleId
                }).ToList()
            }).ToList();
        }
    }
}
