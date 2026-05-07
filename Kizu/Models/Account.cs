using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<PaymentMethod> PaymentMethods { get; set; } = new HashSet<PaymentMethod>();

        public double Invoice()
        {
            double total = 0;

            foreach (PaymentMethod m in PaymentMethods)
                total +=
                    m
                    .Items
                    .Where(i => i.DateTime.Month == DateTime.Now.Month)
                    .Sum(i => i.Income);

            return total;
        }
    }
}
