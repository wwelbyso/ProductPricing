using System;
using System.Collections.Generic;

namespace ProductPricing.Models
{
    public partial class SumDeduct
    {
        public SumDeduct()
        {
            Premium = new HashSet<Premium>();
        }

        public int SumDeductId { get; set; }
        public int SumInsured { get; set; }
        public int Deductible { get; set; }
        public int ProductId { get; set; }

        public virtual ICollection<Premium> Premium { get; set; }
        public virtual Product Product { get; set; }
    }
}
