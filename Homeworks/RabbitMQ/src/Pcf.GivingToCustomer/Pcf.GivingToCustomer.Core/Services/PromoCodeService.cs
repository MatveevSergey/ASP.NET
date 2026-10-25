using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Models;

namespace Pcf.GivingToCustomer.Core.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<Customer> _customersRepository;

        public PromoCodeService(
            IRepository<PromoCode> promoCodesRepository,
            IRepository<Preference> preferencesRepository,
            IRepository<Customer> customersRepository)
        {
            _promoCodesRepository = promoCodesRepository;
            _preferencesRepository = preferencesRepository;
            _customersRepository = customersRepository;
        }

        public async Task<bool> GiveToCustomersWithPreferenceAsync(GivePromoCodeCommand command)
        {
            var preference = await _preferencesRepository.GetByIdAsync(command.PreferenceId);

            if (preference == null)
                return false;

            var customers = await _customersRepository
                .GetWhere(d => d.Preferences.Any(x =>
                    x.Preference.Id == preference.Id));

            var promoCode = new PromoCode
            {
                Id = command.PromoCodeId == Guid.Empty
                    ? Guid.NewGuid()
                    : command.PromoCodeId,
                PartnerId = command.PartnerId,
                Code = command.Code,
                ServiceInfo = command.ServiceInfo,
                BeginDate = command.BeginDate,
                EndDate = command.EndDate,
                Preference = preference,
                PreferenceId = preference.Id,
                Customers = new List<PromoCodeCustomer>()
            };

            foreach (var customer in customers)
            {
                promoCode.Customers.Add(new PromoCodeCustomer
                {
                    CustomerId = customer.Id,
                    Customer = customer,
                    PromoCodeId = promoCode.Id,
                    PromoCode = promoCode
                });
            }

            await _promoCodesRepository.AddAsync(promoCode);

            return true;
        }
    }
}
