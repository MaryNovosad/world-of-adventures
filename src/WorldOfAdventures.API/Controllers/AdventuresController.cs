using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldOfAdventures.API.Requests;
using WorldOfAdventures.BusinessLogic;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.Api.Controllers
{
    [ApiController]
    [Route("v1/adventures")]
    public class AdventuresController : ControllerBase
    {
        private readonly IAdventureService _adventureService;
        private readonly ILogger<AdventuresController> _logger;

        public AdventuresController(IAdventureService adventureService, ILogger<AdventuresController> logger)
        {
            _adventureService = adventureService;
            _logger = logger;
        }

        /// <summary>
        /// Gets an adventure template
        /// </summary>
        /// <param name="name"> Adventure name </param>
        /// <returns> An adventure template </returns>
        [HttpGet("{name}")]
        public async Task<Adventure> Get(string name)
        {
            return await _adventureService.FindAsync(name);
        }

        /// <summary>
        /// If no adventures with the provided name exist - the adventure is created,
        /// otherwise in case no user has tried the adventure yet - the adventure is updated.
        /// 
        /// After someone has already tried the adventure - it's impossible to alter the adventure steps, in case of attempt an error will be returned.
        /// It is recommended to create a new adventure with a new name instead.
        /// </summary>
        /// <param name="name"> Adventure name </param>
        /// <param name="request"> The whole adventure template representation </param>
        /// <returns> Action result </returns>
        [HttpPut("{name}")]
        public async Task<IActionResult> CreateOrUpdate(string name, [FromBody]CreateOrUpdateAdventureRequest request)
        {
            try
            {
                await _adventureService.CreateOrUpdateAsync(name, MapAdventure(request));

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

        private Adventure MapAdventure(CreateOrUpdateAdventureRequest request)
        {
            return new Adventure(request.Name, request.InitialStep);
        }
    }
}
