using System.Text;
using Products.Application.Interfaces;
using Products.Domain.Repositories;

namespace Products.Middleware;

public class BasicAuthMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IUserRepository userRepository, IPasswordVerifier passwordVerifier)
    {
        var credentials = ExtractCredentialsFromAuthHeader(context);

        if (credentials is null)
        {
            context.Response.StatusCode = 401;
            
            return;
        }

        
        var user = await userRepository.GetUserByUsernameAsync(credentials.Value.UserName);

        if (user is null || !passwordVerifier.Verify(credentials.Value.Password, user.Password))
        {
            context.Response.StatusCode = 401;
            
            return;
        }
        
        await _next(context);
    }

    private static (string UserName, string Password)? ExtractCredentialsFromAuthHeader(HttpContext context)
    {
        var authHeaderExists = context.Request.Headers.TryGetValue("Authorization", out var authHeader);
        if (!authHeaderExists)
        {
            return null;
        }

        var authHeaderValue = authHeader.ToString();
        const string basicAuthPrefix = "Basic ";
        
        if (!authHeaderValue.StartsWith(basicAuthPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        
        var encodedCredentials = authHeaderValue.Substring(basicAuthPrefix.Length).Trim();
        
        var credentialsBytes = Convert.FromBase64String(encodedCredentials);
        var decodedCredentials = Encoding.UTF8.GetString(credentialsBytes);
        
        var credentials = decodedCredentials.Split(':', 2);
        
        if (credentials.Length != 2)
        {
            return null;
        }

        return (credentials[0], credentials[1]);
    }
}
