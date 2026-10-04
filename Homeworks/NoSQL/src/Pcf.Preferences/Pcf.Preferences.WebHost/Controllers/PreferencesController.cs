using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pcf.Preferences.Core.Abstractions.Repositories;
using Pcf.Preferences.WebHost.Models;

namespace Pcf.Preferences.WebHost.Controllers
{
    /// <summary>
    /// Справочник предпочтений
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController
        : ControllerBase
    {
        private readonly IPreferenceRepository _preferencesRepository;

        public PreferencesController(IPreferenceRepository preferencesRepository)
        {
            _preferencesRepository = preferencesRepository;
        }

        /// <summary>
        /// Получить список предпочтений
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
        {
            var preferences = await _preferencesRepository.GetAllAsync();

            return Ok(preferences.Select(Map).ToList());
        }

        /// <summary>
        /// Получить предпочтение по id
        /// </summary>
        /// <param name="id">Id предпочтения, например <example>ef7f299f-92d7-459f-896e-078ed53ef99c</example></param>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceAsync(Guid id)
        {
            var preference = await _preferencesRepository.GetByIdAsync(id);
            if (preference == null)
            {
                return NotFound();
            }

            return Ok(Map(preference));
        }

        /// <summary>
        /// Получить предпочтения по списку id
        /// </summary>
        [HttpPost("by-ids")]
        public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesByIdsAsync(PreferenceIdsRequest request)
        {
            if (request?.Ids == null)
            {
                return BadRequest();
            }

            var preferences = await _preferencesRepository.GetRangeByIdsAsync(request.Ids);

            return Ok(preferences.Select(Map).ToList());
        }

        private static PreferenceResponse Map(Core.Domain.Preference preference)
        {
            return new PreferenceResponse
            {
                Id = preference.Id,
                Name = preference.Name
            };
        }
    }
}
