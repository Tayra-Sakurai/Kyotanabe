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
        public virtual Category Category { get; set; } = null!;
    }
}
