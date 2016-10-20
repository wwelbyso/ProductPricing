using System;
using System.Collections.Generic;

namespace ProductPricing.Models
{
    public partial class FamilyComposition
    {
        public FamilyComposition()
        {
            Product = new HashSet<Product>();
        }

        public int FamilyCompositionId { get; set; }
        public string Text { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
