using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Pcf.Preferences.Core.Abstractions.Repositories;
using Pcf.Preferences.Core.Domain;
using Pcf.Preferences.DataAccess.Caching;

namespace Pcf.Preferences.DataAccess.Repositories
{
    public class CachedPreferenceRepository
        : IPreferenceRepository
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private static readonly DistributedCacheEntryOptions CacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };

        private readonly EfPreferenceRepository _repository;
        private readonly IDistributedCache _cache;

        public CachedPreferenceRepository(EfPreferenceRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Preference>> GetAllAsync()
        {
            var cached = await ReadAsync<List<Preference>>(PreferenceCacheKeys.All);
            if (cached != null)
            {
                return cached;
            }

            var preferences = (await _repository.GetAllAsync()).ToList();
            await WriteAsync(PreferenceCacheKeys.All, preferences);
            foreach (var preference in preferences)
            {
                await WriteAsync(PreferenceCacheKeys.ById(preference.Id), preference);
            }

            return preferences;
        }

        public async Task<Preference> GetByIdAsync(Guid id)
        {
            var key = PreferenceCacheKeys.ById(id);
            var cached = await ReadAsync<Preference>(key);
            if (cached != null)
            {
                return cached;
            }

            var preference = await _repository.GetByIdAsync(id);
            if (preference == null)
            {
                return null;
            }

            await WriteAsync(key, preference);
            return preference;
        }

        public async Task<IReadOnlyList<Preference>> GetRangeByIdsAsync(IReadOnlyCollection<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<Preference>();
            }

            var result = new List<Preference>();
            var missingIds = new List<Guid>();

            foreach (var id in ids.Distinct())
            {
                var cached = await ReadAsync<Preference>(PreferenceCacheKeys.ById(id));
                if (cached == null)
                {
                    missingIds.Add(id);
                    continue;
                }

                result.Add(cached);
            }

            if (missingIds.Count == 0)
            {
                return result;
            }

            var loaded = await _repository.GetRangeByIdsAsync(missingIds);
            foreach (var preference in loaded)
            {
                await WriteAsync(PreferenceCacheKeys.ById(preference.Id), preference);
                result.Add(preference);
            }

            return result;
        }

        private async Task<T> ReadAsync<T>(string key)
            where T : class
        {
            var cached = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cached))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(cached, JsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private Task WriteAsync<T>(string key, T value)
        {
            return _cache.SetStringAsync(key, JsonSerializer.Serialize(value, JsonOptions), CacheOptions);
        }
    }
}
