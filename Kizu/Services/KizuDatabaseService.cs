using Kizu.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Services
{
    public class KizuDatabaseService : IDatabaseService<KizuContext>
    {
        private readonly IDbContextFactory<KizuContext> factory;

        public KizuDatabaseService(IDbContextFactory<KizuContext> factory)
        {
            this.factory = factory;
        }

        public async Task<IEnumerable<T>> GetEntitiesAsync<T>(Func<KizuContext, DbSet<T>> selector)
            where T : class
        {
            using KizuContext context = await factory.CreateDbContextAsync();

            return await selector(context).ToListAsync();
        }

        public List<T> GetEntities<T>()
            where T : class
        {
            using KizuContext context = factory.CreateDbContext();

            return context.Set<T>().ToList();
        }

        public List<T> GetEntities<T>(Expression<Func<KizuContext, DbSet<T>>> selector)
            where T : class
        {
            using KizuContext context = factory.CreateDbContext();

            return selector.Compile()(context).ToList();
        }

        public async Task RemoveAsync<T>(T entity)
            where T : class
        {
            using KizuContext context = await factory.CreateDbContextAsync();

            EntityEntry<T> entry = context.Attach(entity);
            entry.State = EntityState.Deleted;

            await context.SaveChangesAsync();
        }

        public async Task AddAsync<T>(T entity)
            where T : class
        {
            using KizuContext context = await factory.CreateDbContextAsync();

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddAsync<T>(IEnumerable<T> entities)
            where T : class
        {
            using KizuContext context = await factory.CreateDbContextAsync();

            foreach (T entity in entities)
                await context.AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity)
            where T : class
        {
            using KizuContext context = await factory.CreateDbContextAsync();

            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public bool Exists<T>(T entity)
            where T : class
        {
            using KizuContext context = factory.CreateDbContext();

            EntityEntry<T> entry = context.Attach(entity);

            return entry.State == EntityState.Unchanged;
        }
    }
}
