namespace Products.Domain.Entities;

public class Product
{
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal NetPrice { get; set; }
    public decimal NetValue { get; set; }
    public decimal Vat { get; set; }
    public decimal PreviousQuantity { get; set; }
    public decimal AveragePreviousPrice { get; set; }
    public decimal CurrentQuantity { get; set; }
    public decimal AverageCurrentPrice { get; set; }
    public string Group { get; set; } = string.Empty;
}
