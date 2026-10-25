using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pcf.Preferences.Core.Domain;

namespace Pcf.Preferences.Core.Abstractions.Repositories
{
    public interface IPreferenceRepository
    {
        Task<IReadOnlyList<Preference>> GetAllAsync();

        Task<Preference> GetByIdAsync(Guid id);

        Task<IReadOnlyList<Preference>> GetRangeByIdsAsync(IReadOnlyCollection<Guid> ids);
    }
}
