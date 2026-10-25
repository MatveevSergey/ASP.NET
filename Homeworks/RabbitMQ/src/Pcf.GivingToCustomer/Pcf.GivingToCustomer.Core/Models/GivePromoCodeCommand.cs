using System;

namespace Pcf.GivingToCustomer.Core.Models
{
    public class GivePromoCodeCommand
    {
        public Guid PromoCodeId { get; set; }

        public string ServiceInfo { get; set; }

        public Guid PartnerId { get; set; }

        public string Code { get; set; }

        public Guid PreferenceId { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
