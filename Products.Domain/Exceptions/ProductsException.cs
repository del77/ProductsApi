namespace Products.Domain.Exceptions;

public abstract class ProductsException : Exception
{
    public string ErrorCode { get; }
    public string UserMessage { get; }

    protected ProductsException(Exception? exception, string message, string errorCode, string userMessage) : base(message, exception)
    {
        ErrorCode = errorCode;
        UserMessage = userMessage;
    }
}