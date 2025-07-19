namespace EnergyBilling.Shared.Dtos;

public class InvoiceCalculationResponseDto
{
    public Guid CustomerId { get; set; }
    public decimal EnergyTotal { get; set; }
    public decimal DistributionTotal { get; set; }
    public decimal BtvTotal { get; set; }
    public decimal VatTotal { get; set; }
    public decimal GrandTotal { get; set; }
}