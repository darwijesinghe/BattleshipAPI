using Domain.Interfaces;
using Domain.Models;
using Domain.Response;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Services.Enums;
using Services.Interfaces;
using Domain.Helpers;

namespace Services.Classes
{
    /// <summary>
    /// Service implementation for shoot specific operations
    /// </summary>
    public class ShootService : BaseService, IShootService
    {        
        // Services
        private readonly ILogger<ShootService>   _logger;
        private readonly IShootHandle       _shootHandle;

        public ShootService(IShootHandle shootHandle, IMemoryCache cache, ILogger<ShootService> logger) : base(cache, logger)
        {
            _shootHandle = shootHandle;
            _logger      = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves the result of a shoot based on the provided shoot (row and column) position.
        /// </summary>
        /// <param name="row">The row position of the shot.</param>
        /// <param name="column">The column position of the shot.</param>
        /// <param name="consumer">The consumer application unique value.</param>
        /// <returns>
        /// The <see cref="Result{ShootResult}"/> object which includes the shoot result.
        /// </returns>
        public async Task<Result<ShootResult>> GetShootResult(int row, int column, string consumer)
        {
            try
            {
                // shoot position
                var position = new ShootPosition(row, column);

                // make cache key
                string shipKey  = $"{consumer}-{ServiceCacheKeys.AllShips}";
                string shootKey = $"{consumer}-{ServiceCacheKeys.ShootResult}";

                // gets ship list
                var shipList = await GetCached<Result<List<Ship>>>(shipKey);
                if (shipList is null || !shipList.Data.HasValue())
                    return new Result<ShootResult> { Message = "No ship(s) were found to shoot." };

                // gets shoot history
                var shootHistory = await GetCached<Result<ShootResult>>(shootKey);
                if (shootHistory is not null)
                    // removes the history for fresh data
                    RemoveCached(shootKey);

                // gets shoot result and cache it
                var result = await CachedLong(shootKey, async () =>
                {
                    return await _shootHandle.ShootResult(shipList.Data, shootHistory?.Data ?? new ShootResult (), position);
                });

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
