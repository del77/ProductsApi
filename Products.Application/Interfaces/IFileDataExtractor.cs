using Products.Domain.Entities;

namespace Products.Application.Interfaces;

public interface IFileDataExtractor
{
    List<Document> ExtractTextFromFileAsync(string fileContent);
}
