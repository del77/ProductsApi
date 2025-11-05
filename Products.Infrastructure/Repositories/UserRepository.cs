using Products.Application.Interfaces;
using Products.Domain.Entities;
using Products.Domain.Repositories;

namespace Products.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private static readonly List<User> Users = new()
    {
        new User { Username = "vs", Password = "rekrutacja" }
    };
    
    private static readonly IDictionary<string, User> UsersDict = Users.ToDictionary(u => u.Username.ToLower());

    public Task<User?> GetUserByUsernameAsync(string username) =>
        Task.FromResult(UsersDict.TryGetValue(username.ToLower(), out var user) ? user : null);
}
