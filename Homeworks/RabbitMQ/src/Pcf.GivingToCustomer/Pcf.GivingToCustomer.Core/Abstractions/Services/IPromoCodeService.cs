using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Models;

namespace Pcf.GivingToCustomer.Core.Abstractions.Services
{
    public interface IPromoCodeService
    {
        Task<bool> GiveToCustomersWithPreferenceAsync(GivePromoCodeCommand command);
    }
}
