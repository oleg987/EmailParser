using EmailParser.Entities;
using OfficeOpenXml;

namespace EmailParser.DataStorage;

public class LocalDataStorage : IDataStorage
{
    private readonly string _path = "requests.xlsx";
    
    public async Task StoreAsync(List<CustomerRequest> requests, CancellationToken cancellationToken)
    {
        await using var file = new FileStream(_path, FileMode.OpenOrCreate);
        
        using var excel = new ExcelPackage(file);

        var worksheet = excel.Workbook.Worksheets["requests"] ?? excel.Workbook.Worksheets.Add("requests");

        foreach (var request in requests)
        {
            worksheet.InsertRow(1, 1);

            worksheet.Cells["A1"].Value = request.Name;
            worksheet.Cells["B1"].Value = request.Phone;
        }

        await excel.SaveAsync(cancellationToken);
    }
}