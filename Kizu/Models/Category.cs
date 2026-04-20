using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float[] Vector { get; set; } = new float[768];
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
