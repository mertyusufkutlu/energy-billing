using EnergyBilling.Application.Invoices.Queries;
using EnergyBilling.Shared.Dtos;
using MediatR;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public class CalculateInvoiceQueryHandler : IRequestHandler<CalculateInvoiceQuery, InvoiceCalculationResponseDto>
{
    private readonly ExcelDataReaderService _excelReader;

    public CalculateInvoiceQueryHandler(ExcelDataReaderService excelReader)
    {
        _excelReader = excelReader;
    }

    public async Task<InvoiceCalculationResponseDto> Handle(CalculateInvoiceQuery request, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "Sayax_Task_Veri.xlsx");
        var sheets = _excelReader.ReadAllSheets(filePath);

        var meterInfo = sheets["Sayac Bilgileri"];
        var priceInfo = sheets["Fiyat Bilgileri"];

        decimal totalEnergy = 0;
        decimal totalDistribution = 0;
        decimal totalBtv = 0;

        foreach (DataRow meterRow in meterInfo.Rows.Cast<DataRow>())
        {
            var meterName = meterRow[0]?.ToString()?.Trim();
            var salesType = meterRow[1]?.ToString()?.Trim();
            var commissionText = meterRow[2]?.ToString()?.Trim();
            var btvRate = Convert.ToDecimal(meterRow[3]);
            var vatRate = Convert.ToDecimal(meterRow[4]);
            var tariffName = meterRow[5]?.ToString()?.Trim();

            var consumptionSheetName = $"{meterName} Tuketim";
            if (!sheets.ContainsKey(consumptionSheetName)) continue;

            var consumptionSheet = sheets[consumptionSheetName];
            var consumptionRows = consumptionSheet.Rows.Cast<DataRow>();

            decimal meterTotal = 0;
            decimal yekPrice = GetPriceByKey(priceInfo, "yek fiyati (tl/mwh)");
            decimal distributionUnitPrice = GetPriceByKey(priceInfo, $"{tariffName} dağıtım tarifesi");
            decimal energyTariff = GetPriceByKey(priceInfo, $"{tariffName} enerji tarifesi");

            Console.WriteLine($"[INFO] Meter: {meterName} | Tariff: {tariffName} | DistributionUnitPrice: {distributionUnitPrice}");

            foreach (var row in consumptionRows)
            {
                if (!DateTime.TryParse(row["Tarih"]?.ToString(), out var originalDate)) continue;
                if (!decimal.TryParse(row["Tüketim (MWh)"]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var consumption)) continue;

                var adjustedDate = originalDate.AddYears(1);
                var priceRow = FindClosestPriceRow(priceInfo, adjustedDate);
                if (priceRow == null) continue;

                if (!decimal.TryParse(priceRow["PTF (TL/MWh)"]?.ToString(), out var ptf)) continue;

                decimal energy = 0;

                if (salesType.Contains("PTF") && salesType.Contains("Komisyon") && commissionText.Contains("%"))
                {
                    decimal commissionRate = decimal.Parse(commissionText.Replace("%", "").Trim()) / 100;
                    energy = (ptf + yekPrice) * (1 + commissionRate) * consumption;
                }
                else if (salesType.Contains("PTF") && commissionText.ToLower().Contains("tl"))
                {
                    decimal commissionValue = decimal.Parse(commissionText.Replace("TL", "").Trim());
                    energy = (ptf + yekPrice + commissionValue) * consumption;
                }
                else if (salesType.Contains("Tarife"))
                {
                    decimal discountRate = decimal.Parse(commissionText.Replace("%", "").Trim()) / 100;
                    energy = energyTariff * (1 - discountRate) * consumption;
                }

                meterTotal += energy;
                totalDistribution += distributionUnitPrice * consumption;
                totalBtv += energy * btvRate;
            }

            totalEnergy += meterTotal;
        }

        var totalVat = (totalEnergy + totalDistribution + totalBtv) * 0.1m;
        var grandTotal = totalEnergy + totalDistribution + totalBtv + totalVat;

        return new InvoiceCalculationResponseDto
        {
            CustomerId = request.Input.CustomerId,
            EnergyTotal = totalEnergy,
            DistributionTotal = totalDistribution,
            BtvTotal = totalBtv,
            VatTotal = totalVat,
            GrandTotal = grandTotal
        };
    }

    private decimal GetPriceByKey(DataTable priceSheet, string labelKey)
    {
        var normalizedKey = Normalize(labelKey);

        foreach (DataRow row in priceSheet.Rows)
        {
            foreach (var cell in row.ItemArray)
            {
                var label = cell?.ToString();
                if (string.IsNullOrWhiteSpace(label)) continue;

                var normalizedLabel = Normalize(label);

                Console.WriteLine($"[LABEL CHECK] '{label}' -> normalized: '{normalizedLabel}' vs '{normalizedKey}'");

                if (normalizedLabel == normalizedKey)
                {
                    // Etiket bulundu, yanındaki hücredeki değeri alalım
                    int labelIndex = row.ItemArray.ToList().IndexOf(cell);
                    if (labelIndex + 1 < row.ItemArray.Length)
                    {
                        var valueRaw = row.ItemArray[labelIndex + 1]?.ToString()?.ToLower()?.Replace("tl", "").Trim();
                        if (decimal.TryParse(valueRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                        {
                            return value;
                        }
                        else
                        {
                            Console.WriteLine($"[PARSE FAIL] Value: '{valueRaw}' for key: '{label}'");
                        }
                    }
                }
            }
        }

        Console.WriteLine($"[KEY NOT FOUND] -> '{labelKey}'");
        return 0;
    }




    private DataRow? FindClosestPriceRow(DataTable priceTable, DateTime adjustedDate)
    {
        DataRow? closestRow = null;
        TimeSpan smallestDifference = TimeSpan.MaxValue;

        foreach (DataRow row in priceTable.Rows)
        {
            if (DateTime.TryParse(row["Takvim_TarihSaat"]?.ToString(), out var ptfDate))
            {
                var diff = (ptfDate - adjustedDate).Duration();
                if (diff < smallestDifference)
                {
                    smallestDifference = diff;
                    closestRow = row;
                }
            }
        }

        return closestRow;
    }

    private string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "";
        return input
            .ToLower(new CultureInfo("tr-TR", false))
            .Replace("ı", "i")
            .Replace("ğ", "g")
            .Replace("ü", "u")
            .Replace("ş", "s")
            .Replace("ö", "o")
            .Replace("ç", "c")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("/mwh", "")
            .Replace("tl", "")
            .Replace("-", "")
            .Replace("_", "")
            .Replace(" ", "")
            .Trim();
    }


}
