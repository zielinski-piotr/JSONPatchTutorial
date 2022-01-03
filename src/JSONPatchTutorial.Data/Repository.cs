using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JSONPatchTutorial.Data
{
    public class Repository : IRepository
    {
        private readonly JsonPatchDbContext _context;

        public Repository(JsonPatchDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<TEntity> Get<TEntity>()
            where TEntity : class
            => _context.Set<TEntity>();

        public EntityEntry<TEntity> Update<TEntity>(TEntity entity)
            where TEntity : class
            => _context.Update(entity);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        public void Remove<TEntity>(TEntity entity)
            where TEntity : class
            => _context.Remove(entity);

        public async Task Add<TEntity>(TEntity entity)
            where TEntity : class
            => await _context.AddAsync(entity);
    }
}