using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Authentication.Business.Exceptions;
using Authentication.Business.Interfaces.Services;
using Authentication.Business.Models;
using Authentication.Business.Models.User.Request;
using Authentication.Business.Models.User.Response;
using Microsoft.Extensions.Logging;

namespace Authentication.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : BaseController<UserController>
{
    private readonly IUserService _userService;
    
    public UserController(
        ILogger<UserController> logger, 
        IMapper mapper,
        IUserService userService) : base(logger, mapper)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<ActionResult<BaseResponse<UserResponse>>> Create(CreateUserRequest request)
    {
        try
        {
            var result = await _userService.CreateUser(request);
            return BaseResponseSuccess(result);
        }
        catch (CustomException cEx)
        {
            return BaseResponseError<UserResponse>(cEx.Message);
        }
        catch (Exception ex)
        {
            return BaseResponseInternalError<UserResponse>(ex.Message);
        }
    }
}