using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;

namespace Finance_HD.Controllers.ChungTu
{
    public class ReceiptController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ReceiptController(ILogger<ReceiptController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
        }


        public IActionResult Index()
        {
            if (Request.Cookies["FullName"] != null)
            {
                ViewData["FullName"] = Request.Cookies["FullName"];
            }
            else
            {
                ViewData["FullName"] = "Cookie không tồn tại";
            }
            var result = _dbContext.FiaPhieuThuChi.ToList();
            return View(result);
        }
        [HttpPost]
        public JsonResult getListReceipt(DateTime? TuNgay, DateTime? DenNgay, string DonViThu)
        {
            DateTime? dtpTuNgay = TuNgay;
            DateTime? dtpDenNgay = DenNgay;

            var listReceipt = (from phieuthu in _dbContext.FiaPhieuThuChi
                               join loaithuchi in _dbContext.CatLoaiThuChi
                                   on phieuthu.MaLoaiThuChi equals loaithuchi.Ma into LoaiThuChiGroup
                               from loaithuchi in LoaiThuChiGroup.DefaultIfEmpty()
                               join chinhanhchi in _dbContext.SysBranch
                                   on phieuthu.MaChiNhanhChi equals chinhanhchi.Ma into ChiNhanhChiGroup
                               from chinhanhchi in ChiNhanhChiGroup.DefaultIfEmpty()
                               join chinhanhthu in _dbContext.SysBranch
                                   on phieuthu.MaChiNhanhThu equals chinhanhthu.Ma into ChiNhanhThuGroup
                               from chinhanhthu in ChiNhanhThuGroup.DefaultIfEmpty()
                               join phongbanchi in _dbContext.TblPhongBan
                                   on phieuthu.MaPhongBanChi equals phongbanchi.Ma into PhongBanChiGroup
                               from phongbanchi in PhongBanChiGroup.DefaultIfEmpty()
                               join phongbanthu in _dbContext.TblPhongBan
                                   on phieuthu.MaPhongBanThu equals phongbanthu.Ma into PhongBanThuGroup
                               from phongbanthu in PhongBanThuGroup.DefaultIfEmpty()
                               join noidungthuchi in _dbContext.CatNoiDungThuChi
                                   on phieuthu.MaNoiDungThuChi equals noidungthuchi.Ma into NoiDungThuChiGroup
                               from noidungthuchi in NoiDungThuChiGroup.DefaultIfEmpty()
                               join nguoigiaodich in _dbContext.SysUser
                                   on phieuthu.NguoiGiaoDich equals nguoigiaodich.Ma into NguoiGiaoDichGroup
                               from nguoigiaodich in NguoiGiaoDichGroup.DefaultIfEmpty()
                               join tiente in _dbContext.FiaTienTe
                                   on phieuthu.MaTienTe equals tiente.Ma into TienTeGroup
                               from tiente in TienTeGroup.DefaultIfEmpty()
                               join hinhthuc in _dbContext.TblHinhThucThuChi
                                   on phieuthu.MaHinhThuc equals hinhthuc.Ma into HinhThucGroup
                               from hinhthuc in HinhThucGroup.DefaultIfEmpty()
                               join bangke in _dbContext.FiaBangKeNopTien
                                   on phieuthu.MaBangKeNopTien equals bangke.Ma into BangKeGroup
                               from bangke in BangKeGroup.DefaultIfEmpty()
                               join nguoilapphieu in _dbContext.SysUser
                                   on phieuthu.NguoiLapPhieu equals nguoilapphieu.Ma into NguoiLapPhieuGroup
                               from nguoilapphieu in NguoiLapPhieuGroup.DefaultIfEmpty()
                               join phieuchi in _dbContext.FiaPhieuThuChi
                                   on phieuthu.MaPhieuChi equals phieuchi.Ma into PhieuChiGroup
                               from phieuchi in PhieuChiGroup.DefaultIfEmpty()
                               join nguoinhantien in _dbContext.SysUser
                                   on phieuthu.NguoiNhanTien equals nguoinhantien.Ma into NguoiNhanTienGroup
                               from nguoinhantien in NguoiNhanTienGroup.DefaultIfEmpty()
                               where !(phieuthu.Deleted ?? false) &&
         (string.IsNullOrEmpty(DonViThu) ||
          phieuthu.MaChiNhanhThu == DonViThu.GetGuid() ||
          DonViThu.GetGuid() == Finance_HD.Helpers.CommonGuids.defaultUID)
                               // &&
                               //(!dtpTuNgay.HasValue || phieuthu.NgayLapPhieu >= dtpTuNgay.Value) &&
                               //(!dtpDenNgay.HasValue || phieuthu.NgayLapPhieu <= dtpDenNgay.Value)
                               select new
                               {
                                   Ma = phieuthu.Ma.ToString(),
                                   MaLoaiThuChi = loaithuchi.Ma.ToString(),
                                   TenLoaiThuChi = loaithuchi.Ten,
                                   MaChiNhanhChi = chinhanhchi.Ma.ToString(),
                                   TenChiNhanhChi = chinhanhchi.Ten,
                                   MaChiNhanhThu = chinhanhthu.Ma.ToString(),
                                   TenChiNhanhThu = chinhanhthu.Ten,
                                   MaPhongBanChi = phongbanchi.Ma.ToString(),
                                   TenPhongBanChi = phongbanchi.Ten,
                                   MaPhongBanThu = phongbanthu.Ma.ToString(),
                                   TenPhongBanThu = phongbanthu.Ten,
                                   MaNoiDungThuChi = noidungthuchi.Ma.ToString(),
                                   TenNoiDungThuChi = noidungthuchi.Ten,
                                   SoPhieu = phieuthu.SoPhieu.ToString(),
                                   NgayLapPhieu = phieuthu.NgayLapPhieu,
                                   NgayDuyet = phieuthu.NgayDuyet,
                                   NguoiDuyet = phieuthu.NguoiDuyet,
                                   NguoiGiaoDich = nguoigiaodich.Ma.ToString(),
                                   TenNguoiGiaoDich = nguoigiaodich.FullName,
                                   MaTienTe = tiente.Ma.ToString(),
                                   TenTienTe = tiente.Ten,
                                   TyGia = phieuthu.TyGia ?? 0,
                                   MaHinhThuc = hinhthuc.Ma.ToString(),
                                   SoTien = phieuthu.SoTien ?? 0,
                                   GhiChu = phieuthu.GhiChu,
                                   SoHoSoKemTheo = phieuthu.SoHoSoKemTheo ?? 0,
                                   MaBangKeNopTien = bangke.Ma.ToString(),
                                   TenBangKeNopTien = tiente.Ten,
                                   NguoiLapPhieu = nguoilapphieu.Ma.ToString(),
                                   TenNguoiLapPhieu = nguoilapphieu.FullName,
                                   TenNguoiNhanTien = nguoinhantien.FullName,
                                   CreatedDate = phieuthu.CreatedDate,
                                   TrangThai = phieuthu.TrangThai == (int)TrangThaiChungTu.LapPhieu ? "Lập phiếu" :
            phieuthu.TrangThai == (int)TrangThaiChungTu.DaDuyet ? "Đã duyệt đề nghị" :
            phieuthu.TrangThai == (int)TrangThaiChungTu.DaThu ? "Đã thu" :
            phieuthu.TrangThai == (int)TrangThaiChungTu.DaChi ? "Đã chi" :
            phieuthu.TrangThai == (int)TrangThaiChungTu.TamNop ? "Tạm nộp" : "",

                                   HinhThuc = phieuthu.MaHinhThuc == (int)HinhThucThuChi.TienMat ? "Tiền mặt" : phieuthu.MaHinhThuc == (int)HinhThucThuChi.TaiKhoanCaNhan ? "Tài khoản cá nhân" : phieuthu.MaHinhThuc == (int)HinhThucThuChi.NganHang ? "Ngân hàng" : "",
                               })
                               .OrderByDescending(receipt => receipt.CreatedDate)
                               .ToList();

            return Json(new { success = true, Data = listReceipt });
        }

        public IActionResult Add()
        {
            ViewBag.DongTienThu = _dbContext.CatNoiDungThuChi.ToList();
            ViewBag.DongTienChi = _dbContext.CatNoiDungThuChi.ToList();
            ViewBag.LoaiTaiKhoanNganHang = _dbContext.FiaLoaiTaiKhoanNganHang
           .GroupBy(x => x.Ten)
           .Select(g => g.First())
           .ToList();

            return View("Form", new FiaPhieuThuChi());
        }
        private int GetNextSequentialNumber(string chiNhanhCode)
        {
            var lastRequest = _dbContext.FiaPhieuThuChi
                .Where(x => x.SoPhieu.StartsWith($"PT/{chiNhanhCode}/"))
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
        [HttpPost]
        public JsonResult Add(
    string BangKe,
    string BoPhanChi,
    string BoPhanThu,
    string DonViChi,
    string DonViThu,
    DateTime NgayLap,
    string NguoiThuTien,
    string NhanVienNop,
    string KhachHangNop,
    string Ma,
    string MaDongTien,
    string SoPhieuChi,
    string SoTien,
    string TienTe,
    string TyGia,
    string HinhThuc,
    string MaPhieuChi,
    string GhiChu,
    string SoHoSoKemTheo
            )
        {


            if (string.IsNullOrWhiteSpace(DonViThu))
            {
                return Json(new { success = false, message = "Tên chi nhánh không được để trống!" });
            }
            SysBranch? cn = _dbContext.SysBranch.FirstOrDefault(t => t.Ma == DonViThu.GetGuid());
            if (cn == null)
            {
                return Json(new { success = false, message = "Chi nhánh không tồn tại!" });
            }

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            int nextSequentialNumber = GetNextSequentialNumber(cn.Code);
            var Receipt = new FiaPhieuThuChi
            {
                MaLoaiThuChi = Guid.Parse("AB3FF94A-905D-47C3-89B4-E8F5099CDC9D"),
                MaChiNhanhThu = DonViThu.GetGuid(),
                MaChiNhanhChi = DonViChi.GetGuid(),
                MaPhongBanChi = BoPhanChi.GetGuid(),
                MaPhongBanThu = BoPhanThu.GetGuid(),
                NgayLapPhieu = NgayLap,
                NguoiGiaoDich = NguoiThuTien.GetGuid(),
                NguoiLapPhieu = NhanVienNop.GetGuid(),
                TenNguoiGiaoDich = KhachHangNop,
                MaBangKeNopTien = BangKe.GetGuid(),
                MaTienTe = TienTe.GetGuid(),
                TyGia = TyGia.ToInt(),
                SoPhieu = string.Format("PT/{0}/{1:yyyyMMdd}/{2:000}", cn.Code, DateTime.Today, nextSequentialNumber),
                SoTien = SoTien.ToInt(),
                MaHinhThuc = HinhThuc.ToInt(),
                MaChungTuKeToan = SoHoSoKemTheo,
                MaNoiDungThuChi = MaDongTien.GetGuid(),
                GhiChu = GhiChu,
                TrangThai = (int)TrangThaiChungTu.TamNop,
            
                MaPhieuChi = MaPhieuChi.GetGuid(),

            };

            _dbContext.FiaPhieuThuChi.Add(Receipt);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var listReceipt = _dbContext.FiaPhieuThuChi.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (listReceipt == null)
            {
                return NotFound();
            }
            return View("Form", listReceipt);
        }
        [HttpPost]
        public JsonResult Edit(string Ma,
    string BangKe,
    string BoPhanChi,
    string BoPhanThu,
    string DonViChi,
    string DonViThu,
    DateTime NgayLap,
    string NguoiThuTien,
    string NhanVienNop,
    string KhachHangNop,
    string MaDongTien,
    string SoChungTu,
    string SoPhieuChi,
    string SoTien,
    string TienTe,
    string TyGia,
    string HinhThuc,
    string MaPhieuChi,
    string GhiChu
)
        {
            var listReceipt = _dbContext.FiaPhieuThuChi.FirstOrDefault(x => x.Ma == Ma.GetGuid());
          
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            listReceipt.MaLoaiThuChi = Guid.Parse("AB3FF94A-905D-47C3-89B4-E8F5099CDC9D");
            listReceipt.MaChiNhanhThu = DonViThu.GetGuid();
            listReceipt.MaPhongBanThu = BoPhanThu.GetGuid();
            listReceipt.MaNoiDungThuChi = MaDongTien.GetGuid();
            listReceipt.MaBangKeNopTien = BangKe.GetGuid();
            listReceipt.SoHoSoKemTheo = SoChungTu.ToInt();
            listReceipt.NgayLapPhieu = NgayLap;
            listReceipt.NguoiLapPhieu = loggedInUser.Ma;
            listReceipt.TrangThai = (int)TrangThaiChungTu.TamNop;
            listReceipt.MaPhongBanChi = BoPhanChi.GetGuid();
            listReceipt.MaChiNhanhChi = DonViChi.GetGuid();
            listReceipt.MaTienTe = TienTe.GetGuid();
            listReceipt.TyGia = TyGia.ToInt();
            listReceipt.SoTien = SoTien.ToInt();
            listReceipt.MaHinhThuc = HinhThuc.ToInt();
            listReceipt.GhiChu = GhiChu;
            listReceipt.MaPhieuChi = MaPhieuChi.GetGuid();
            listReceipt.SoPhieu = SoPhieuChi;
            listReceipt.TenNguoiNhanTien = loggedInUser.FullName;
            listReceipt.NguoiNhanTien = loggedInUser.Ma;
            listReceipt.NguoiGiaoDich = KhachHangNop.GetGuid();
            _dbContext.FiaPhieuThuChi.Update(listReceipt);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var listReceipt = _dbContext.FiaPhieuThuChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (listReceipt == null)
            {
                return Json(new { success = false, message = "Tiền tệ không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            listReceipt.UserDeleted = loggedInUser.Ma;
            listReceipt.Deleted = true;  // Đánh dấu đã bị xoá
            listReceipt.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FiaPhieuThuChi.Update(listReceipt);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá tiền tệ thành công!" });
        }
        [HttpPost]
        public JsonResult DuyetPhieuThu(string Id)
        {
            var item = _dbContext.FiaPhieuThuChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (item == null)
            {
                return Json(new { success = false, message = "Phiếu này không tồn tại" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser.Ma == item.MaChiNhanhThu)
            {
                return Json(new { success = false, message = "Không được duyệt phiếu từ chi nhánh khác" });
            }
          
            item.TrangThai = (int)TrangThaiChungTu.DaThu;
            item.NguoiDuyet = loggedInUser.Ma;
            item.NgayDuyet = DateTime.Now;
            item.UserModified = loggedInUser.Ma;
            _dbContext.Update(item);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Duyệt phiếu thành công" });
        }

        [HttpPost]
        public JsonResult BoDuyetDuyetPhieuThu(string Id)
        {
            var item = _dbContext.FiaPhieuThuChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (item == null)
            {
                return Json(new { success = false, message = "Phiếu này không tồn tại" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

            if (loggedInUser.Ma == item.MaChiNhanhThu)
            {
                return Json(new { success = false, message = "Không được bỏ duyệt phiếu từ chi nhánh khác" });
            }
          
            item.TrangThai = (int)TrangThaiChungTu.TamNop;
            item.NguoiDuyet = loggedInUser.Ma;
            item.NgayDuyet = DateTime.Now;
            item.UserModified = loggedInUser.Ma;
            _dbContext.Update(item);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Bỏ duyệt phiếu thành công" });
        }
    }
}
