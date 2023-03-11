using System;
using Authentication.Domain.Models;

namespace Authentication.Business.Interfaces.Services;

public interface ITokenService
{
    (string token, DateTime expireDate) GenerateToken(User user);
}