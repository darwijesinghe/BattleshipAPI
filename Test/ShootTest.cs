using Domain.Enums;
using Domain.Handling;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Services.Classes;
using Services.Interfaces;

namespace Test
{
    /// <summary>
    /// Test class for the shoot cases
    /// </summary>
    [TestClass]
    public class ShootTest
    {
        // Services
        private IMemoryCache              _cache;
        private IShootHandle        _shootHandle;
        private IShipHandle          _shipHandle;
        private IShootService      _shootService;
        private IShipService        _shipService;

        // Logs
        private ILogger<ShootService>   _slogger;
        private ILogger<ShipService>    _xlogger;
        private ILogger<ShootTest>       _logger;

        [TestInitialize]
        public void Setup()
        {
            try
            {
                // set up a service provider to support logging in the application
                var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

                // add logger services
                _slogger            = serviceProvider.GetRequiredService<ILogger<ShootService>>();
                _xlogger            = serviceProvider.GetRequiredService<ILogger<ShipService>>();
                _logger             = serviceProvider.GetRequiredService<ILogger<ShootTest>>();

                // initialize an in-memory cache
                _cache              = new MemoryCache(new MemoryCacheOptions());

                // init shoot service handler
                _shootHandle        = new ShootHandle(_shipHandle);

                // initialize shoot service
                _shootService       = new ShootService(_shootHandle, _cache, _slogger);

                // init ship service handler
                _shipHandle         = new ShipHandle();

                // initialize service
                _shipService        = new ShipService(_shipHandle, _cache, _xlogger);

            }
            catch (Exception ex)
            {
                // logs any exceptions that occur during the setup process
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Tests the GetShootResult method to verify that shoot result 
        /// is returned for the given row and column coordinates.
        /// </summary>
        [TestMethod]
        public async Task GetShootResultTest()
        {
            // Arrange

            // Cache key
            string cacheKey = "test-key";
            // gets ship data
            var result      = await _shipService.GetShipList(cacheKey);

            // Act: Retrieves the shoot result
            var shootResult = await _shootService.GetShootResult(1, 1, cacheKey);

            try
            {
                // Assert: Ensures the result is not null, indicating shoot result data were retrieved successfully
                Assert.IsNotNull(shootResult.Data, "Ship position data not found in the cached.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests the hit status functionality by simulating a shot and verifying 
        /// if the correct status 'Hit' is returned based on the target coordinates.
        /// </summary>
        [TestMethod]
        public async Task HitStatusTest()
        {
            // Arrange

            // cache key
            string cacheKey = "test-key";
            // gets ship data
            var result      = await _shipService.GetShipList(cacheKey);
            // extract positions
            var positions   = result?.Data?.SelectMany(x => x.ShipPositions)?.ToList() ?? new List<ShipPosition>();

            var row         = positions.First().Row;
            var column      = positions.First().Column;

            // shoot status
            var status      = ShootStatus.Hit;

            // Act: Retrieves the shoot result
            var shootResult = await _shootService.GetShootResult(row, column, cacheKey);
            var shootStatus = shootResult?.Data?.ShootStatus;

            try
            {
                // Assert: Ensures the shoot status is not null
                Assert.IsNotNull(shootStatus, "Shoot status value is null on shoot result.");

                // Assert: Ensures the expected shoot status is matched with the actual status
                Assert.AreEqual(status, shootStatus, "Expected result is not matched with the act.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests the hit status functionality by simulating a shot and verifying 
        /// if the correct status 'Miss' is returned based on the target coordinates.
        /// </summary>
        [TestMethod]
        public async Task MissStatusTest()
        {
            // Arrange

            // cache key
            string cacheKey = "test-key";
            // gets ship data
            var result      = await _shipService.GetShipList(cacheKey);
            // extract positions
            var positions   = result?.Data?.SelectMany(x => x.ShipPositions)?.ToList() ?? new List<ShipPosition>();

            // 10 * 10 grid
            int[] rows      = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] columns   = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // extract the rows that are not occupied by any ship
            var missRows    = rows.Where(x => !positions.Select(r => r.Row).Contains(x)).ToList();
            // extract the columns that are not occupied by any ship
            var missColumns = columns.Where(x => !positions.Select(r => r.Column).Contains(x)).ToList();

            var row         = missRows.First();
            var column      = missColumns.First();

            // shoot status
            var status = ShootStatus.Miss;

            // Act: Retrieves the shoot result
            var shootResult = await _shootService.GetShootResult(row, column, cacheKey);
            var shootStatus = shootResult?.Data?.ShootStatus;

            try
            {
                // Assert: Ensures the shoot status is not null
                Assert.IsNotNull(shootStatus, "Shoot status value is null on shoot result.");

                // Assert: Ensures the expected shoot status is matched with the actual status
                Assert.AreEqual(status, shootStatus, "Expected result is not matched with the act.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests the hit status functionality by simulating a shot and verifying 
        /// if the correct status 'Invalid' is returned based on the target coordinates.
        /// </summary>
        [TestMethod]
        public async Task InvalidStatusTest()
        {
            // Arrange

            // cache key
            string cacheKey = "test-key";
            // gets ship data
            var result = await _shipService.GetShipList(cacheKey);

            // out of boundary values
            var row    = 12;
            var column = 5;

            // shoot status
            var status = ShootStatus.Invalid;

            // Act: Retrieves the shoot result
            var shootResult = await _shootService.GetShootResult(row, column, cacheKey);
            var shootStatus = shootResult?.Data?.ShootStatus;

            try
            {
                // Assert: Ensures the shoot status is not null
                Assert.IsNotNull(shootStatus, "Shoot status value is null on shoot result.");

                // Assert: Ensures the expected shoot status is matched with the actual status
                Assert.AreEqual(status, shootStatus, "Expected result is not matched with the act.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests the hit status functionality by simulating a shot and verifying 
        /// if the correct status 'Same' is returned based on the target coordinates.
        /// </summary>
        [TestMethod]
        public async Task SameStatusTest()
        {
            // Arrange

            // cache key
            string cacheKey = "test-key";
            // gets ship data
            var result = await _shipService.GetShipList(cacheKey);

            var row    = 1;
            var column = 5;

            // shoot status
            var status = ShootStatus.Same;

            // Act: Retrieves the shoot result
            var shootResult1St = await _shootService.GetShootResult(row, column, cacheKey);
            var shootResult2Nd = await _shootService.GetShootResult(row, column, cacheKey);
            var shootStatus    = shootResult2Nd?.Data?.ShootStatus;

            try
            {
                // Assert: Ensures the shoot status is not null
                Assert.IsNotNull(shootStatus, "Shoot status value is null on shoot result.");

                // Assert: Ensures the expected shoot status is matched with the actual status
                Assert.AreEqual(status, shootStatus, "Expected result is not matched with the act.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests the hit status functionality by simulating a shot and verifying 
        /// if the correct status 'Sunk' is returned based on the target coordinates.
        /// </summary>
        [TestMethod]
        public async Task SunkStatusTest()
        {
            // Arrange

            // cache key
            string cacheKey = "test-key";
            // gets ship data
            var result      = await _shipService.GetShipList(cacheKey);
            // extract the Battleship data
            var ship        = result?.Data?.Where(x => x.ShipType == ShipType.Battleship).ToList();
            // positions
            var positions   = ship?.SelectMany(x => x.ShipPositions)?.ToList() ?? new List<ShipPosition>();
            // shoot status
            var status      = ShootStatus.Sunk;

            // Act

            // stores new status
            ShootStatus? newStatus = null;

            // shoots until the positions are found
            foreach (var p in positions)
            {
                // retrieves the shoot result
                var shootResult = await _shootService.GetShootResult(p.Row, p.Column, cacheKey);
                newStatus       = shootResult?.Data?.ShootStatus;
            }
            
            try
            {
                // Assert: Ensures the shoot status is not null
                Assert.IsNotNull(newStatus, "Shoot status value is null on shoot result.");

                // Assert: Ensures the expected shoot status is matched with the actual status
                Assert.AreEqual(status, newStatus, "Expected result is not matched with the act.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests the hit status functionality by simulating a shot and verifying 
        /// if the correct status 'Won' is returned based on the target coordinates.
        /// </summary>
        [TestMethod]
        public async Task WonStatusTest()
        {
            // Arrange

            // cache key
            string cacheKey = "test-key";
            // gets ship data
            var result      = await _shipService.GetShipList(cacheKey);
            // all ship's positions
            var positions   = result?.Data?.SelectMany(x => x.ShipPositions)?.ToList() ?? new List<ShipPosition>();
            // shoot status
            var status      = ShootStatus.Won;

            // Act

            // stores new status
            ShootStatus? newStatus = null;

            // shoots until the positions are found
            foreach (var p in positions)
            {
                // retrieves the shoot result
                var shootResult = await _shootService.GetShootResult(p.Row, p.Column, cacheKey);
                newStatus       = shootResult?.Data?.ShootStatus;
            }

            try
            {
                // Assert: Ensures the shoot status is not null
                Assert.IsNotNull(newStatus, "Shoot status value is null on shoot result.");

                // Assert: Ensures the expected shoot status is matched with the actual status
                Assert.AreEqual(status, newStatus, "Expected result is not matched with the act.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion
                Assert.Fail(ex.Message);
            }
        }
    }
}
