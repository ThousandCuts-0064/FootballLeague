using System;
using FootballLeague.Application.Common;
using FootballLeague.Application.Common.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.API.Controllers
{
    [Route("[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public abstract class FootballLeagueBaseController : ControllerBase
    {
        public IActionResult ResultToResponse<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result);

            return result.Error switch
            {
                NotFoundError => NotFound(result),
                ValidationError => UnprocessableEntity(result),
                ConflictError => Conflict(result),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(result),
                    $"Error of type {result.Error.GetType().Name} not mapped")
            };
        }
    }
}