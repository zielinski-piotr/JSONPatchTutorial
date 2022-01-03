using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JSONPatchTutorial.Data
{
    public interface IRepository
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        Task SaveChangesAsync();
        void Remove<TEntity>(TEntity entity) where TEntity : class;
        Task Add<TEntity>(TEntity entity) where TEntity : class;
    }
}