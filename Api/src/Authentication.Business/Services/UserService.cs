using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Authentication.Business.Exceptions;
using Authentication.Business.Interfaces.Repositories;
using Authentication.Business.Interfaces.Services;
using Authentication.Business.Models;
using Authentication.Business.Models.User.Request;
using Authentication.Business.Models.User.Response;
using Authentication.Domain.Models;

namespace Authentication.Business.Services;

public class UserService : BaseService, IUserService
{
    #region Repositories
    private readonly IUserRepository _userRepository;
    #endregion

    #region Services
    private readonly IEncryptService _encryptService;
    #endregion

    #region Validators
    private readonly CreateUserRequestValidator _createUserRequestValidator;
    private readonly UserValidator _userValidator;
    #endregion
    
    public UserService(
        IMapper mapper, 
        IOptions<AppSettings> appSettings, 
        ResourceManager resourceManager, 
        CultureInfo cultureInfo,
        IUserRepository userRepository,
        IEncryptService encryptService,
        CreateUserRequestValidator createUserRequestValidator, 
        UserValidator userValidator) : base(mapper, appSettings, resourceManager, cultureInfo)
    {
        _userRepository = userRepository;
        _createUserRequestValidator = createUserRequestValidator;
        _userValidator = userValidator;
        _encryptService = encryptService;
    }

    public async Task<UserResponse> CreateUser(CreateUserRequest request)
    {
        var requestValidator = await _createUserRequestValidator.ValidateModel(request);
        if (requestValidator.error) ReturnError<CustomException>(requestValidator.messageError);

        var userDuplicated = await _userRepository.IsDuplicated(request.Email);
        if (userDuplicated) ReturnResourceError<CustomException>("USER-EXISTING_USER");
        
        var user = Mapper.Map<User>(request);
        user.Password = _encryptService.EncryptPassword(user.Password);

        var userValidator = await _userValidator.ValidateModel(user);
        if (userValidator.error) ReturnError<CustomException>(userValidator.messageError);
        
        _userRepository.Add(user);
        await _userRepository.SaveChanges();
        
        return Mapper.Map<UserResponse>(user);
    }
}