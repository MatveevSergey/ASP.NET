using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.Integration.Dto;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class PreferencesGateway
        : IPreferencesGateway
    {
        private readonly HttpClient _httpClient;

        public PreferencesGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<Preference>> GetAllAsync()
        {
            var preferences = await _httpClient.GetFromJsonAsync<List<PreferenceDto>>("api/v1/Preferences");

            return Map(preferences);
        }

        public async Task<Preference> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/v1/Preferences/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            var preference = await response.Content.ReadFromJsonAsync<PreferenceDto>();

            return Map(preference);
        }

        private static IReadOnlyList<Preference> Map(IEnumerable<PreferenceDto> preferences)
        {
            if (preferences == null)
            {
                return new List<Preference>();
            }

            return preferences.Where(x => x != null).Select(Map).ToList();
        }

        private static Preference Map(PreferenceDto preference)
        {
            if (preference == null)
            {
                return null;
            }

            return new Preference
            {
                Id = preference.Id,
                Name = preference.Name
            };
        }
    }
}
