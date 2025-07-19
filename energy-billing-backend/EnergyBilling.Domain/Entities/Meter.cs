namespace EnergyBilling.Domain.Entities;

public class Meter
{
    public Guid Id { get; set; }
    public string MeterNo { get; set; }
    public string CustomerName { get; set; }
    public string SalesMethod { get; set; } // "PTF" veya "Tarife"
    public string TariffGroup { get; set; } // Tarifeye göre ayrım
    public decimal BtvRate { get; set; }    // % olarak
    public decimal KdvRate { get; set; }    // % olarak
}
