using System.Globalization;
using System.Resources;
using AutoMapper;
using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using Authentication.Api.Configuration;
using Authentication.Business.Interfaces.Repositories;
using Authentication.Business.Interfaces.Services;
using Authentication.Business.Models;
using Authentication.Business.Models.User.Request;
using Authentication.Business.Services;
using Authentication.Business.Tests.Utils;
using Authentication.Business.Utils;
using Xunit;

namespace Authentication.Business.Tests.Fixture;

[CollectionDefinition(nameof(AuthenticatedCollection))]
public class AuthenticatedCollection : ICollectionFixture<AuthenticatedFixture>
{
}

public class AuthenticatedFixture
{
    public AuthenticatedService AuthenticatedService;
    public ResourceManager ResourceManager;
    public IMapper Mapper;
    public Mock<IOptions<AppSettings>> AppSettings;

    #region Repositories
    public Mock<IUserRepository> UserRepository;
    #endregion
    
    #region Services
    public Mock<ITokenService> TokenService;
    public Mock<IEncryptService> EncryptService;
    #endregion

    #region Validators
    public LoginRequestValidator LoginRequestValidator;
    #endregion

    private const string CultureFaker = "pt_BR";
    private const string Culture = "pt-BR";

    public void GenerateService()
    {
        ResourceManager = new ResourceManager(typeof(Api.Resource.ApiResource));
        Mapper = MapperTests.Mapping<AutoMapperConfiguration>();
        var culture = CultureInfo.GetCultureInfo(Culture);
        AppSettings = new Mock<IOptions<AppSettings>>();
        UserRepository = new Mock<IUserRepository>();
        TokenService = new Mock<ITokenService>();
        EncryptService = new Mock<IEncryptService>();
        LoginRequestValidator = new LoginRequestValidator(ResourceManager, culture);

        AuthenticatedService = new AuthenticatedService(
            Mapper,
            AppSettings.Object,
            ResourceManager,
            culture,
            UserRepository.Object,
            TokenService.Object,
            EncryptService.Object,
            LoginRequestValidator
        );
    }

    public LoginRequest GenerateValidLoginRequest()
    {
        return new Faker<LoginRequest>(CultureFaker)
            .CustomInstantiator(f => new LoginRequest()
            {
                Email = f.Internet.Email(),
                Password = f.Random.String2(10)
            });
    }
    
    public string GetMessageResource(string name, params object[] parameters)
    {
        return parameters.Length > 0
            ? ResourceManager.GetString(name, CultureInfo.GetCultureInfo(Culture))!.ResourceFormat(parameters)
            : ResourceManager.GetString(name);
    }
}