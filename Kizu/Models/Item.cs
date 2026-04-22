using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string? ItemName { get; set; }
        public int CategoryId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public double Expense { get; set; } = 0;
        public double Income { get; set; } = 0;
        public int PaymentMethodId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;
        public float[] Vector { get; set; } = new float[768];
    }
}
