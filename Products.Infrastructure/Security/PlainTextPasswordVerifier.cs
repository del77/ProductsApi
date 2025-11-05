using Products.Application.Interfaces;

namespace Products.Infrastructure.Security;

public class PlainTextPasswordVerifier : IPasswordVerifier
{
    public bool Verify(string providedPassword, string storedPassword)
    {
        // in real world scenario, stored password would be hashed so we would have to hash provided password too.
        return providedPassword == storedPassword;
    }
}
