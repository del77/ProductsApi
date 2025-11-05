namespace Products.Application.Interfaces;

public interface IPasswordVerifier
{
    bool Verify(string providedPassword, string storedPassword);
}
