using IPCountryBlocker.Domain.Models;
using System.Linq.Expressions;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : ModelKey<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> CreateBulkAsync(IEnumerable<TEntity> entities);

        void RemoveBulk(IEnumerable<TEntity> entities);

        Task<IEnumerable<TEntity>> GetAllWithSpecsAsync(ISpecification<TEntity> specification, bool asNoTracking = false);

        Task<int> GetCountWithSpecsAsync(ISpecification<TEntity> specification);

        Task<TEntity> GetAsync(TKey id);

        Task<TEntity> GetWithSpecsAsync(ISpecification<TEntity> specification);

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        void Detach(TEntity entity); // Add this method
        IQueryable<TEntity> AsNoTracking();

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int skip = 0, int take = int.MaxValue);

        IQueryable<TEntity> AsQueryable();

    }
}
