using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? AcountId { get; set; } = null;
        public virtual Account? Account { get; set; } = null;
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
