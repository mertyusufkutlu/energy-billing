namespace EnergyBilling.Domain.Entities;

public class PriceInfo
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }  // Saatlik PTF için
    public decimal? PtfPrice { get; set; }
    public decimal? YekPrice { get; set; }
    public string? TariffGroup { get; set; }
    public decimal? TariffPrice { get; set; }
    public decimal? DistributionPrice { get; set; }
}
