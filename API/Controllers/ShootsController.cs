using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShootsController : ControllerBase
    {        
        // services
        private readonly ILogger<ShipsController> _logger;
        private readonly IShootService      _shootService;

        public ShootsController(ILogger<ShipsController> logger, IShootService shootService)
        {
            _logger       = logger;
            _shootService = shootService;
        }

        /// <summary>
        /// Processes a shot fired result
        /// </summary>
        /// <param name="consumer">The unique key that will be used to data cache</param>
        /// <param name="row">The row number of the grid where the shot is fired</param>
        /// <param name="column">The column number of the grid where the shot is fired</param>
        /// <returns>
        /// A JsonResult that contains the outcome of the shot
        /// </returns>
        [HttpGet("ShootResult")]
        public async Task<JsonResult> ShootResult([FromHeader(Name = "X-consumer")] string consumer, int row, int column)
        {
            try
            {
                // checks the key
                if (string.IsNullOrEmpty(consumer))
                    // returns the result
                    return new JsonResult(new { Message = "No consumer key found." });

                // gets shoot result
                var history = await _shootService.GetShootResult(row, column, consumer);

                // returns the result
                return new JsonResult(new { history.Message, history.Success, history.Data });

            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }
    }
}
