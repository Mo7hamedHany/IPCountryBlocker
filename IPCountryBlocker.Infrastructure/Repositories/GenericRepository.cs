using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Infrastructure.Data.Context;
using IPCountryBlocker.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IPCountryBlocker.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : ModelKey<TKey>
    {
        private readonly IPCountryBlockerDataContext _context;

        public GenericRepository(IPCountryBlockerDataContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);

        public IQueryable<TEntity> AsNoTracking()
            => _context.Set<TEntity>().AsNoTracking();

        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void Detach(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();


        public async Task<IEnumerable<TEntity>> GetAllWithSpecsAsync(ISpecification<TEntity> specification, bool asNoTracking = false)
        {
            var query = ApplySpecifications(specification);

            if (asNoTracking)
                query = query.AsNoTracking(); // Apply AsNoTracking if needed

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(TKey id) => (await _context.Set<TEntity>().FindAsync(id))!;

        public async Task<int> GetCountWithSpecsAsync(ISpecification<TEntity> specification) => await ApplySpecifications(specification).CountAsync();


        public async Task<TEntity> GetWithSpecsAsync(ISpecification<TEntity> specification) => (await ApplySpecifications(specification).FirstOrDefaultAsync())!;

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> CreateBulkAsync(IEnumerable<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
            return entities;
        }


        private IQueryable<TEntity> ApplySpecifications(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity, TKey>.BuildQuery(_context.Set<TEntity>(), specification);
        }


        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int skip = 0, int take = int.MaxValue)
        {
            return await _context.Set<TEntity>()
                                 .Where(predicate)
                                 .Skip(skip)
                                 .Take(take)
                                 .ToListAsync();
        }

        public void RemoveBulk(IEnumerable<TEntity> entities)
        {
            _context.RemoveRange(entities);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

    }
}
