using System.Data;
using ExcelDataReader;
using System.Text;
using Microsoft.AspNetCore.Hosting;

public class ExcelDataReaderService
{
    private readonly IWebHostEnvironment _env;

    public ExcelDataReaderService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public Dictionary<string, DataTable> ReadAllSheets(string filePath)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var result = reader.AsDataSet(new ExcelDataSetConfiguration
        {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration
            {
                UseHeaderRow = true 
            }
        });

        var sheets = new Dictionary<string, DataTable>();
        foreach (DataTable table in result.Tables)
        {
            sheets.Add(table.TableName, table);
        }

        return sheets;
    }
}