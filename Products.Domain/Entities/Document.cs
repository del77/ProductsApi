namespace Products.Domain.Entities;

public class Document
{
    public string BaCode { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime OperationDate { get; set; }
    public int DocumentDayNumber { get; set; }
    public string ContractorCode { get; set; } = string.Empty;
    public string ContractorName { get; set; } = string.Empty;
    public string ExternalDocumentNumber { get; set; } = string.Empty;
    public DateTime ExternalDocumentDate { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal F1 { get; set; }
    public decimal F2 { get; set; }
    public decimal F3 { get; set; }
    
    public string? Comment { get; set; }
    public List<Product> Products { get; set; } = new();
}
