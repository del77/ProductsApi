using Products.Application.DTOs;
using Products.Domain.Entities;

namespace Products.Application.Interfaces;

public interface IProductsDataProcessor
{
    DocumentProcessingResult Process(string fileContent, List<Document> documents, int documentProductsCountThreshold);
}