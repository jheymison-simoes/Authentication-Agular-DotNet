namespace Authentication.Business.Interfaces.Services;

public interface IEncryptService
{
    string EncryptPassword(string password);
}