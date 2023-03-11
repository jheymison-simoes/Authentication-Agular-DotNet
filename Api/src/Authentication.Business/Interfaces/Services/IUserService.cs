using System.Threading.Tasks;
using Authentication.Business.Models.User.Request;
using Authentication.Business.Models.User.Response;

namespace Authentication.Business.Interfaces.Services;

public interface IUserService
{
    Task<UserResponse> CreateUser(CreateUserRequest request);
}