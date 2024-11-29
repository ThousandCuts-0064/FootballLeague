using System.Collections.Generic;
using System.Threading.Tasks;
using FootballLeague.Application.Common;
using FootballLeague.Application.Teams.Dtos;

namespace FootballLeague.Application.Teams
{
    public interface ITeamService
    {
        public Task<Result<IReadOnlyCollection<TeamDto>>> GetPageAsync(GetTeamsPageDto dto);
        public Task<Result<TeamWithDetailsDto>> FindByNameAsync(string name);
        public Task<Result<TeamDto>> CreateAsync(CreateTeamDto dto);
        public Task<Result<TeamDto>> UpdateNameByNameAsync(string name, UpdateTeamNameDto dto);
        public Task<Result<TeamDto>> DeleteByNameAsync(string name);
    }
}
