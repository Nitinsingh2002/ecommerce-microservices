

using Authentication.Infrastructure.Persistence.Context;
using  Authentication.Application.Abstractions.Authentication;

namespace Authentication.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext _context)
    {
        this._context = _context;
    }
    public async Task<int> SaveChangesAsync(CancellationToken token)
    {
        return await _context.SaveChangesAsync(token);
    }
}