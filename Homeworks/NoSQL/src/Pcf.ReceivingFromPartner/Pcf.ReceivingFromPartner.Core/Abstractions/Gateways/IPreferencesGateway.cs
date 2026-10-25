using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Gateways
{
    public interface IPreferencesGateway
    {
        Task<IReadOnlyList<Preference>> GetAllAsync();

        Task<Preference> GetByIdAsync(Guid id);
    }
}
