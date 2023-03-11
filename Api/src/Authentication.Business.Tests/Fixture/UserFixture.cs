using System.Collections.Generic;
using Bogus;
using Authentication.Domain.Models;
using Xunit;

namespace Authentication.Business.Tests.Fixture;

[CollectionDefinition(nameof(UserCollection))]
public class UserCollection : ICollectionFixture<UserFixture>
{
}

public class UserFixture
{
    private const string CultureFaker = "pt_BR";
    
    public List<User> GenerateValidUsers(int quantidade)
    {
        return new Faker<User>(CultureFaker)
            .CustomInstantiator(f =>
            {
                var firstName = new Faker().Name.FirstName();
                var lastName = new Faker().Name.LastName();
                var fullName = $"{firstName} {lastName}";
                
                return new User(
                    fullName,
                    f.Random.Replace("##"),
                    f.Random.Replace("#########"),
                    f.Internet.Email(firstName, lastName),
                    f.Random.String2(8)
                );
            }).Generate(quantidade);
    }
}