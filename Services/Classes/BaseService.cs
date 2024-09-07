using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Services.Classes
{
    /// <summary>
    /// Base service class. can use for all other services
    /// </summary>
    public class BaseService
    {
        // services
        private readonly IMemoryCache           _cache;
        private readonly ILogger<BaseService>  _logger;

        protected BaseService(IMemoryCache cache, ILogger<BaseService> logger)
        {
            _cache  = cache;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves an item from the cache based on the provided key
        /// </summary>
        /// <typeparam name="T">The type of the data to retrieve from the cache</typeparam>
        /// <param name="key">The key used to look up the cached item</param>
        /// <returns>
        /// The result will be the cached item if found; othersie, <c>null</c> if the item is not in the cache
        /// </returns>
        protected Task<T> GetCached<T>(string key) where T : class
        {
            if (_cache.TryGetValue(key, out T data))
                // return cached data if available
                return Task.FromResult(data);

            // return null (or default) if the value does not exist in the cache
            return Task.FromResult(default(T));
        }

        /// <summary>
        /// Stores data in the cache with the specified key. Cache duration is 5 minutes
        /// </summary>
        /// <typeparam name="T">The type of the data to be cached</typeparam>
        /// <param name="key">The key used to store and retrieve the cached item</param>
        /// <param name="data">The data to be stored in the cache</param>
        protected async Task Cached<T>(string key, T data) where T : class
        {
            // calls the set cache method to set the cache
            await SetCache(key, 5, data);
        }

        /// <summary>
        /// Retrieves data from the cache or fetches it using the provided 
        /// function if not already cached. Cache duration is 5 minutes
        /// </summary>
        /// <typeparam name="T">The type of data to be cached and retrieved</typeparam>
        /// <param name="key">The key used to store and retrieve the cached item</param>
        /// <param name="create">A function that retrieves the data if it is not found in the cache</param>
        /// <returns>
        /// The cached data if available; otherwise, data retrieved by the function
        /// </returns>
        protected async Task<T> Cached<T>(string key, Func<Task<T>> create) where T : class
        {
            // calls the method to get the data from the cache or fetch it if not cached
            return await LoadCached(key, 5, create);
        }

        /// <summary>
        /// Retrieves data from the cache or fetches it using the provided 
        /// function if not already cached. Cache duration is 60 minutes
        /// </summary>
        /// <typeparam name="T">The type of data to be cached and retrieved</typeparam>
        /// <param name="key">The key used to store and retrieve the cached item</param>
        /// <param name="create">A function that retrieves the data if it is not found in the cache</param>
        /// <returns>
        /// The cached data if available; otherwise, data retrieved by the function
        /// </returns>
        protected async Task<T> CachedLong<T>(string key, Func<Task<T>> create) where T : class
        {
            // calls the method to get the data from the cache or fetch it if not cached
            return await LoadCached(key.ToString(), 60, create);
        }

        /// <summary>
        /// Removes a specific cache entry based on the provided key
        /// </summary>
        /// <param name="key">The cache key for the data to remove</param>
        protected void RemoveCached(string key)
        {
            // checks the cache for the given key
            if (_cache.TryGetValue(key, out _))
            {
                _cache.Remove(key);
                _logger.LogInformation("Cache entry with key {CacheKey} has been removed.", key);
            }
        }

        /// <summary>
        /// Stores a value in the cache with a specified key and duration
        /// </summary>
        /// <typeparam name="T">The type of the value to be cached. Must be a reference type</typeparam>
        /// <param name="internalKey">The key used to store and retrieve the cached item</param>
        /// <param name="duration">The time duration (in minutes) for which the value should remain in the cache</param>
        /// <param name="value">The value to be stored in the cache</param>
        private Task SetCache<T>(string internalKey, int duration, T value) where T : class
        {
            try
            {
                // sets the cache value with an expiration time
                _cache.Set(internalKey, value, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(duration)
                });

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex, "An error occurred while setting the cached data.");
                return default;
            }
        }

        /// <summary>
        /// Loads data from the cache or retrieves it using the provided function if not cached
        /// </summary>
        /// <typeparam name="T">The type of data to be cached and retrieved</typeparam>
        /// <param name="internalKey">The key used to store and retrieve the cached data</param>
        /// <param name="duration">The duration (in minutes) for which the data should be cached</param>
        /// <param name="create">A function that retrieves the data if it is not found in the cache</param>
        /// <returns>
        /// The cached data if available; otherwise, data retrieved by the function; null on error.
        /// </returns>
        private async Task<T> LoadCached<T>(string internalKey, int duration, Func<Task<T>> create) where T : class
        {
            // attempts to get the cached data
            if (_cache.TryGetValue(internalKey, out T data))
            {
                // returns cached data if available
                return data;
            }

            try
            {
                // retrieves data from the provided function
                data = await create();
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex, "An error occurred while loading the cached data.");
                return default;
            }

            if (data is not null)
            {
                // sets the cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(duration)
                };

                // adds the data to the cache
                _cache.Set(internalKey, data, cacheEntryOptions);
            }

            // returns the retrieved data
            return data;
        }
    }
}
