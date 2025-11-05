using System.Text.RegularExpressions;
using Products.Application.DTOs;
using Products.Application.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Services;

public class ProductsDataProcessor : IProductsDataProcessor
{
    public DocumentProcessingResult Process(string fileContent, List<Document> documents,
        int documentProductsCountThreshold)
    {
        var (grossAmountSum, productWithMaxNetValue) = ExtractTotalGrossAmountAndProductWithHighestNetAmount(documents);

        return new DocumentProcessingResult
        {
            Documents = documents.ToList(),
            LineCount = Regex.Matches(fileContent, @"\r?\n").Count,
            CharCount = fileContent.Length,
            Sum = grossAmountSum,
            XCount = documents.Count(d => d.Products.Count > documentProductsCountThreshold),
            ProductsWithMaxNetValue = string.Join(", ", productWithMaxNetValue.Select(p => p.ProductName)),
        };
    }

    private static (decimal GrossAmountSum, List<Product> productWithMaxNetValue)
        ExtractTotalGrossAmountAndProductWithHighestNetAmount(List<Document> documents)
    {
        var sum = 0m;
        List<Product>? productsWithMaxNetValue = [];
        
        foreach (var document in documents)
        {
            sum += document.GrossAmount;
            foreach (var documentProduct in document.Products)
            {
                if (productsWithMaxNetValue.Count == 0)
                {
                    productsWithMaxNetValue.Add(documentProduct);
                }
                else if (documentProduct.NetValue == productsWithMaxNetValue[0].NetValue)
                {
                    productsWithMaxNetValue.Add(documentProduct);
                }
                else if (documentProduct.NetValue > productsWithMaxNetValue[0].NetValue)
                {
                    productsWithMaxNetValue = [documentProduct];
                }
            }
        }

        return (sum, productsWithMaxNetValue);
    }
}