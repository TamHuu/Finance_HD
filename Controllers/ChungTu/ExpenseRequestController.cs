using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using iText.Layout;
using OfficeOpenXml;
using Enum = System.Enum;

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
                                      on denghichi.UserCreated equals nguoilapphieu.Ma

                                      where !(denghichi.Deleted ?? false) &&
                                            (string.IsNullOrEmpty(ChiNhanhDeNghi) ||
                                             denghichi.MaChiNhanhDeNghi == ChiNhanhDeNghi.GetGuid() ||
                                             ChiNhanhDeNghi.GetGuid() == Finance_HD.Helpers.CommonGuids.defaultUID) &&
                                            (denghichi.NgayLap >= dtpTuNgay && denghichi.NgayLap <= dtpDenNgay)
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
                                          where !(denghichi.Deleted ?? false) &&
                                                (string.IsNullOrEmpty(ChiNhanhDeNghi) ||
                                                 denghichi.MaChiNhanhDeNghi == ChiNhanhDeNghi.GetGuid() ||
                                                 ChiNhanhDeNghi.GetGuid() == Finance_HD.Helpers.CommonGuids.defaultUID) &&
                                                (denghichi.NgayLap >= dtpTuNgay && denghichi.NgayLap <= dtpDenNgay)
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

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "DeNghiChi.xlsx");
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

                                if (colIndex == 8 || colIndex == 18)
                                {
                                    cell.Style.Numberformat.Format = "#,##0.00";
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
            try
            {
                // Chuyển đổi ngày từ chuỗi sang DateTime
                var dtpTuNgay = TuNgay.ToDateTime2(DateTime.Now)!.Value;
                var dtpDenNgay = DenNgay.ToDateTime2(DateTime.Now)!.Value;
                string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
                var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

                // Truy vấn dữ liệu
                var listExpenseRequest  = (from denghichi in _dbContext.FiaDeNghiChi

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
                                                                   on denghichi.UserCreated equals nguoilapphieu.Ma

                                                                   where !(denghichi.Deleted ?? false) &&
                                                                         (string.IsNullOrEmpty(ChiNhanhDeNghi) ||
                                                                          denghichi.MaChiNhanhDeNghi == ChiNhanhDeNghi.GetGuid() ||
                                                                          ChiNhanhDeNghi.GetGuid() == Finance_HD.Helpers.CommonGuids.defaultUID) &&
                                                                         (denghichi.NgayLap >= dtpTuNgay && denghichi.NgayLap <= dtpDenNgay)
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
                                                                       NgayChi = denghichi.NgayYeuCauNhanTien + "",
                                                                   }).ToList();


                // Tạo PDF
                // Tạo PDF
                using (var memoryStream = new MemoryStream())
                {
                    using (var pdfWriter = new PdfWriter(memoryStream))
                    using (var pdfDocument = new PdfDocument(pdfWriter))
                    {
                        var document = new Document(pdfDocument);

                        // Thêm tiêu đề
                        document.Add(new Paragraph("Danh Sách Đề Nghị Chi")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(20));

                        document.Add(new Paragraph($"Người lập: {loggedInUser.FullName}")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFontSize(12));

                        // Tạo bảng
                        var table = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2, 1, 2 })).UseAllAvailableWidth();
                        table.SetMarginTop(20);

                        // Thêm tiêu đề cho bảng
                        table.AddHeaderCell("Số Phiếu");
                        table.AddHeaderCell("Ngày Lập");
                        table.AddHeaderCell("Chi Nhánh Đề Nghị");
                        table.AddHeaderCell("Số Tiền");
                        table.AddHeaderCell("Ghi Chú");

                        // Thêm dữ liệu vào bảng
                        foreach (var request in listExpenseRequest)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(request.SoPhieu)));
                            table.AddCell(new Cell().Add(new Paragraph(request.NgayLap))); // Định dạng ngày
                            table.AddCell(new Cell().Add(new Paragraph(request.TenChiNhanhDeNghi)));
                            table.AddCell(new Cell().Add(new Paragraph(request.SoTien.ToString("N0")))); // Định dạng số tiền
                            table.AddCell(new Cell().Add(new Paragraph(request.GhiChu)));
                        }

                        // Thêm bảng vào tài liệu
                        document.Add(table);
                        document.Close();
                    }

                    // Trả về file PDF cho người dùng
                    var fileName = $"DeNghiChi_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                    return File(memoryStream.ToArray(), "application/pdf", fileName);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
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
