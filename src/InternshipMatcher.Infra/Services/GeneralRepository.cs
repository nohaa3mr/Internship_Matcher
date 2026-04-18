using InternshipMatcher.Application.DTO;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Domain.Models;
using InternshipMatcher.Infra.DbContext;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InternshipMatcher.Infra.Services;

public class GeneralRepository<T>(AppDbContext context) : IGeneralRepository<T> where T : BaseModel, new()
{
    private readonly AppDbContext _context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken);
        if (entity is null) return;

        entity.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet
            .Where(e => !e.IsDeleted)
            .ToListAsync(cancellationToken);

        return entities.AsQueryable();
    }

    public async Task<List<T>> GetAllByPredicateAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .FirstOrDefaultAsync(e => e.ID == id, cancellationToken);
    }

    public async Task<T?> GetByPredicateAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .CountAsync(cancellationToken);
    }

    public async Task<List<T>> GetPageAsync(
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(e => e.ID == id && !e.IsDeleted, cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var existing = await _dbSet
            .FirstOrDefaultAsync(e => e.ID == entity.ID && !e.IsDeleted, cancellationToken);

        if (existing is null)
            throw new InvalidOperationException($"{typeof(T).Name} with ID {entity.ID} not found.");

        entity.Adapt(existing);
        await _context.SaveChangesAsync(cancellationToken);
    }
}