using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using UpravlenieRC.Server.DTO;
using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.Services
{
    public class InvantarizationService
    {
        private readonly RC_SkladContext _context;
        public InvantarizationService(RC_SkladContext context)
        {
            _context = context;
        }
        public async Task<FileData> GetInventarizationExcel()
        {

            CellBalanceServices cellBalanceservice = new CellBalanceServices(_context);
            List<CellBalanseDTO> cellBalanse = await cellBalanceservice.GetCellBalanse();
            ExcelPackage.License.SetNonCommercialPersonal("Dmitry0752");
            using var package = new ExcelPackage();
            var grouped = cellBalanse
                .GroupBy(x => new { x.storageCellDTO.Name })
                .OrderBy(g => g.Key.Name);
            foreach (var group in grouped)
            {
                
                var worksheet = package.Workbook.Worksheets.Add(group.Key.Name);

                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "Товар";
                worksheet.Cells["C1"].Value = "Номер партии";
                worksheet.Cells["D1"].Value = "Кол-во (учёт)";
                worksheet.Cells["E1"].Value = "Кол-во (факт)";
                worksheet.Cells["F1"].Value = "Расхождение";
                var headerRange = worksheet.Cells["A1:F1"];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int row = 2;
                foreach (var item in group)
                {
                    worksheet.Cells[row, 1].Value = item.tovarDTO?.Id;
                    worksheet.Cells[row, 2].Value = item.tovarDTO?.Name;
                    worksheet.Cells[row, 3].Value = item.tovarDTO?.BatchNumber;
                    worksheet.Cells[row, 4].Value = item.amount;
                    worksheet.Cells[row, 6].Formula = $"=(D{row}-E{row})";

                    row++;
                }

                worksheet.Columns[1, 6].AutoFit();

                if (row > 2)
                {
                    int totalRow = row;
                    worksheet.Cells[totalRow, 3].Value = "Итого:";
                    worksheet.Cells[totalRow, 3].Style.Font.Bold = true;
                    worksheet.Cells[totalRow, 4].Formula = $"=SUM(D2:D{row - 1})";
                }
            }
            var excelData = package.GetAsByteArray();
            string date = DateTime.Now.ToString("dd.MM.yy_HH.mm");
            string fileName = $"{date}_inventory.xlsx";

            return new FileData(excelData, fileName);
        }
    }
}
