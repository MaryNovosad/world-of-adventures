using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldOfAdventures.BusinessLogic;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.Api.Controllers
{
    [ApiController]
    [Route("v1/users-adventures")]
    public class UsersAdventuresController : ControllerBase
    {
        private readonly IAdventureService _adventureService;
        private readonly ILogger<AdventuresController> _logger;

        public UsersAdventuresController(IAdventureService adventureService, ILogger<AdventuresController> logger)
        {
            _adventureService = adventureService;
            _logger = logger;
        }

        /// <summary>
        /// Gets user adventure with the highlighted answers which user has chosen
        /// </summary>
        /// <param name="adventureName"> Adventure name </param>
        /// <param name="userName"> User name </param>
        /// <returns> User adventure tree with the highlighted answers which user has chosen </returns>
        [HttpGet("{userName}/adventures/{adventureName}")]
        public async Task<UserAdventure> Get(string userName, string adventureName)
        {
            return await _adventureService.FindAsync(userName, adventureName);
        }

        /// <summary>кл
        /// Appends user answer which user has chosen on current adventure level to user's adventure tree
        /// </summary>
        /// <param name="userName"> User name </param>
        /// <param name="adventureName"> Name of an adventure which user is currently taking </param>
        /// <param name="userChoice"> Answer which user has chosen on the provided adventure level </param>
        /// <returns> Action result </returns>
        [HttpPut("{userName}/adventures/{adventureName}")]
        public async Task<IActionResult> ChooseAnswer(string userName, string adventureName, UserChoice userChoice)
        {
            if (userChoice.AdventureLevel <= 0)
            {
                return BadRequest("Adventure level should be greater than 0");
            }

            try
            {
                await _adventureService.ChooseAnswerAsync(userName, adventureName, userChoice);

                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unknown error has occurred");
                throw;
            }
        }
    }
}
