using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShipsController : ControllerBase
    {
        // services
        private readonly ILogger<ShipsController> _logger;
        private readonly IShipService        _shipService;
        private readonly IShootService      _shootService;

        public ShipsController(ILogger<ShipsController> logger, IShipService shipService, IShootService shootService)
        {
            _logger       = logger;
            _shipService  = shipService;
            _shootService = shootService;
        }

        /// <summary>
        /// Processes the ships placing process on the grid
        /// </summary>
        /// <param name="consumer">The unique key that will be used to data cache</param>
        /// <returns>
        /// A JsonResult that indicates the success or failure of the ship placement operation
        /// </returns>
        [HttpGet("PlaceShips")]
        public async Task<JsonResult> PlaceShips([FromHeader(Name = "X-consumer")] string consumer)
        {
            try
            {
                // checks the key
                if (string.IsNullOrEmpty(consumer))
                    // returns the result
                    return new JsonResult(new { Message = "No consumer key found." });

                // gets ships placed result
                var result = await _shipService.GetShipList(consumer);

                // returns the result
                return new JsonResult(new { result.Message, result.Success, result.Data });

            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }
    }
}
