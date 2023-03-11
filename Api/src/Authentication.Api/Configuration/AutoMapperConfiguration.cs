using AutoMapper;
using Authentication.Business.Models.User.Request;
using Authentication.Business.Models.User.Response;
using Authentication.Domain.Models;

namespace Authentication.Api.Configuration;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<User, UserAuthenticatedResponse>().ReverseMap();
        CreateMap<CreateUserRequest, User>().ReverseMap();
        CreateMap<User, UserResponse>().ReverseMap();
    }
    
}