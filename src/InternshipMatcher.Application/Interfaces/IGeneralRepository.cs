using InternshipMatcher.Domain.Models;
using System;
using System.Linq.Expressions;
namespace InternshipMatcher.Application.Interfaces;

public interface IGeneralRepository<T> where T : BaseModel
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<T>> GetAllByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<T>> GetPageAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task<bool> IsExistsAsync(Guid id, CancellationToken cancellationToken = default);
}