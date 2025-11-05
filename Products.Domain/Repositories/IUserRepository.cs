using Products.Domain.Entities;

namespace Products.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
}
