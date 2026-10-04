using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Abstractions.Gateways;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.IntegrationTests.Data;

namespace Pcf.GivingToCustomer.IntegrationTests.Fakes
{
    public class FakePreferencesGateway
        : IPreferencesGateway
    {
        private readonly List<Preference> _preferences = TestDataFactory.Preferences;

        public Task<IReadOnlyList<Preference>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Preference>>(_preferences);
        }

        public Task<Preference> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_preferences.FirstOrDefault(x => x.Id == id));
        }

        public Task<IReadOnlyList<Preference>> GetRangeByIdsAsync(IReadOnlyCollection<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Task.FromResult<IReadOnlyList<Preference>>(new List<Preference>());
            }

            IReadOnlyList<Preference> preferences = _preferences.Where(x => ids.Contains(x.Id)).ToList();

            return Task.FromResult(preferences);
        }
    }
}
