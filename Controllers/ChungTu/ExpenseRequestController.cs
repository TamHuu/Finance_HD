using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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

            // Trả về kết quả dưới dạng JSON
            return Json(new { success = true, Data = listExpenseRequest });
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

            // Cập nhật các trường từ tham số
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
        public JsonResult BoDuyetPhieuDeNghiChi(string ma)
        {
            var item = _dbContext.FiaDeNghiChi.FirstOrDefault(x => x.Ma == ma.GetGuid());
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
            item.TrangThai = (int)TrangThaiChungTu.DaDuyet;
            item.NguoiDuyet = loggedInUser.Ma;
            item.NgayDuyet = DateTime.Now;
            _dbContext.Update(item);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Duyệt phiếu thành công" });
        }
    }
}
