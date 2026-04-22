using Kizu.Converters;
using Kizu.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Contexts
{
    public class KizuContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public KizuContext(DbContextOptions<KizuContext> options) : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<float[]>()
                .HaveConversion<VectorConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Account>()
                .HasIndex(e => e.Name)
                .IsUnique();
            modelBuilder
                .Entity<Category>()
                .HasIndex(e => e.Name)
                .IsUnique();
            modelBuilder
                .Entity<Item>()
                .ToTable(b => b.HasCheckConstraint("CK_Balance", "([Expense] > 0 AND [Income] = 0) OR ([Expense] = 0 AND [Income] > 0)"));
            modelBuilder
                .Entity<PaymentMethod>()
                .HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
