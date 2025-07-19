namespace EnergyBilling.Domain.Entities;

public class Consumption
{
    public Guid Id { get; set; }
    public string MeterNo { get; set; }
    public DateTime Date { get; set; }  // Saatlik veri için
    public decimal Value { get; set; }  // Tüketim miktarı
}
