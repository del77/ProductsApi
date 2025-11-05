using Products.Domain.Exceptions;

namespace Products.Application.Exceptions;

public class InvalidProductsFileException : ProductsException
{
    public InvalidProductsFileException() : this(null)
    {
        
    }
    
    public InvalidProductsFileException(Exception? exception) 
        : base(exception, "Could not process products file",
            "INVALID_PRODUCTS_FILE",
            "Plik z produktami nie został dostarczony lub jest pusty")
    {
    }
}