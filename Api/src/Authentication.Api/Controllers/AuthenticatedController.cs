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
public class AuthenticatedController : BaseController<AuthenticatedController>
{
    private readonly IAuthenticatedService _authenticatedService;
    
    public AuthenticatedController(
        ILogger<AuthenticatedController> logger, 
        IMapper mapper, 
        IAuthenticatedService authenticatedService) : base(logger, mapper)
    {
        _authenticatedService = authenticatedService;
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<BaseResponse<UserSessionResponse>>> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var token = await _authenticatedService.UserAuthenticated(loginRequest);
            return BaseResponseSuccess(token);
        }
        catch (CustomException cEx)
        {
            return BaseResponseError<UserSessionResponse>(cEx.Message);
        }
        catch (Exception ex)
        {
            return BaseResponseInternalError<UserSessionResponse>(ex.Message);
        }
    }
    
    [HttpPost("LoginWithGoogle")]
    public async Task<ActionResult<BaseResponse<UserSessionResponse>>> LoginWithGoogle([FromBody] string googleCredential)
    {
        try
        {
            var token = await _authenticatedService.UserAuthenticated(googleCredential);
            return BaseResponseSuccess(token);
        }
        catch (CustomException cEx)
        {
            return BaseResponseError<UserSessionResponse>(cEx.Message);
        }
        catch (Exception ex)
        {
            return BaseResponseInternalError<UserSessionResponse>(ex.Message);
        }
    }
}