using System.Globalization;
using System.Resources;
using AutoMapper;
using Microsoft.Extensions.Options;
using Authentication.Business.Exceptions;
using Authentication.Business.Interfaces.Repositories;
using Authentication.Business.Interfaces.Services;
using Authentication.Business.Models;
using Authentication.Business.Models.User.Request;
using Authentication.Business.Models.User.Response;
using Google.Apis.Auth;

namespace Authentication.Business.Services;

public class AuthenticatedService : BaseService, IAuthenticatedService
{
    #region Repositories
    private readonly IUserRepository _userRepository;
    #endregion

    #region Services
    private readonly ITokenService _tokenService;
    private readonly IEncryptService _encryptService;
    #endregion
    
    #region Validators
    private readonly LoginRequestValidator _loginRequestValidator;
    #endregion
    
    public AuthenticatedService(
        IMapper mapper, 
        IOptions<AppSettings> appSettings, 
        ResourceManager resourceManager, 
        CultureInfo cultureInfo,
        IUserRepository userRepository,
        ITokenService tokenService,
        IEncryptService encryptService,
        LoginRequestValidator loginRequestValidator) : base(mapper, appSettings, resourceManager, cultureInfo)
    {
        _loginRequestValidator = loginRequestValidator;
        _encryptService = encryptService;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<UserSessionResponse> UserAuthenticated(LoginRequest request)
    {
        var requestValidator = await _loginRequestValidator.ValidateModel(request);
        if (requestValidator.error) ReturnError<CustomException>(requestValidator.messageError);

        var user = await _userRepository.GetByEmail(request.Email);
        if (user is null) ReturnResourceError<CustomException>("USER-INVALID_LOGIN");

        var passwordEncrypt = _encryptService.EncryptPassword(request.Password);
        if (passwordEncrypt != user!.Password) ReturnResourceError<CustomException>("USER-INVALID_LOGIN");

        var tokenGenerated = _tokenService.GenerateToken(user);
        
        return new UserSessionResponse()
        {
            Token = tokenGenerated.token,
            ExpireIn = (int)tokenGenerated.expireDate.Subtract(DateTime.UtcNow).TotalSeconds,
            UserName = user.Name,
            ExpireTymeSpan = tokenGenerated.expireDate
        };
    }

    public async Task<UserSessionResponse> UserAuthenticated(string googleCredential)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { AppSettings.GoogleClientId }
        };

        var userData = await GoogleJsonWebSignature.ValidateAsync(googleCredential, settings);
        
        var user = await _userRepository.GetByEmail(userData.Email);
        if (user is null) ReturnResourceError<CustomException>("LOGIN-INVALID-WITH_LOGIN");
        
        var tokenGenerated = _tokenService.GenerateToken(user);
        
        return new UserSessionResponse()
        {
            Token = tokenGenerated.token,
            ExpireIn = (int)tokenGenerated.expireDate.Subtract(DateTime.UtcNow).TotalSeconds,
            UserName = user!.Name,
            ExpireTymeSpan = tokenGenerated.expireDate
        };
    }
}