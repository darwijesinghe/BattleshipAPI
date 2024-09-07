using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Services.Classes;
using Services.Interfaces;
using Domain.Handling;

namespace Test
{
    /// <summary>
    /// Test class for the ship placements
    /// </summary>
    [TestClass]
    public class ShipPlacementTest
    {
        // services
        private IMemoryCache                  _cache;
        private IShipHandle              _shipHandle;
        private IShipService            _shipService;

        // logs
        private ILogger<ShipService>        _slogger;
        private ILogger<ShipPlacementTest>   _logger;

        [TestInitialize]
        public void Setup()
        {
            try
            {
                // set up a service provider to support logging in the application
                var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

                // add logger services
                _slogger = serviceProvider.GetRequiredService<ILogger<ShipService>>();
                _logger  = serviceProvider.GetRequiredService<ILogger<ShipPlacementTest>>();

                // initialize an in-memory cache
                _cache = new MemoryCache(new MemoryCacheOptions());

                // init ship service handler
                _shipHandle = new ShipHandle();

                // initialize service
                _shipService = new ShipService(_shipHandle, _cache, _slogger);
            }
            catch (Exception ex)
            {
                // logs any exceptions that occur during the setup process
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Tests the retrieval of all ships data with the placement information
        /// </summary>
        [TestMethod]
        public async Task GetShipListTest()
        {
            // Act: Retrieves all the ship placement data using the service.
            var result = await _shipService.GetShipList("test-key");

            try
            {
                // Assert: Ensures the result is not null, indicating ship info and placement data were retrieved successfully.
                Assert.IsNotNull(result, "Ship placement process has not succeeded.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion.
                Assert.Fail(ex.Message);
            }
        }
    }
}
