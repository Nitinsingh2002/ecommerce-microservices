

namespace Authentication.Application.Abstractions.Authentication;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken token);
}