using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models.Customers;
using PromoCodeFactory.WebHost.Models.PromoCodes;

namespace PromoCodeFactory.WebHost.Mapping;

public static class CustomersMapper
{
    public static CustomerShortResponse ToCustomerShortResponse(Customer customer)
    {
        return new CustomerShortResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.Preferences.Select(PreferencesMapper.ToPreferenceShortResponse).ToList());
    }

    public static CustomerResponse ToCustomerResponse(
        Customer customer,
        IReadOnlyDictionary<Guid, PromoCode> promoCodesById)
    {
        var promoCodes = customer.CustomerPromoCodes
            .Where(cpc => promoCodesById.ContainsKey(cpc.PromoCodeId))
            .Select(cpc => PromoCodesMapper.ToCustomerPromoCodeResponse(cpc, promoCodesById[cpc.PromoCodeId]))
            .ToList();

        return new CustomerResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.Preferences.Select(PreferencesMapper.ToPreferenceShortResponse).ToList(),
            promoCodes);
    }

    public static Customer ToCustomer(CustomerCreateRequest request, IReadOnlyCollection<Preference> preferences)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Preferences = preferences.ToList()
        };
    }
}
