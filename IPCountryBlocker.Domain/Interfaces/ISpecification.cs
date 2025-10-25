using System.Linq.Expressions;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> IncludeExpressions { get; }
        public List<string> IncludeStrings { get; }
        List<Tuple<Expression<Func<T, object>>, Expression<Func<object, object>>>> ThenIncludeExpressions { get; }
        public Expression<Func<T, object>> OrderBy { get; }
        public Expression<Func<T, object>> OrderByDesc { get; }
        public int Take { get; }
        public int Skip { get; }
        public bool IsPaginated { get; }
    }
}
