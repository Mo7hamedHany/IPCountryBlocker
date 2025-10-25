using IPCountryBlocker.Domain.Interfaces;
using System.Linq.Expressions;

namespace IPCountryBlocker.Infrastructure.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> IncludeExpressions { get; } = new();

        public List<Tuple<Expression<Func<T, object>>, Expression<Func<object, object>>>> ThenIncludeExpressions { get; } = new();

        public List<string> IncludeStrings { get; } = new();

        public Expression<Func<T, object>> OrderBy { get; protected set; }

        public Expression<Func<T, object>> OrderByDesc { get; protected set; }

        public int Take { get; protected set; }

        public int Skip { get; protected set; }

        public bool IsPaginated { get; protected set; }

        public void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }

        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }


        public void AddThenInclude(Expression<Func<T, object>> includeExpression, Expression<Func<object, object>> thenIncludeExpression)
        {
            ThenIncludeExpressions.Add(Tuple.Create(includeExpression, thenIncludeExpression));
        }


    }
}
