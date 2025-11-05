using Microsoft.AspNetCore.Http;
using Products.Application.DTOs;
using Products.Application.Exceptions;
using Products.Application.Interfaces;

namespace Products.Application.Services;

public class DocumentProcessingService
{
    private readonly IFileDataExtractor _fileDataExtractor;
    private readonly IProductsDataProcessor _productsDataProcessor;

    public DocumentProcessingService(
        IFileDataExtractor fileDataExtractor,
        IProductsDataProcessor productsDataProcessor)
    {
        _fileDataExtractor = fileDataExtractor;
        _productsDataProcessor = productsDataProcessor;
    }

    public async Task<DocumentProcessingResult> ProcessDocumentFileAsync(IFormFile? file, int documentProductsCountThreshold)
    {
        if (file is null || file.Length == 0)
        {
            throw new InvalidProductsFileException();
        }

        using var reader = new StreamReader(file.OpenReadStream());
        var fileContent = await reader.ReadToEndAsync();
        
        var documents = _fileDataExtractor.ExtractTextFromFileAsync(fileContent);
        
        return _productsDataProcessor.Process(fileContent, documents, documentProductsCountThreshold);
    }
}
