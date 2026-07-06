using E_Commerce.Domain.Common;
using E_Commerce.Domain.Specifications;
using System.Linq.Expressions;

namespace E_Commerce.Application.Specifications;

public class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; } = [];

    public Expression<Func<TEntity, bool>> Criteria { get; private set; }

    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPaginated { get; private set; }

    public BaseSpecification(Expression<Func<TEntity, bool>> expression)
    {
        Criteria = expression;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> expression)
    {
        IncludeExpressions.Add(expression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> expression)
    {
        OrderBy = expression;
    }
    protected void AddOrderByDesc(Expression<Func<TEntity, object>> expression)
    {
        OrderByDesc = expression;
    }

    protected void ApplyPagination(int pageIndex, int pageSize)
    {
        Take = pageSize;
        Skip = (pageIndex - 1) * pageSize;
        IsPaginated = true;
    }



}
