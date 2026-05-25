using System.Collections.Generic;
using System.Linq;

namespace ElectronicsStore.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total => Items.Sum(i => i.Subtotal);
        public int Count => Items.Sum(i => i.Quantity);
    }
}
