using System.Threading.Tasks;
using MassTransit;
using Pcf.Contracts;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.GivingToCustomer.Core.Models;

namespace Pcf.GivingToCustomer.WebHost.Consumers
{
    public class PromoCodeReceivedConsumer : IConsumer<PromoCodeReceivedEvent>
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeReceivedConsumer(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        public async Task Consume(ConsumeContext<PromoCodeReceivedEvent> context)
        {
            var message = context.Message;

            await _promoCodeService.GiveToCustomersWithPreferenceAsync(new GivePromoCodeCommand
            {
                PromoCodeId = message.PromoCodeId,
                ServiceInfo = message.ServiceInfo,
                PartnerId = message.PartnerId,
                Code = message.Code,
                PreferenceId = message.PreferenceId,
                BeginDate = message.BeginDate,
                EndDate = message.EndDate
            });
        }
    }
}
