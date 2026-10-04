using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pcf.Preferences.Core.Abstractions.Repositories;
using Pcf.Preferences.Core.Domain;

namespace Pcf.Preferences.DataAccess.Repositories
{
    public class EfPreferenceRepository
        : IPreferenceRepository
    {
        private readonly DataContext _dataContext;

        public EfPreferenceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IReadOnlyList<Preference>> GetAllAsync()
        {
            return await _dataContext.Preferences.ToListAsync();
        }

        public async Task<Preference> GetByIdAsync(Guid id)
        {
            return await _dataContext.Preferences.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Preference>> GetRangeByIdsAsync(IReadOnlyCollection<Guid> ids)
        {
            return await _dataContext.Preferences
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
