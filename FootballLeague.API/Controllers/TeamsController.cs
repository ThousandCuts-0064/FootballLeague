using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FootballLeague.API.Requests.Teams;
using FootballLeague.Application.Common;
using FootballLeague.Application.Teams;
using FootballLeague.Application.Teams.Dtos;
using FootballLeague.Domain.Teams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.API.Controllers
{
    public class TeamsController : FootballLeagueBaseController
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Result<IReadOnlyCollection<TeamDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<IReadOnlyCollection<TeamDto>>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetPage([FromQuery] GetTeamsPageRequest request)
        {
            var result = await _teamService.GetPageAsync(request.ToDto());

            return ResultToResponse(result);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> FindByName(
            [StringLength(TeamConstraint.NameMaxLength, MinimumLength = TeamConstraint.NameMinLength)]
            string name)
        {
            var result = await _teamService.FindByNameAsync(name);

            return ResultToResponse(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateTeamRequest request)
        {
            var result = await _teamService.CreateAsync(request.ToDto());

            return ResultToResponse(result);
        }

        [HttpPatch("{name}")]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateNameByName(
            [StringLength(TeamConstraint.NameMaxLength, MinimumLength = TeamConstraint.NameMinLength)]
            string name,
            [FromBody] UpdateTeamNameRequest request)
        {
            var result = await _teamService.UpdateNameByNameAsync(name, request.ToDto());

            return ResultToResponse(result);
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<TeamDto>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteByName(
            [StringLength(TeamConstraint.NameMaxLength, MinimumLength = TeamConstraint.NameMinLength)]
            string name)
        {
            var result = await _teamService.DeleteByNameAsync(name);

            return ResultToResponse(result);
        }
    }
}