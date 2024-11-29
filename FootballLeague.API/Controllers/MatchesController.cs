using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballLeague.API.Requests.Matches;
using FootballLeague.Application.Common;
using FootballLeague.Application.Matches;
using FootballLeague.Application.Matches.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.API.Controllers
{
    public class MatchesController : FootballLeagueBaseController
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Result<IReadOnlyCollection<MatchDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] GetMatchesPageRequest request)
        {
            var teams = await _matchService.GetPageAsync(request.ToDto());

            return Ok(teams);
        }

        [HttpGet("{key:guid}")]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindByKey(Guid key)
        {
            var result = await _matchService.FindByKeyAsync(key);

            return ResultToResponse(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateMatchRequest request)
        {
            var result = await _matchService.CreateAsync(request.ToDto());

            return ResultToResponse(result);
        }

        [HttpPatch("{key:guid}")]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateScoresByKey(Guid key, [FromBody] UpdateMatchScoresRequest request)
        {
            var result = await _matchService.UpdateScoresByKeyAsync(key, request.ToDto());

            return ResultToResponse(result);
        }

        [HttpDelete("{key:guid}")]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<MatchDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteByKey(Guid key)
        {
            var result = await _matchService.DeleteByKeyAsync(key);

            return ResultToResponse(result);
        }
    }
}