using Kizu.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Factory
{
    public class FakeKizuContextFactory : IDesignTimeDbContextFactory<KizuContext>
    {
        public KizuContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<KizuContext> optionsBuilder = new();
            optionsBuilder.UseSqlite("Data Source=TestDb.db");

            return new(optionsBuilder.Options);
        }
    }
}
