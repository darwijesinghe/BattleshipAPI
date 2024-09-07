using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Response;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Services.Enums;
using Services.Interfaces;

namespace Services.Classes
{
    /// <summary>
    /// Service implementation for ship specific operations
    /// </summary>
    public class ShipService : BaseService, IShipService
    {
        // services
        private readonly ILogger<ShipService> _logger;
        private readonly IShipHandle      _shipHandle;

        public ShipService(IShipHandle shipHandle, IMemoryCache cache, ILogger<ShipService> logger) : base(cache, logger)
        {
            _shipHandle = shipHandle;
            _logger     = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves a list of ships
        /// </summary>
        /// <param name="consumer">The consumer application unique value</param>
        /// <returns>
        /// The <see cref="Result"/> object which includes a list of ships if the operation is successful
        /// </returns>
        public async Task<Result<List<Ship>>> GetShipList(string consumer)
        {
            try
            {
                // creates ship type list
                var shipTypes = Enum.GetValues(typeof(ShipType)).Cast<ShipType>().ToList();

                // make cache key
                string shipkey  = $"{consumer}-{ServiceCacheKeys.AllShips}";
                string shootkey = $"{consumer}-{ServiceCacheKeys.ShootResult}";

                // clears the cache for fresh data
                RemoveCached(shipkey);
                RemoveCached(shootkey);

                // gets the result
                var result = await CachedLong(shipkey, async () =>
                {
                    return await _shipHandle.ShipList(shipTypes);
                });

                // returns result
                return result;
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
