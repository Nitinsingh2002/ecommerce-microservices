
using Authentication.Application.Abstractions.Authentication;
using Authentication.Domain.Identity;
using Authentication.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    public RefreshTokenRepository(ApplicationDbContext _context)
    {
        this._context = _context;
    }

    //  to save a new refresh token 
    // after login we will genrate and save the refresh token in the DB
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _context.RefreshToken.AddAsync(refreshToken, cancellationToken);
        //await _context.SaveChangesAsync(cancellationToken);
    }

    //to find the refresh token 
    //when access token expire we will use find refresh token and genrate new access and refresh token
    public async Task<RefreshToken?> GetByTokenAsync (string token, CancellationToken cancellationToken)
    {
        var result = await _context.RefreshToken.Include(x =>x.User).FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        return result;
    }

    // to update the old refresh token when refresh token is invalid
    public Task UpdateAsync (RefreshToken refreshToken, CancellationToken cancellationToken)
    {
       _context.RefreshToken.Update(refreshToken);
        return  Task.CompletedTask;
       //await _context.SaveChangesAsync(cancellationToken);

    }


}
