using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.ChungTu
{
    public class CashDepositController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CashDepositController(ILogger<CashDepositController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
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

        public IActionResult Index()
        {
            if (Request.Cookies["FullName"] != null)
            {
                ViewData["FullName"] = Request.Cookies["FullName"];
            }
            else
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpPost]
        public JsonResult getDanhSachBangKe(string TuNgay, string DenNgay, string DonViNop)
        {
            DateTime dtpTuNgay = TuNgay.ToDateTime2(DateTime.Now)!.Value;
            DateTime dtpDenNgay = DenNgay.ToDateTime2(DateTime.Now)!.Value;

            var listBangKe = (from bangkenoptien in _dbContext.FiaBangKeNopTien
                              join chinhanhnop in _dbContext.SysBranch
                              on bangkenoptien.MaChiNhanhNop equals chinhanhnop.Ma

                              join phongbannop in _dbContext.TblPhongBan
                              on bangkenoptien.MaPhongBanNop equals phongbannop.Ma into PhongBanNopGroup
                              from phongbannop in PhongBanNopGroup.DefaultIfEmpty()

                              join chinhanhnhan in _dbContext.SysBranch
                              on bangkenoptien.MaChiNhanhNhan equals chinhanhnhan.Ma

                              join phongbannhan in _dbContext.TblPhongBan
                              on bangkenoptien.MaPhongBanNhan equals phongbannhan.Ma into PhongBanNhanGroup
                              from phongbannhan in PhongBanNhanGroup.DefaultIfEmpty()

                              join tiente in _dbContext.FiaTienTe
                              on bangkenoptien.MaTienTe equals tiente.Ma

                              join noidung in _dbContext.CatNoiDungThuChi
                              on bangkenoptien.MaNoiDung equals noidung.Ma into NoiDungGroup
                              from noidung in NoiDungGroup.DefaultIfEmpty()

                              join hinhthuc in _dbContext.TblHinhThucThuChi
                              on bangkenoptien.MaHinhThuc equals hinhthuc.Ma into HinhThucGroup
                              from hinhthuc in HinhThucGroup.DefaultIfEmpty()

                              join nguoinoptien in _dbContext.SysUser
                              on bangkenoptien.NguoiNopTien equals nguoinoptien.Ma

                              join nguoinhantien in _dbContext.SysUser
                              on bangkenoptien.NguoiNhanTien equals nguoinhantien.Ma into NguoiNhanTienGroup
                              from nguoinhantien in NguoiNhanTienGroup.DefaultIfEmpty()

                              where !(bangkenoptien.Deleted ?? false) &&
                                             (string.IsNullOrEmpty(DonViNop) ||
                                              bangkenoptien.MaChiNhanhNop == DonViNop.GetGuid() ||
                                              DonViNop.GetGuid() == Finance_HD.Helpers.CommonGuids.defaultUID) &&
                                             (bangkenoptien.NgayLap >= dtpTuNgay && bangkenoptien.NgayLap <= dtpDenNgay)
                              orderby bangkenoptien.CreatedDate descending
                              select new
                              {
                                  Ma = bangkenoptien.Ma + "",
                                  TenChiNhanhNhan = chinhanhnhan.Ten + "",
                                  TenChiNhanhNop = chinhanhnop.Ten + "",
                                  TenPhongBanNhan = phongbannhan.Ten + "",
                                  TenPhongBanNop = phongbannop.Ten + "",
                                  SoPhieu = bangkenoptien.SoPhieu + "",
                                  NgayLap = bangkenoptien.NgayLap + "",
                                  NgayNopTien = bangkenoptien.NgayNopTien + "",
                                  TenNguoiNopTien = nguoinoptien.FullName + "",
                                  DiaChi = bangkenoptien.DiaChi + "",
                                  LyDo = bangkenoptien.LyDo + "",
                                  TenTienTe = tiente.Ten + "",
                                  TyGia = bangkenoptien.TyGia ?? 0,
                                  SoTien = bangkenoptien.SoTien ?? 0,
                                  GhiChu = bangkenoptien.GhiChu + "",
                                  NguoiNhanTien = nguoinhantien.FullName + "",
                                  NoiDung = noidung.Ten + "",
                                  TrangThai = bangkenoptien.TrangThai == (int)TrangThaiChungTu.LapPhieu ? "Lập phiếu" : bangkenoptien.TrangThai == (int)TrangThaiChungTu.DaDuyet ? "Đã duyệt đề nghị" : bangkenoptien.TrangThai == (int)TrangThaiChungTu.DaThu ? "Đã thu" : bangkenoptien.TrangThai == (int)TrangThaiChungTu.DaChi ? "Đã chi" : "",
                                  CreatedDate = DateTime.Now,
                                  HinhThuc = bangkenoptien.MaHinhThuc == (int)HinhThucThuChi.TienMat ? "Tiền mặt" : bangkenoptien.MaHinhThuc == (int)HinhThucThuChi.TaiKhoanCaNhan ? "Tài khoản cá nhân" : bangkenoptien.MaHinhThuc == (int)HinhThucThuChi.NganHang ? "Ngân hàng" : "",
                              }).ToList();
            return Json(new { success = true, Data = listBangKe });
        }

        [HttpPost]
        public JsonResult getChiTietBangKe(string Ma)
        {
            var listChiTietBangKe = (from chitietbangke in _dbContext.FiaChiTietBangKeNopTien
                                     join loaitien in _dbContext.FaLoaiTien
                                     on chitietbangke.MaLoaiTien equals loaitien.Ma into loaitienGroup
                                     from loaitien in loaitienGroup.DefaultIfEmpty()

                                     join bangkenoptien in _dbContext.FiaBangKeNopTien
                                     on chitietbangke.MaBangKeNopTien equals bangkenoptien.Ma into bangkeGroup
                                     from bangkenoptien in bangkeGroup.DefaultIfEmpty()

                                     where chitietbangke.MaBangKeNopTien == Ma.GetGuid()
                           && !(chitietbangke.Deleted ?? false)
                                     select new
                                     {
                                         Ma = chitietbangke.Ma + "",
                                         MaLoaiTien = loaitien.Ma + "",
                                         TenLoaiTien = loaitien.GiaTri ?? 0,
                                         SoLuong = chitietbangke.SoLuong ?? 0,
                                         ThanhTien = chitietbangke.ThanhTien ?? 0,
                                         GhiChu = chitietbangke.GhiChu + "",
                                         CreatedDate = DateTime.Now,
                                     }).OrderByDescending(x => x.CreatedDate).ToList();
            return Json(new { success = true, Data = listChiTietBangKe });
        }
        [HttpPost]
        public JsonResult getChiTietNhanVien(string Ma)
        {
            var listChiTietNhanVien = (from bangkenhanvien in _dbContext.FiaChiTietBangKeNhanVien
                                       join nhanvien in _dbContext.SysUser
                                       on bangkenhanvien.MaNhanVien equals nhanvien.Ma

                                       join bangkenoptien in _dbContext.FiaChiTietBangKeNopTien
                                       on bangkenhanvien.MaBangKe equals bangkenoptien.Ma
                                       where bangkenhanvien.MaBangKe == Ma.GetGuid()
                             && !(bangkenhanvien.Deleted ?? false)
                                       select new
                                       {
                                           Ma = bangkenhanvien.Ma + "",
                                           MaBangKe = bangkenoptien.Ma + "",
                                           MaNhanVien = nhanvien.Ma + "",
                                           SoTien = bangkenhanvien.SoTien ?? 0,
                                           UserCreated = bangkenhanvien.UserCreated,
                                           CreatedDate = DateTime.Now,
                                       }).OrderByDescending(x => x.CreatedDate).ToList();
            return Json(new { success = true, Data = listChiTietNhanVien });
        }
        [HttpGet]
        public IActionResult Add()
        {
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

            ViewData["listTienTe"] = _dbContext.FiaTienTe.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNoiDung"] = _dbContext.CatNoiDungThuChi.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNguoiNopTien"] = _dbContext.SysUser.Where(x => !(x.Deleted ?? false)).ToList();

            return View("Form", new FiaBangKeNopTien());
        }


        [HttpPost]
        public JsonResult Add(
       string Ma,
       string MaChiNhanhNhan,
       string MaChiNhanhNop,
       string MaPhongBanNhan,
       string MaPhongBanNop,
       DateTime NgayNopTien,
       DateTime NgayLap,
       int MaHinhThuc,
       string MaTienTe,
       decimal TyGia,
       string NguoiNopTien,
       string MaNoiDung,
       string GhiChu,
       string DiaChi,
       string LyDo,
       int SoTien,
       string NguoiNhanTien,
    List<FiaChiTietBangKeNopTien> DataChiTietBangKe,
    List<FiaChiTietBangKeNhanVien> DataNhanVien
   )
        {

            SysBranch? cn = _dbContext.SysBranch.FirstOrDefault(t => t.Ma == MaChiNhanhNop.GetGuid());
            if (cn == null)
            {
                return Json(new { success = false, message = "Chi nhánh không tồn tại!" });
            }

            int nextSequentialNumber = GetNextSequentialNumber(cn.Code);
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

            // Kiểm tra bảng kê có tồn tại cho chi nhánh này chưa
            var existingListDocumentType = _dbContext.FiaBangKeNopTien.FirstOrDefault(x => x.MaChiNhanhNop == loggedInUser.Ma);
            if (existingListDocumentType != null)
            {
                return Json(new { success = false, message = "Không được thêm bảng kê từ chi nhánh khác!" });
            }
            var TenNguoiNopTien = _dbContext.SysUser.FirstOrDefault(x => x.Ma == NguoiNopTien.GetGuid());
            var listBangKe = new FiaBangKeNopTien
            {
                Ma = Ma.GetGuid(),
                MaChiNhanhNhan = MaChiNhanhNhan.GetGuid(),
                MaChiNhanhNop = MaChiNhanhNop.GetGuid(),
                MaPhongBanNhan = MaPhongBanNhan.GetGuid(),
                MaPhongBanNop = MaPhongBanNop.GetGuid(),
                NgayLap = NgayLap,
                NgayNopTien = NgayNopTien,
                NguoiNopTien = NguoiNopTien.GetGuid(),
                TenNguoiNopTien = TenNguoiNopTien.FullName,
                DiaChi = DiaChi,
                LyDo = LyDo,
                MaTienTe = MaTienTe.GetGuid(),
                TyGia = TyGia,
                SoTien = SoTien,
                GhiChu = GhiChu,
                NguoiNhanTien = NguoiNhanTien.GetGuid(),
                MaNoiDung = MaNoiDung.GetGuid(),
                MaHinhThuc = MaHinhThuc,
                SoPhieu = string.Format("BK/{0}/{1:yyyyMMdd}/{2:000}", cn.Code, DateTime.Today, nextSequentialNumber),
                UserCreated = loggedInUser.Ma,
                TrangThai = (int)TrangThaiChungTu.LapPhieu,
                CreatedDate = DateTime.Now
            };
            _dbContext.FiaBangKeNopTien.Add(listBangKe);
            _dbContext.SaveChanges();
            foreach (var chiTiet in DataChiTietBangKe)
            {
                var listChiTietBangKe = new FiaChiTietBangKeNopTien
                {
                    Ma = chiTiet.Ma,
                    MaBangKeNopTien = listBangKe.Ma,
                    MaLoaiTien = listBangKe.MaTienTe,
                    SoLuong = chiTiet.SoLuong,
                    ThanhTien = chiTiet.ThanhTien,
                    GhiChu = chiTiet.GhiChu,
                    UserCreated = loggedInUser.Ma,
                    CreatedDate = DateTime.Now
                };

                _dbContext.FiaChiTietBangKeNopTien.Add(listChiTietBangKe);
            }
            foreach (var chiTiet in DataNhanVien)
            {
                var listChiTietBangKe = new FiaChiTietBangKeNhanVien
                {
                    Ma = chiTiet.Ma,
                    MaBangKe = listBangKe.Ma,
                    MaNhanVien = chiTiet.MaNhanVien,
                    SoTien = chiTiet.SoTien,
                    UserCreated = loggedInUser.Ma,
                    CreatedDate = DateTime.Now
                };

                _dbContext.FiaChiTietBangKeNhanVien.Add(listChiTietBangKe);
            }

            _dbContext.SaveChanges();


            // Trả về kết quả thành công
            return Json(new { success = true, message = "Thêm thành công!" });
        }

        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listTienTe"] = _dbContext.FiaTienTe.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNoiDung"] = _dbContext.CatNoiDungThuChi.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNguoiNopTien"] = _dbContext.SysUser.Where(x => !(x.Deleted ?? false)).ToList();
            var BangKe = _dbContext.FiaBangKeNopTien.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (BangKe == null)
            {
                return NotFound();
            }
            return View("Form", BangKe);
        }
        [HttpPost]
        public JsonResult Edit(
       string Ma,
       string MaChiNhanhNhan,
       string MaChiNhanhNop,
       string MaPhongBanNhan,
       string MaPhongBanNop,
       DateTime NgayNopTien,
       DateTime NgayLap,
       int MaHinhThuc,
       string MaTienTe,
       decimal TyGia,
       string NguoiNopTien,
       string MaNoiDung,
       string GhiChu,
       string DiaChi,
       string LyDo,
       int SoTien,
       string NguoiNhanTien,
    List<FiaChiTietBangKeNopTien> DataChiTietBangKe,
    List<FiaChiTietBangKeNhanVien> DataNhanVien
   )
        {

            SysBranch? cn = _dbContext.SysBranch.FirstOrDefault(t => t.Ma == MaChiNhanhNop.GetGuid());
            if (cn == null)
            {
                return Json(new { success = false, message = "Chi nhánh không tồn tại!" });
            }

            int nextSequentialNumber = GetNextSequentialNumber(cn.Code);
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

            var existingListDocumentType = _dbContext.FiaBangKeNopTien.FirstOrDefault(x => x.MaChiNhanhNop == loggedInUser.Ma);
            if (existingListDocumentType != null)
            {
                return Json(new { success = false, message = "Không được thêm bảng kê từ chi nhánh khác!" });
            }
            var TenNguoiNopTien = _dbContext.SysUser.FirstOrDefault(x => x.Ma == NguoiNopTien.GetGuid());
            var listBangKe = new FiaBangKeNopTien
            {
                Ma = Ma.GetGuid(),
                MaChiNhanhNhan = MaChiNhanhNhan.GetGuid(),
                MaChiNhanhNop = MaChiNhanhNop.GetGuid(),
                MaPhongBanNhan = MaPhongBanNhan.GetGuid(),
                MaPhongBanNop = MaPhongBanNop.GetGuid(),
                NgayLap = NgayLap,
                NgayNopTien = NgayNopTien,
                NguoiNopTien = NguoiNopTien.GetGuid(),
                TenNguoiNopTien = TenNguoiNopTien.FullName,
                DiaChi = DiaChi,
                LyDo = LyDo,
                MaTienTe = MaTienTe.GetGuid(),
                TyGia = TyGia,
                SoTien = SoTien,
                GhiChu = GhiChu,
                NguoiNhanTien = NguoiNhanTien.GetGuid(),
                MaNoiDung = MaNoiDung.GetGuid(),
                MaHinhThuc = MaHinhThuc,
                SoPhieu = string.Format("BK/{0}/{1:yyyyMMdd}/{2:000}", cn.Code, DateTime.Today, nextSequentialNumber),
                UserModified = loggedInUser.Ma,
                CreatedDate = DateTime.Now,
                UserCreated = loggedInUser.Ma,
                TrangThai = (int)TrangThaiChungTu.LapPhieu,
                ModifiedDate = DateTime.Now
            };
            _dbContext.FiaBangKeNopTien.Update(listBangKe);
            _dbContext.SaveChanges();
            foreach (var chiTiet in DataChiTietBangKe)
            {
                var listChiTietBangKe = new FiaChiTietBangKeNopTien
                {
                    Ma = chiTiet.Ma,
                    MaBangKeNopTien = listBangKe.Ma,
                    MaLoaiTien = listBangKe.MaTienTe,
                    SoLuong = chiTiet.SoLuong,
                    ThanhTien = chiTiet.ThanhTien,
                    GhiChu = chiTiet.GhiChu,
                    CreatedDate = DateTime.Now,
                    UserCreated = loggedInUser.Ma,
                    UserModified = loggedInUser.Ma,
                    ModifiedDate = DateTime.Now
                };

                _dbContext.FiaChiTietBangKeNopTien.Update(listChiTietBangKe);
            }
            foreach (var chiTiet in DataNhanVien)
            {
                var listChiTietBangKe = new FiaChiTietBangKeNhanVien
                {
                    Ma = chiTiet.Ma,
                    MaBangKe = listBangKe.Ma,
                    MaNhanVien = chiTiet.MaNhanVien,
                    SoTien = chiTiet.SoTien,
                    CreatedDate = DateTime.Now,
                    UserCreated = loggedInUser.Ma,
                    UserModified = loggedInUser.Ma,
                    ModifiedDate = DateTime.Now
                };

                _dbContext.FiaChiTietBangKeNhanVien.Update(listChiTietBangKe);
            }

            _dbContext.SaveChanges();


            // Trả về kết quả thành công
            return Json(new { success = true, message = "Thêm thành công!" });
        }

        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var CashDeposit = _dbContext.FiaBangKeNopTien.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (CashDeposit == null)
            {
                return Json(new { success = false, message = "Bảng kê không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            CashDeposit.Deleted = true;  // Đánh dấu đã bị xoá
            CashDeposit.DeletedDate = DateTime.Now;  // Lưu thời gian xoá
            CashDeposit.UserDeleted = loggedInUser.Ma;
            _dbContext.FiaBangKeNopTien.Update(CashDeposit);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }

}
