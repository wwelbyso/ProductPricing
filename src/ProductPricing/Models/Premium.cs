using System;
using System.Collections.Generic;

namespace ProductPricing.Models
{
    public partial class Premium
    {
        public int PremiumId { get; set; }
        public short Age { get; set; }
        public decimal Premium1 { get; set; }
        public int SumDeductId { get; set; }

        public virtual SumDeduct SumDeduct { get; set; }
    }
}
