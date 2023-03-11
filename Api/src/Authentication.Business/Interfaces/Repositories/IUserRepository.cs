using System.Threading.Tasks;
using Authentication.Domain.Models;

namespace Authentication.Business.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByPhoneNumber(string ddd, string phoneNumber);
    Task<User> GetByEmail(string email);
    Task<bool> IsDuplicated(string email);
}