#if DEBUG

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.API.Controllers
{
    public class TestsController : FootballLeagueBaseController
    {
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public IActionResult UnhandledException()
        {
            throw new InvalidOperationException(nameof(UnhandledException));
        }
    }
}

#endif
