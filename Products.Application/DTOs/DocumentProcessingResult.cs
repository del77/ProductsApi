namespace Products.Application.DTOs;

public class DocumentProcessingResult
{
    public List<Domain.Entities.Document> Documents { get; set; } = new();
    public int LineCount { get; set; }
    public int CharCount { get; set; }
    public decimal Sum { get; set; }
    public int XCount { get; set; }
    public string ProductsWithMaxNetValue { get; set; } = string.Empty;
}
