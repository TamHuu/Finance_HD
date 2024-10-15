using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace Finance_HD.Common
{
    public class ExcelExportHelper
    {
        public static FileContentResult ExportToExcel<T>(List<T> data, string fileName)
        {
            // Thiết lập sử dụng EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Thiết lập ngữ cảnh giấy phép

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Thiết lập tiêu đề
                var properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i].Name; // Tiêu đề
                }

                // Điền dữ liệu vào Excel
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < properties.Length; j++)
                    {
                        var value = properties[j].GetValue(data[i]);
                        worksheet.Cells[i + 2, j + 1].Value = value; // Dữ liệu
                    }
                }

                // Chuyển đổi tệp Excel thành mảng byte
                var excelData = package.GetAsByteArray();
                return new FileContentResult(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = $"{fileName}.xlsx"
                };
            }
        }
    }

}
