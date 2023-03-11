using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Authentication.Business.Interfaces.Repositories;
using Authentication.Domain.Models;

namespace Authentication.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(SqlContext db) : base(db)
    {
    }

    public async Task<User> GetByPhoneNumber(string ddd, string phoneNumber)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Ddd == ddd && u.PhoneNumber == phoneNumber);
    }
    
    public async Task<User> GetByEmail(string email)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<bool> IsDuplicated(string email)
    {
        return await DbSet
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);
    }
}