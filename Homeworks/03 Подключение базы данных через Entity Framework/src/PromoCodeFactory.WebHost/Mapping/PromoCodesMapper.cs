using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models.PromoCodes;

namespace PromoCodeFactory.WebHost.Mapping;

public static class PromoCodesMapper
{
    public static PromoCodeShortResponse ToPromoCodeShortResponse(PromoCode promoCode)
    {
        return new PromoCodeShortResponse(
            promoCode.Id,
            promoCode.Code,
            promoCode.ServiceInfo,
            promoCode.PartnerName,
            promoCode.BeginDate,
            promoCode.EndDate,
            promoCode.PartnerManager.Id,
            promoCode.Preference.Id);
    }

    public static CustomerPromoCodeResponse ToCustomerPromoCodeResponse(
        CustomerPromoCode customerPromoCode,
        PromoCode promoCode)
    {
        return new CustomerPromoCodeResponse(
            promoCode.Id,
            promoCode.Code,
            promoCode.ServiceInfo,
            promoCode.PartnerName,
            promoCode.BeginDate,
            promoCode.EndDate,
            promoCode.PartnerManager.Id,
            promoCode.Preference.Id,
            customerPromoCode.CreatedAt,
            customerPromoCode.AppliedAt);
    }

    public static PromoCode ToPromoCode(
        PromoCodeCreateRequest request,
        Employee partnerManager,
        Preference preference,
        IEnumerable<Customer> customers)
    {
        var promoCodeId = Guid.NewGuid();
        var createdAt = DateTimeOffset.UtcNow;

        return new PromoCode
        {
            Id = promoCodeId,
            Code = request.Code,
            ServiceInfo = request.ServiceInfo,
            PartnerName = request.PartnerName,
            BeginDate = request.BeginDate,
            EndDate = request.EndDate,
            PartnerManager = partnerManager,
            Preference = preference,
            CustomerPromoCodes = customers
                .Select(customer => new CustomerPromoCode
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    PromoCodeId = promoCodeId,
                    CreatedAt = createdAt
                })
                .ToList()
        };
    }
}
