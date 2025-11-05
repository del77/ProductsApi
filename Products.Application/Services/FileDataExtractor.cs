using System.Globalization;
using Products.Application.Exceptions;
using Products.Application.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Services;

public class FileDataExtractor : IFileDataExtractor
{
    private const char Separator = ',';
    
    public List<Document> ExtractTextFromFileAsync(string fileContent)
    {
        try
        {
            var lines = GetDocumentLines(fileContent);
            var documents = new List<Document>();

            Document? currentlyProcessedDocument = null;

            foreach (var line in lines)
            {
                var lineParts = line.Split(Separator);
            
                if (lineParts.Length == 0)
                {
                    continue;
                }

                var lineType = lineParts[0].Trim();

                switch (lineType)
                {
                    case "H":
                        currentlyProcessedDocument = ParseHeaderLine(lineParts);
                        documents.Add(currentlyProcessedDocument);
                        break;

                    case "C":
                        currentlyProcessedDocument!.Comment = string.Join("", lineParts.Skip(1));
                        break;

                    case "B":
                        var documentLine = ParseBodyLine(lineParts);
                        currentlyProcessedDocument!.Products.Add(documentLine);
                        break;
                }
            }
        
            return documents;
        }
        catch (Exception e)
        {
            throw new InvalidProductsFileException(e);
        }
    }

    private static IReadOnlyList<string> GetDocumentLines(string fileContent)
    {
        var lines = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        return lines
            .Where(l => !string.IsNullOrWhiteSpace(l) && l.Any(c => c != Separator))
            .ToList();
    }

    private Document ParseHeaderLine(string[] parts)
    {
        if (parts.Length != 17)
        {
            throw new InvalidProductsFileException();
        }
        
        var document = new Document
        {
            BaCode = parts[1].Trim(),
            Type = parts[2].Trim(),
            DocumentNumber = parts[3].Trim(),
            OperationDate = ParseDate(parts[4].Trim()),
            DocumentDayNumber = int.Parse(parts[5].Trim()),
            ContractorCode = parts[6].Trim(),
            ContractorName = parts[7].Trim(),
            ExternalDocumentNumber = parts[8].Trim(),
            ExternalDocumentDate = ParseDate(parts[9].Trim()),
            NetAmount = ParseDecimal(parts[10].Trim()),
            VatAmount = ParseDecimal(parts[11].Trim()),
            GrossAmount = ParseDecimal(parts[12].Trim()),
            F1 = ParseDecimal(parts[13].Trim()),
            F2 = ParseDecimal(parts[14].Trim()),
            F3 = ParseDecimal(parts[15].Trim())
        };

        return document;
    }

    private Product ParseBodyLine(string[] parts)
    {
        if (parts.Length != 13)
        {
            throw new InvalidProductsFileException();
        }
        
        var line = new Product
        {
            ProductCode = parts[1].Trim(),
            ProductName = parts[2].Trim(),
            Quantity = ParseDecimal(parts[3].Trim()),
            NetPrice = ParseDecimal(parts[4].Trim()),
            NetValue = ParseDecimal(parts[5].Trim()),
            Vat = ParseDecimal(parts[6].Trim()),
            PreviousQuantity = ParseDecimal(parts[7].Trim()),
            AveragePreviousPrice = ParseDecimal(parts[8].Trim()),
            CurrentQuantity = ParseDecimal(parts[9].Trim()),
            AverageCurrentPrice = ParseDecimal(parts[10].Trim()),
            Group = parts[11].Trim()
        };

        return line;
    }

    private DateTime ParseDate(string value) => DateTime.ParseExact(value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    private decimal ParseDecimal(string value) => decimal.Parse(value, CultureInfo.InvariantCulture);
}
