using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.Core.Abstractions.Gateways
{
    public interface IPreferencesGateway
    {
        Task<IReadOnlyList<Preference>> GetAllAsync();

        Task<Preference> GetByIdAsync(Guid id);

        Task<IReadOnlyList<Preference>> GetRangeByIdsAsync(IReadOnlyCollection<Guid> ids);
    }
}
