namespace EnergyBilling.Shared.Dtos;

public class InvoiceCalculationRequestDto
{
    public Guid CustomerId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}