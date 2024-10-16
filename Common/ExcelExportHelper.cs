using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing; // Namespace cho System.Drawing.Color

namespace Finance_HD.Common
{
    public static class ExcelHelper
    {
        public static void FillData<T>(ExcelWorksheet worksheet, List<T> dataList, int startRow, int totalColumns, Func<T, int, object> getValueForColumn)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                int rowIndex = i + startRow;
                for (int colIndex = 1; colIndex <= totalColumns; colIndex++) // Sử dụng số lượng cột được truyền vào
                {
                    worksheet.Cells[rowIndex, colIndex].Value = getValueForColumn(dataList[i], colIndex);
                }
            }
        }


        // Hàm áp dụng style cho ô Excel
        public static void ApplyCellStyle(ExcelRange cell, bool isBold, Color fontColor, Color backgroundColor, OfficeOpenXml.Style.ExcelHorizontalAlignment horizontalAlignment)
        {
            cell.Style.Font.Bold = isBold; // Định dạng chữ in đậm
            cell.Style.Font.Color.SetColor(fontColor); // Màu chữ
            cell.Style.HorizontalAlignment = horizontalAlignment; // Căn chỉnh theo chiều ngang
            cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; // Căn chỉnh theo chiều dọc
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid; // Kiểu nền
            cell.Style.Fill.BackgroundColor.SetColor(backgroundColor); // Màu nền
            cell.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin; // Viền trên mỏng
            cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin; // Viền dưới mỏng
            cell.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin; // Viền trái mỏng
            cell.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin; // Viền phải mỏng

        }
    }
}
