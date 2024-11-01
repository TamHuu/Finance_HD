﻿using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_HD.Controllers.ChungTu
{
    public class CashDepositController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CashDepositController(ILogger<CashDepositController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public long CovertLong(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            if (long.TryParse(input, out long result))
            {
                return result;
            }

            return 0;
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
            DateTime? dtpTuNgay = null;
            DateTime? dtpDenNgay = null;

            // Chuyển đổi TuNgay và DenNgay nếu có giá trị
            if (!string.IsNullOrEmpty(TuNgay))
                dtpTuNgay = TuNgay.ToDateTime2(DateTime.Now);
            if (!string.IsNullOrEmpty(DenNgay))
                dtpDenNgay = DenNgay.ToDateTime2(DateTime.Now);

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
                           (!dtpTuNgay.HasValue || bangkenoptien.NgayLap >= dtpTuNgay) &&
                           (!dtpDenNgay.HasValue || bangkenoptien.NgayLap <= dtpDenNgay)
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
                                     }).OrderByDescending(x => x.TenLoaiTien).ToList();
            return Json(new { success = true, Data = listChiTietBangKe });
        }
        [HttpPost]
        public JsonResult getChiTietNhanVien(string Ma)
        {
            var listChiTietNhanVien = (from bangkenhanvien in _dbContext.FiaChiTietBangKeNhanVien
                                       join nhanvien in _dbContext.SysUser
                                       on bangkenhanvien.MaNhanVien equals nhanvien.Ma into nhanVienGroup
                                       from nhanvien in nhanVienGroup.DefaultIfEmpty() // Left Join

                                       join bangkenoptien in _dbContext.FiaChiTietBangKeNopTien
                                       on bangkenhanvien.MaBangKe equals bangkenoptien.Ma into bangKeNopTienGroup
                                       from bangkenoptien in bangKeNopTienGroup.DefaultIfEmpty() // Left Join
                                       where bangkenhanvien.MaBangKe == Ma.GetGuid()
                             && !(bangkenhanvien.Deleted ?? false)
                                       select new
                                       {
                                           TenNhanVien = nhanvien.FullName + "",
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
            var maChiTietBangKe = _dbContext.FiaChiTietBangKeNopTien
                            .Select(x => x.Ma)
                            .FirstOrDefault();
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            ViewData["TaiKhoanDangNhap"] = loggedInUser;
            ViewData["listNoiDung"] = _dbContext.CatNoiDungThuChi.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNguoiNopTien"] = _dbContext.SysUser.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNhanVien"] = _dbContext.SysUser.Where(x => !(x.Deleted ?? false)).ToList();

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
       string SoTien,
       string NguoiNhanTien,
       string KhachHang,
    List<FiaChiTietBangKeNopTien> SaveDataBangKe,
    List<FiaChiTietBangKeNhanVien> SaveDataNhanVien
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
                MaTienTe = MaTienTe.GetGuid(),
                TyGia = TyGia,
                SoTien = CovertLong(SoTien),
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
            foreach (var chiTiet in SaveDataBangKe)
            {
                var listChiTietBangKe = new FiaChiTietBangKeNopTien
                {
                    Ma = chiTiet.Ma,
                    MaBangKeNopTien = listBangKe.Ma,
                    MaLoaiTien = chiTiet.MaLoaiTien,
                    SoLuong = chiTiet.SoLuong,
                    ThanhTien = chiTiet.ThanhTien,
                    GhiChu = chiTiet.GhiChu,
                    UserCreated = loggedInUser.Ma,
                    CreatedDate = DateTime.Now
                };

                _dbContext.FiaChiTietBangKeNopTien.Add(listChiTietBangKe);
            }
            foreach (var nhanVien in SaveDataNhanVien)
            {
                var BangKeNhanVien = new FiaChiTietBangKeNhanVien
                {
                    Ma = nhanVien.Ma,
                    MaBangKe = listBangKe.Ma,
                    MaNhanVien = nhanVien.MaNhanVien,
                    SoTien = nhanVien.SoTien,
                    UserCreated = loggedInUser.Ma,
                    CreatedDate = DateTime.Now
                };

                _dbContext.FiaChiTietBangKeNhanVien.Add(BangKeNhanVien);
            }

            _dbContext.SaveChanges();


            // Trả về kết quả thành công
            return Json(new { success = true, message = "Thêm thành công!" });
        }

        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            ViewData["TaiKhoanDangNhap"] = loggedInUser;
            ViewData["listTienTe"] = _dbContext.FiaTienTe.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listLoaiTien"] = _dbContext.FaLoaiTien.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNoiDung"] = _dbContext.CatNoiDungThuChi.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNguoiNopTien"] = _dbContext.SysUser.Where(x => !(x.Deleted ?? false)).ToList();
            ViewData["listNhanVien"] = _dbContext.SysUser.Where(x => !(x.Deleted ?? false)).ToList();

            var BangKe = _dbContext.FiaBangKeNopTien.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (BangKe == null)
            {
                return NotFound();
            }

            var DataChiTietBangKe = _dbContext.FiaChiTietBangKeNopTien.Where(x => x.MaBangKeNopTien == BangKe.Ma).ToList();
            var DataNhanVien = _dbContext.FiaChiTietBangKeNhanVien.Where(x => x.MaBangKe == BangKe.Ma).ToList();

            // Passing data to the view
            ViewBag.DataChiTietBangKe = DataChiTietBangKe;
            ViewBag.DataNhanVien = DataNhanVien;
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
            string SoTien,
            string NguoiNhanTien,
            List<FiaChiTietBangKeNopTien> SaveDataBangKe,
            List<FiaChiTietBangKeNhanVien> SaveDataNhanVien
        )
        {
            SysBranch? cn = _dbContext.SysBranch.FirstOrDefault(t => t.Ma == MaChiNhanhNop.GetGuid());
            if (cn == null)
            {
                return Json(new { success = false, message = "Chi nhánh không tồn tại!" });
            }

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

            var listBangKe = _dbContext.FiaBangKeNopTien.FirstOrDefault(x => x.Ma == Ma.GetGuid());
            if (listBangKe == null)
            {
                return Json(new { success = false, message = "Không tìm thấy bảng kê để cập nhật!" });
            }

            // Cập nhật các thuộc tính
            listBangKe.MaChiNhanhNhan = MaChiNhanhNhan.GetGuid();
            listBangKe.MaChiNhanhNop = MaChiNhanhNop.GetGuid();
            listBangKe.MaPhongBanNhan = MaPhongBanNhan.GetGuid();
            listBangKe.MaPhongBanNop = MaPhongBanNop.GetGuid();
            listBangKe.NgayLap = NgayLap;
            listBangKe.NgayNopTien = NgayNopTien;
            listBangKe.NguoiNopTien = NguoiNopTien.GetGuid();
            listBangKe.DiaChi = DiaChi;
            listBangKe.LyDo = LyDo;
            listBangKe.MaTienTe = MaTienTe.GetGuid();
            listBangKe.TyGia = TyGia;
            listBangKe.SoTien = CovertLong(SoTien);
            listBangKe.GhiChu = GhiChu;
            listBangKe.NguoiNhanTien = NguoiNhanTien.GetGuid();
            listBangKe.MaNoiDung = MaNoiDung.GetGuid();
            listBangKe.MaHinhThuc = MaHinhThuc;
            listBangKe.ModifiedDate = DateTime.Now;
            listBangKe.UserModified = loggedInUser?.Ma;

            foreach (var chiTiet in SaveDataBangKe)
            {
                var listChiTietBangKe = _dbContext.FiaChiTietBangKeNopTien.FirstOrDefault(x => x.Ma == chiTiet.Ma);
                if (listChiTietBangKe != null)
                {
                    listChiTietBangKe.MaLoaiTien = listBangKe.MaTienTe;
                    listChiTietBangKe.SoLuong = chiTiet.SoLuong;
                    listChiTietBangKe.ThanhTien = chiTiet.ThanhTien;
                    listChiTietBangKe.GhiChu = chiTiet.GhiChu;
                    listChiTietBangKe.ModifiedDate = DateTime.Now;
                    listChiTietBangKe.UserModified = loggedInUser?.Ma;
                }
            }

            foreach (var chiTiet in SaveDataNhanVien)
            {
                var listChiTietBangKe = _dbContext.FiaChiTietBangKeNhanVien.FirstOrDefault(x => x.Ma == chiTiet.Ma);
                if (listChiTietBangKe != null)
                {
                    listChiTietBangKe.MaNhanVien = chiTiet.MaNhanVien;
                    listChiTietBangKe.SoTien = chiTiet.SoTien;
                    listChiTietBangKe.ModifiedDate = DateTime.Now;
                    listChiTietBangKe.UserModified = loggedInUser?.Ma;
                }
            }

            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật thành công!" });
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
        [HttpPost]

        public async Task<JsonResult> getCashDepositById(string ma)
        {
            if (string.IsNullOrEmpty(ma))
            {
                return Json(new { success = false, message = "Mã không hợp lệ." });
            }

            // Kiểm tra chuyển đổi `ma` sang `Guid`
            var maGuid = ma.GetGuid();
            if (maGuid == null)
            {
                return Json(new { success = false, message = "Mã không hợp lệ." });
            }



            var cashDeposit = await (from bangke in _dbContext.FiaBangKeNopTien
                                     join tiente in _dbContext.FiaTienTe
                                     on bangke.MaTienTe equals tiente.Ma
                                     where bangke.Ma == maGuid
                                     select new
                                     {
                                         Ma = bangke.Ma,
                                         MaChiNhanhNhan = bangke.MaChiNhanhNhan,
                                         MaChiNhanhNop = bangke.MaChiNhanhNop,
                                         MaPhongBanNhan = bangke.MaPhongBanNhan,
                                         MaPhongBanNop = bangke.MaPhongBanNop,
                                         NgayLap = bangke.NgayLap,
                                         NgayNopTien = bangke.NgayNopTien,
                                         NguoiNopTien = bangke.NguoiNopTien,
                                         TenNguoiNopTien = bangke.TenNguoiNopTien,
                                         DiaChi = bangke.DiaChi,
                                         LyDo = bangke.LyDo,
                                         MaTienTe = tiente.Ma,
                                         TenTienTe = tiente.Ten,
                                         TyGia = bangke.TyGia,
                                         SoTien = bangke.SoTien,
                                         GhiChu = bangke.GhiChu,
                                         NguoiNhanTien = bangke.NguoiNhanTien,
                                         MaNoiDung = bangke.MaNoiDung,
                                         MaHinhThuc = bangke.MaHinhThuc,
                                         SoPhieu = bangke.SoPhieu,
                                         TrangThai = bangke.TrangThai,
                                         CreatedDate = bangke.CreatedDate,
                                     }).OrderByDescending(x => x.CreatedDate).ToListAsync();

            // Truy vấn chi tiết bảng kê và loại tiền
            var listChiTietBangKe = await (from bangke in _dbContext.FiaChiTietBangKeNopTien
                                           join loaitien in _dbContext.FaLoaiTien
                                           on bangke.MaLoaiTien equals loaitien.Ma
                                           where bangke.MaBangKeNopTien == maGuid
                                           select new
                                           {
                                               Ma = bangke.Ma,
                                               MaBangKeNopTien = bangke.MaBangKeNopTien,
                                               MaLoaiTien = bangke.MaLoaiTien,
                                               SoLuong = bangke.SoLuong,
                                               ThanhTien = bangke.ThanhTien,
                                               GhiChu = bangke.GhiChu,
                                               TenLoaiTien = loaitien.GiaTri
                                           }).OrderByDescending(x => x.TenLoaiTien).ToListAsync();

            // Truy vấn nhân viên
            var listNhanVien = await (from nhanvien in _dbContext.FiaChiTietBangKeNhanVien
                                      join user in _dbContext.SysUser
                                      on nhanvien.MaNhanVien equals user.Ma
                                      where nhanvien.MaBangKe == maGuid
                                      select new
                                      {
                                          Ma = nhanvien.Ma,
                                          MaBangKe = nhanvien.MaBangKe,
                                          MaNhanVien = nhanvien.MaNhanVien,
                                          SoTien = nhanvien.SoTien,
                                          TenNhanVien = user.FullName,
                                          CreatedDate = nhanvien.CreatedDate,
                                      }).OrderByDescending(x => x.CreatedDate).ToListAsync();
            // Đóng gói dữ liệu trả về
            var responseData = new
            {
                cashDeposit,
                ChiTietBangKe = listChiTietBangKe,
                NhanVien = listNhanVien
            };

            return Json(new { success = true, data = responseData });
        }


    }

}
