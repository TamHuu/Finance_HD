using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using iText.Layout;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Html2pdf;
using iText.Kernel.Geom;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal; // Nếu không sử dụng, xóa đi
using Microsoft.IdentityModel.Tokens; // Nếu không sử dụng, xóa đi
using iText.Kernel.Pdf.Canvas.Parser; // Nếu không sử dụng, xóa đi
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using OfficeOpenXml;
namespace Finance_HD.Controllers.ChungTu
{
    public class ExpenseRequestController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ExpenseRequestController(ILogger<ExpenseRequestController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            var branches = _dbContext.SysBranch.ToList();
            var deNghiChis = _dbContext.FiaDeNghiChi.ToList();

            if (Request.Cookies["FullName"] != null)
            {
                ViewData["FullName"] = Request.Cookies["FullName"];
            }
            else
            {
                ViewData["FullName"] = "Cookie không tồn tại";
            }
            ViewData["listChiNhanh"] = branches;

            return View(deNghiChis);
        }
        [HttpPost]
        public IActionResult GetListExpenseRequest(string TuNgay, string DenNgay, string ChiNhanhDeNghi)
        {
            DateTime dtpTuNgay = TuNgay.ToDateTime2(DateTime.Now)!.Value;
            DateTime dtpDenNgay = DenNgay.ToDateTime2(DateTime.Now)!.Value;

            var listExpenseRequest = (from denghichi in _dbContext.FiaDeNghiChi
                                      join chinhanhdenghi in _dbContext.SysBranch
                                          on denghichi.MaChiNhanhDeNghi equals chinhanhdenghi.Ma into chinhanhdenghiGroup
                                      from chinhanhdenghi in chinhanhdenghiGroup.DefaultIfEmpty()
                                      join bophandenghi in _dbContext.TblPhongBan
                                          on denghichi.MaPhongBanDeNghi equals bophandenghi.Ma into bophandenghiGroup
                                      from bophandenghi in bophandenghiGroup.DefaultIfEmpty()
                                      join chinhanhchi in _dbContext.SysBranch
                                          on denghichi.MaChiNhanhChi equals chinhanhchi.Ma into chinhanhchiGroup
                                      from chinhanhchi in chinhanhchiGroup.DefaultIfEmpty()
                                      join bophanchi in _dbContext.TblPhongBan
                                          on denghichi.MaPhongBanChi equals bophanchi.Ma into bophanchiGroup
                                      from bophanchi in bophanchiGroup.DefaultIfEmpty()
                                      join noidungthuchi in _dbContext.CatNoiDungThuChi
                                          on denghichi.MaNoiDung equals noidungthuchi.Ma into noidungthuchiGroup
                                      from noidungthuchi in noidungthuchiGroup.DefaultIfEmpty()
                                      join tien in _dbContext.FiaTienTe
                                          on denghichi.MaTienTe equals tien.Ma into tienGroup
                                      from tien in tienGroup.DefaultIfEmpty()
                                      join nguoilapphieu in _dbContext.SysUser
                                          on denghichi.UserCreated equals nguoilapphieu.Ma into NguoilapphieuGroup
                                      from nguoilapphieu in NguoilapphieuGroup.DefaultIfEmpty()
                                      join nguoiduyet in _dbContext.SysUser
                                          on denghichi.NguoiDuyet equals nguoiduyet.Ma into NguoiduyetphieuGroup
                                      from nguoiduyet in NguoiduyetphieuGroup.DefaultIfEmpty()
                                      where !(denghichi.Deleted ?? false)
                                          && (string.IsNullOrEmpty(ChiNhanhDeNghi) ||
                                              denghichi.MaChiNhanhDeNghi == ChiNhanhDeNghi.GetGuid() ||
                                              ChiNhanhDeNghi.GetGuid() == Finance_HD.Helpers.CommonGuids.defaultUID)
                                          && denghichi.NgayLap.HasValue
                                          && denghichi.NgayLap.Value.Date >= dtpTuNgay.Date
                                          && denghichi.NgayLap.Value.Date <= dtpDenNgay.Date
                                      orderby denghichi.CreatedDate descending
                                      select new
                                      {
                                          Ma = denghichi.Ma + "",
                                          MaChiNhanhDeNghi = chinhanhdenghi.Ma + "",
                                          MaChiNhanhChi = chinhanhchi.Ma + "",
                                          MaPhongBanDeNghi = bophandenghi.Ma + "",
                                          MaPhongBanChi = bophanchi.Ma + "",
                                          SoPhieu = denghichi.SoPhieu + "",
                                          NgayLap = denghichi.NgayLap + "",
                                          NgayYeuCauNhanTien = denghichi.NgayYeuCauNhanTien + "",
                                          MaNoiDung = noidungthuchi.Ma + "",
                                          MaTienTe = tien.Ma + "",
                                          TyGia = denghichi.TyGia ?? 1,
                                          SoTien = denghichi.SoTien ?? 0,
                                          NoiDungThuChi = noidungthuchi.Ten + "",
                                          TenChiNhanhDeNghi = chinhanhdenghi.Ten + "",
                                          TenChiNhanhChi = chinhanhchi.Ten + "",
                                          TenTienTe = tien.Ten + "",
                                          TenPhongBanDeNghi = bophandenghi.Ten ?? "",
                                          TenPhongBanChi = bophanchi.Ten ?? "",
                                          HinhThucChi = denghichi.HinhThucChi == (int)HinhThucThuChi.TienMat ? "Tiền mặt" : denghichi.HinhThucChi == (int)HinhThucThuChi.TaiKhoanCaNhan ? "Tài khoản cá nhân" : denghichi.HinhThucChi == (int)HinhThucThuChi.NganHang ? "Ngân hàng" : "",
                                          GhiChu = denghichi.GhiChu + "",
                                          TenTrangThai = denghichi.TrangThai == (int)TrangThaiChungTu.LapPhieu ? "Lập phiếu" : denghichi.TrangThai == (int)TrangThaiChungTu.DaDuyet ? "Đã duyệt đề nghị" : denghichi.TrangThai == (int)TrangThaiChungTu.DaThu ? "Đã thu" : denghichi.TrangThai == (int)TrangThaiChungTu.DaChi ? "Đã chi" : "",
                                          NguoiDuyet = nguoilapphieu.FullName + "",
                                          NgayDuyet = denghichi.NgayDuyet + "",
                                          TenNguoiChiTien = nguoiduyet.FullName + "",
                                          NgayChi = denghichi.NgayYeuCauNhanTien + "",
                                      }).ToList();

            return Json(new { success = true, Data = listExpenseRequest });
        }

        [HttpPost]
        public IActionResult ExportToExcel(string TuNgay, string DenNgay, string ChiNhanhDeNghi, string fileType)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var dtpTuNgay = TuNgay.ToDateTime2(DateTime.Now)!.Value;
                var dtpDenNgay = DenNgay.ToDateTime2(DateTime.Now)!.Value;
                string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
                var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
                var listExpenseRequest = (from denghichi in _dbContext.FiaDeNghiChi
                                          join chinhanhdenghi in _dbContext.SysBranch
                                          on denghichi.MaChiNhanhDeNghi equals chinhanhdenghi.Ma into chinhanhdenghiGroup
                                          from chinhanhdenghi in chinhanhdenghiGroup.DefaultIfEmpty()
                                          join bophandenghi in _dbContext.TblPhongBan
                                          on denghichi.MaPhongBanDeNghi equals bophandenghi.Ma into bophandenghiGroup
                                          from bophandenghi in bophandenghiGroup.DefaultIfEmpty()
                                          join chinhanhchi in _dbContext.SysBranch
                                          on denghichi.MaChiNhanhChi equals chinhanhchi.Ma into chinhanhchiGroup
                                          from chinhanhchi in chinhanhchiGroup.DefaultIfEmpty()
                                          join bophanchi in _dbContext.TblPhongBan
                                          on denghichi.MaPhongBanChi equals bophanchi.Ma into bophanchiGroup
                                          from bophanchi in bophanchiGroup.DefaultIfEmpty()
                                          join noidungthuchi in _dbContext.CatNoiDungThuChi
                                          on denghichi.MaNoiDung equals noidungthuchi.Ma into noidungthuchiGroup
                                          from noidungthuchi in noidungthuchiGroup.DefaultIfEmpty()
                                          join tien in _dbContext.FiaTienTe
                                          on denghichi.MaTienTe equals tien.Ma into tienGroup
                                          from tien in tienGroup.DefaultIfEmpty()
                                          join user in _dbContext.SysUser
                                          on denghichi.UserCreated equals user.Ma
                                          where !(denghichi.Deleted ?? false)
                          &&
                               (ChiNhanhDeNghi == null || denghichi.MaChiNhanhDeNghi == ChiNhanhDeNghi.GetGuid() ||
                                ChiNhanhDeNghi.GetGuid() == Finance_HD.Helpers.CommonGuids.defaultUID)
                               && denghichi.NgayLap != null && denghichi.NgayLap.Value.Date >= dtpTuNgay.Date && denghichi.NgayLap.Value.Date <= dtpDenNgay.Date
                                          orderby denghichi.CreatedDate descending
                                          select new
                                          {
                                              Ma = denghichi.Ma + "",
                                              MaChiNhanhDeNghi = chinhanhdenghi.Ma + "",
                                              MaChiNhanhChi = chinhanhchi.Ma + "",
                                              MaPhongBanDeNghi = bophandenghi.Ma + "",
                                              MaPhongBanChi = bophanchi.Ma + "",
                                              SoPhieu = denghichi.SoPhieu + "",
                                              NgayLap = denghichi.NgayLap + "",
                                              NgayYeuCauNhanTien = denghichi.NgayYeuCauNhanTien + "",
                                              MaNoiDung = noidungthuchi.Ma + "",
                                              MaTienTe = tien.Ma + "",
                                              TyGia = denghichi.TyGia ?? 1,
                                              SoTien = denghichi.SoTien ?? 0,
                                              NoiDungThuChi = noidungthuchi.Ten + "",
                                              TenChiNhanhDeNghi = chinhanhdenghi.Ten + "",
                                              TenChiNhanhChi = chinhanhchi.Ten + "",
                                              TenTienTe = tien.Ten + "",
                                              TenPhongBanDeNghi = bophandenghi.Ten ?? "",
                                              TenPhongBanChi = bophanchi.Ten ?? "",
                                              HinhThucChi = denghichi.HinhThucChi == (int)HinhThucThuChi.TienMat ? "Tiền mặt" :
                                                            denghichi.HinhThucChi == (int)HinhThucThuChi.TaiKhoanCaNhan ? "Tài khoản cá nhân" :
                                                            denghichi.HinhThucChi == (int)HinhThucThuChi.NganHang ? "Ngân hàng" : "",
                                              GhiChu = denghichi.GhiChu + "",
                                              TenTrangThai = denghichi.TrangThai == (int)TrangThaiChungTu.LapPhieu ? "Lập phiếu" :
                                                             denghichi.TrangThai == (int)TrangThaiChungTu.DaDuyet ? "Đã duyệt đề nghị" :
                                                             denghichi.TrangThai == (int)TrangThaiChungTu.DaThu ? "Đã thu" :
                                                             denghichi.TrangThai == (int)TrangThaiChungTu.DaChi ? "Đã chi" : "",
                                              TenNguoiDuyet = loggedInUser.FullName + "",
                                              TenNguoiLapPhieu = user.FullName + "",
                                              NgayDuyet = denghichi.NgayDuyet + "",
                                              NgayChi = denghichi.NgayYeuCauNhanTien + "",
                                              SoTienChi = 0,
                                          }).ToList();

                var templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "DeNghiChi.xlsx");
                using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        // Fill data into the worksheet
                        ExcelHelper.FillData(
                            worksheet,
                            listExpenseRequest,
                            5, // Starting row
                            18, // Number of columns
                            (item, colIndex) =>
                            {
                                var propertyValues = new List<object>
                                {
                    item.NgayLap,
                    item.SoPhieu,
                    item.TenChiNhanhDeNghi,
                    item.TenPhongBanDeNghi,
                    item.TenNguoiLapPhieu,
                    item.TenChiNhanhChi,
                    item.TenPhongBanChi,
                    item.SoTien,
                    item.TenTienTe,
                    item.TyGia,
                    item.NgayYeuCauNhanTien,
                    item.NoiDungThuChi,
                    item.HinhThucChi,
                    item.GhiChu,
                    item.TenTrangThai,
                    item.TenNguoiDuyet,
                    item.NgayChi,
                    item.SoTienChi
                                };

                                return propertyValues[colIndex - 1];
                            });

                        for (int i = 0; i < listExpenseRequest.Count; i++)
                        {
                            int rowIndex = i + 5;
                            for (int colIndex = 1; colIndex <= 18; colIndex++)
                            {
                                var cell = worksheet.Cells[rowIndex, colIndex];

                                ExcelHelper.ApplyCellStyle(cell, true, System.Drawing.Color.Black, System.Drawing.Color.LightYellow, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                                cell.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                if ((colIndex >= 1 && colIndex <= 7) || colIndex == 9 || colIndex == 11 || colIndex == 12 || colIndex == 13 || colIndex == 14)
                                {
                                    cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                }
                                else
                                {
                                    cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    cell.Style.Numberformat.Format = "#,##0.00";
                                }
                                if (colIndex == 15 || colIndex == 16 || colIndex == 17)
                                {
                                    cell.Style.Numberformat.Format = "dd/MM/yyyy"; // Định dạng ngày tháng
                                    cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // Căn giữa cho các ô ngày tháng
                                }
                            }
                        }

                        var excelStream = new MemoryStream();
                        package.SaveAs(excelStream);
                        excelStream.Position = 0;

                        var fileName = $"DeNghiChi_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        return File(excelStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ExportToPdf(string TuNgay, string DenNgay, string ChiNhanhDeNghi, string fileType)
        {
            // Validate incoming parameters
            if (string.IsNullOrEmpty(TuNgay) || string.IsNullOrEmpty(DenNgay) || string.IsNullOrEmpty(ChiNhanhDeNghi))
            {
                return BadRequest("Thông tin không hợp lệ.");
            }

            try
            {
                // Create PDF
                using (var stream = new MemoryStream())
                {
                    using (var writer = new PdfWriter(stream))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            pdf.SetDefaultPageSize(PageSize.A4.Rotate()); // Chuyển sang chế độ nằm ngang

                            var document = new Document(pdf);
                            string fontPath = "wwwroot/fonts/NotoSans_Condensed-Bold.ttf";
                            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

                            // Add title and subtitle with the specified font
                            document.Add(new Paragraph("Tiêu đề tài liệu PDF")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(font)
                                .SetFontSize(16)); // Set font size for the title
                            document.Add(new Paragraph("Danh sách yêu cầu chi:")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(font)
                                .SetFontSize(14)); // Set font size for the subtitle
                            document.Add(new Paragraph("\n")); // Add spacing

                            // Create table with 11 columns and set widths
                            var table = new iText.Layout.Element.Table(new float[] { 8, 6, 12, 10, 8, 10, 10, 10, 6, 6, 20 });
                            table.SetFixedLayout(); // Thiết lập layout cố định cho bảng

                            // Add table headers
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Ngày lập")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Mã")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Đơn vị đề nghị")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Bộ phận đề nghị")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Người lập")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Đơn vị chi")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Bộ phận chi")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Số tiền đề nghị")).SetFont(font).SetTextAlignment(TextAlignment.RIGHT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Đơn vị tiền")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Tỷ giá")).SetFont(font).SetTextAlignment(TextAlignment.RIGHT));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Nội dung")).SetFont(font).SetTextAlignment(TextAlignment.LEFT));

                            // Sample data for the table
                            var expenseRequests = new[]
                            {
                        new { NgayLap = new DateTime(2024, 10, 1), Ma = "PH001", DonViDeNghi = "Công ty A", BoPhanDeNghi = "Phòng Kế toán", NguoiLap = "Nguyễn Văn A", DonViChi = "Công ty B", BoPhanChi = "Phòng Tài chính", SoTienDeNghi = 1000000, DonViTien = "VND", TyGia = 1, NoiDung = "Chi phí văn phòng" },
                        new { NgayLap = new DateTime(2024, 10, 5), Ma = "PH002", DonViDeNghi = "Công ty C", BoPhanDeNghi = "Phòng Nhân sự", NguoiLap = "Trần Thị B", DonViChi = "Công ty D", BoPhanChi = "Phòng Hành chính", SoTienDeNghi = 2000000, DonViTien = "VND", TyGia = 1, NoiDung = "Chi phí tuyển dụng" },
                        // Additional items can be added here
                    };

                            // Add data to the table
                            foreach (var item in expenseRequests)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(item.NgayLap.ToString("dd/MM/yyyy"))).SetFont(font)); // Format date
                                table.AddCell(new Cell().Add(new Paragraph(item.Ma)).SetFont(font));
                                table.AddCell(new Cell().Add(new Paragraph(item.DonViDeNghi)).SetFont(font));
                                table.AddCell(new Cell().Add(new Paragraph(item.BoPhanDeNghi)).SetFont(font));
                                table.AddCell(new Cell().Add(new Paragraph(item.NguoiLap)).SetFont(font));
                                table.AddCell(new Cell().Add(new Paragraph(item.DonViChi)).SetFont(font));
                                table.AddCell(new Cell().Add(new Paragraph(item.BoPhanChi)).SetFont(font));
                                table.AddCell(new Cell().Add(new Paragraph(item.SoTienDeNghi.ToString("N0"))).SetFont(font)); // Format money
                                table.AddCell(new Cell().Add(new Paragraph(item.DonViTien)).SetFont(font));
                                table.AddCell(new Cell().Add(new Paragraph(item.TyGia.ToString("N2"))).SetFont(font)); // Format exchange rate
                                table.AddCell(new Cell().Add(new Paragraph(item.NoiDung)).SetFont(font));

                            }

                            // Add the table to the document
                            document.Add(table);
                            document.Close();
                        }
                    }

                    // Return the PDF file as a response
                    return File(stream.ToArray(), "application/pdf", "DeNghiChi.pdf");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging library)
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Có lỗi xảy ra khi tạo PDF.");
            }
        }

        [HttpGet]
        public IActionResult Add()
        {

            ViewData["listTienTe"] = _dbContext.FiaTienTe.ToList();
            ViewData["listNoiDung"] = _dbContext.CatNoiDungThuChi.ToList();
            return View("Form", new FiaDeNghiChi());
        }
        [HttpPost]
        public JsonResult Add(string ngayLap, string ngayNhanTien, string chiNhanhDeNghi, string phongBanDeNghi, string chiNhanhChi, string phongBanChi, string noiDung, string tienTe, decimal tyGia, decimal soTien, string hinhThuc, string ghiChu)
        {
            if (soTien <= 0 || tyGia <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            SysBranch? cn = _dbContext.SysBranch.FirstOrDefault(t => t.Ma == chiNhanhDeNghi.GetGuid());
            if (cn == null)
            {
                return Json(new { success = false, message = "Chi nhánh không tồn tại!" });
            }

            int nextSequentialNumber = GetNextSequentialNumber(cn.Code);

            DateTime ngayLapDate, ngayNhanTienDate;
            if (!DateTime.TryParse(ngayLap, out ngayLapDate) || !DateTime.TryParse(ngayNhanTien, out ngayNhanTienDate))
            {
                return Json(new { success = false, message = "Ngày lập hoặc ngày nhận tiền không hợp lệ!" });
            }
            Finance_HD.Common.HinhThucThuChi hinhThucChiEnum;
            Finance_HD.Common.TrangThaiChungTu trangThaiChungTuEnum;
            if (!Enum.TryParse(hinhThuc, out hinhThucChiEnum))
            {
                return Json(new { success = false, message = "Hình thức chi không hợp lệ!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var ExpenseRequest = new FiaDeNghiChi
            {
                MaChiNhanhDeNghi = chiNhanhDeNghi.GetGuid(),
                MaChiNhanhChi = chiNhanhChi.GetGuid(),
                MaPhongBanDeNghi = phongBanDeNghi.GetGuid(),
                MaPhongBanChi = phongBanChi.GetGuid(),
                SoPhieu = string.Format("DNC/{0}/{1:yyyyMMdd}/{2:000}", cn.Code, DateTime.Today, nextSequentialNumber),
                NgayLap = ngayLapDate,
                NgayYeuCauNhanTien = ngayNhanTienDate,
                MaNoiDung = noiDung.GetGuid(),
                MaTienTe = tienTe.GetGuid(),
                TyGia = tyGia,
                SoTien = soTien,
                TrangThai = (int)TrangThaiChungTu.LapPhieu,
                HinhThucChi = Convert.ToInt32(hinhThucChiEnum),
                GhiChu = ghiChu,
                CreatedDate = DateTime.Now,
                UserCreated = loggedInUser.Ma,
            };

            _dbContext.FiaDeNghiChi.Add(ExpenseRequest);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Đề nghị chi đã được thêm thành công!" });
        }

        // Hàm để lấy số thứ tự tiếp theo
        private int GetNextSequentialNumber(string chiNhanhCode)
        {
            var lastRequest = _dbContext.FiaDeNghiChi
                .Where(x => x.SoPhieu.StartsWith($"DNC/{chiNhanhCode}/"))
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefault();

            if (lastRequest != null)
            {
                // Lấy số thứ tự từ số phiếu hiện tại
                var parts = lastRequest.SoPhieu.Split('/');
                if (parts.Length > 3 && int.TryParse(parts[3], out int lastNumber))
                {
                    return lastNumber + 1; // Tăng số thứ tự lên 1
                }
            }

            return 1; // Nếu không có số nào, bắt đầu từ 1
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listTienTe"] = _dbContext.FiaTienTe.ToList();
            ViewData["listNoiDung"] = _dbContext.CatNoiDungThuChi.ToList();
            var FiaDeNghiChi = _dbContext.FiaDeNghiChi.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (FiaDeNghiChi == null)
            {
                return NotFound();
            }
            return View("Form", FiaDeNghiChi);
        }
        [HttpPost]
        public JsonResult Edit(string ma, string ngayLap, string ngayNhanTien, string chiNhanhDeNghi, string phongBanDeNghi, string chiNhanhChi, string phongBanChi, string noiDung, string tienTe, string tyGia, string soTien, string hinhThuc, string trangThai, string ghiChu)
        {
            var ExpenseRequest = _dbContext.FiaDeNghiChi.FirstOrDefault(x => x.Ma == ma.GetGuid());
            if (ExpenseRequest == null)
            {
                return Json(new { success = false, message = "Đề nghị chi này không tồn tại!" });
            }

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            ExpenseRequest.Ma = ma.GetGuid();
            ExpenseRequest.MaChiNhanhDeNghi = chiNhanhDeNghi.GetGuid();
            ExpenseRequest.MaChiNhanhChi = chiNhanhChi.GetGuid();
            ExpenseRequest.MaPhongBanDeNghi = phongBanDeNghi.GetGuid();
            ExpenseRequest.MaPhongBanChi = phongBanChi.GetGuid();
            ExpenseRequest.NgayLap = DateTime.Parse(ngayLap);
            ExpenseRequest.NgayYeuCauNhanTien = DateTime.Parse(ngayNhanTien);
            ExpenseRequest.MaNoiDung = noiDung.GetGuid();
            ExpenseRequest.MaTienTe = tienTe.GetGuid();
            ExpenseRequest.TyGia = decimal.Parse(tyGia);
            ExpenseRequest.SoTien = decimal.Parse(soTien);
            ExpenseRequest.HinhThucChi = Convert.ToInt32(hinhThuc); ;
            ExpenseRequest.GhiChu = ghiChu;

            // Thông tin người sửa và ngày sửa
            ExpenseRequest.UserModified = loggedInUser.Ma;
            ExpenseRequest.ModifiedDate = DateTime.Now;

            _dbContext.FiaDeNghiChi.Update(ExpenseRequest);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật Đề nghị chi thành công!" });
        }

        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var ExpenseRequest = _dbContext.FiaDeNghiChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (ExpenseRequest == null)
            {
                return Json(new { success = false, message = "Đề nghị chi không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            ExpenseRequest.Deleted = true;  // Đánh dấu đã bị xoá
            ExpenseRequest.DeletedDate = DateTime.Now;  // Lưu thời gian xoá
            ExpenseRequest.UserDeleted = loggedInUser.Ma;
            _dbContext.FiaDeNghiChi.Update(ExpenseRequest);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
        [HttpPost]
        public JsonResult DuyetPhieuDeNghiChi(string Id)
        {
            var item = _dbContext.FiaDeNghiChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (item == null)
            {
                return Json(new { success = false, message = "Phiếu này không tồn tại" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser.Ma == item.MaChiNhanhDeNghi)
            {
                return Json(new { success = false, message = "Không được duyệt phiếu từ chi nhánh khác" });
            }
            if (item.TrangThai > (int)TrangThaiChungTu.LapPhieu)
            {
                return Json(new { success = false, message = "Phiếu này đã duyệt rồi" });
            }
            item.TrangThai = (int)TrangThaiChungTu.DaDuyet;
            item.NguoiDuyet = loggedInUser.Ma;
            item.NgayDuyet = DateTime.Now;
            _dbContext.Update(item);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Duyệt phiếu thành công" });
        }

        [HttpPost]
        public JsonResult BoDuyetPhieuDeNghiChi(string Id)
        {
            var item = _dbContext.FiaDeNghiChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (item == null)
            {
                return Json(new { success = false, message = "Phiếu này không tồn tại" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

            if (loggedInUser.Ma == item.MaChiNhanhDeNghi)
            {
                return Json(new { success = false, message = "Không được bỏ duyệt phiếu từ chi nhánh khác" });
            }
            if (item.TrangThai == (int)TrangThaiChungTu.LapPhieu)
            {
                return Json(new { success = false, message = "Phiếu chưa duyệt" });
            }

            if (item.TrangThai == (int)TrangThaiChungTu.DaChi)
            {
                return Json(new { success = false, message = "Phiếu đã chi tiền" });
            }
            item.TrangThai = (int)TrangThaiChungTu.LapPhieu;
            item.NguoiDuyet = loggedInUser.Ma;
            item.NgayDuyet = DateTime.Now;
            item.UserModified = loggedInUser.Ma;
            _dbContext.Update(item);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Bỏ duyệt phiếu thành công" });
        }
    }
}
