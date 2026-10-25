using System.Threading.Tasks;
using MassTransit;
using Pcf.Administration.Core.Abstractions.Services;
using Pcf.Contracts;

namespace Pcf.Administration.WebHost.Consumers
{
    public class PromoCodeReceivedConsumer : IConsumer<PromoCodeReceivedEvent>
    {
        private readonly IEmployeeService _employeeService;

        public PromoCodeReceivedConsumer(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task Consume(ConsumeContext<PromoCodeReceivedEvent> context)
        {
            if (!context.Message.PartnerManagerId.HasValue)
                return;

            await _employeeService.UpdateAppliedPromocodesAsync(context.Message.PartnerManagerId.Value);
        }
    }
}
