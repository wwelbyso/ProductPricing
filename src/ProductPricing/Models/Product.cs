using System;
using System.Collections.Generic;

namespace ProductPricing.Models
{
    public partial class Product
    {
        public Product()
        {
            SumDeduct = new HashSet<SumDeduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PlanName { get; set; }
        public string ProductType { get; set; }
        public string Condition { get; set; }
        public int FamilyCompositionId { get; set; }

        public virtual ICollection<SumDeduct> SumDeduct { get; set; }
        public virtual FamilyComposition FamilyComposition { get; set; }
    }
}
