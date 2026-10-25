using Microsoft.Extensions.Caching.Distributed;
using Pcf.Preferences.DataAccess.Caching;

namespace Pcf.Preferences.DataAccess.Data
{
    public class EfDbInitializer
        : IDbInitializer
    {
        private readonly DataContext _dataContext;
        private readonly IDistributedCache _cache;

        public EfDbInitializer(DataContext dataContext, IDistributedCache cache)
        {
            _dataContext = dataContext;
            _cache = cache;
        }

        public void InitializeDb()
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();

            _dataContext.AddRange(FakeDataFactory.Preferences);
            _dataContext.SaveChanges();

            _cache.Remove(PreferenceCacheKeys.All);
            foreach (var preference in FakeDataFactory.Preferences)
            {
                _cache.Remove(PreferenceCacheKeys.ById(preference.Id));
            }
        }
    }
}
