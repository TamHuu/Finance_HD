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
                              on bangkenoptien.MaNoiDung equals noidung.Ma

                              join hinhthuc in _dbContext.TblHinhThucThuChi
                              on bangkenoptien.MaHinhThuc equals hinhthuc.Ma

                              join nguoinoptien in _dbContext.SysUser
                              on bangkenoptien.NguoiNopTien equals nguoinoptien.Ma

                              join nguoinhantien in _dbContext.SysUser
                              on bangkenoptien.NguoiNhanTien equals nguoinhantien.Ma

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
                                  TrangThai = bangkenoptien.TrangThai + "",
                                  CreatedDate = DateTime.Now,
                                  HinhThuc = hinhthuc.Ten + "",
                              }).ToList();
            return Json(new { success = true, Data = listBangKe });
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["listTienTe"] = _dbContext.FiaTienTe.ToList();
            ViewData["listNoiDung"] = _dbContext.CatNoiDungThuChi.ToList();
            ViewData["listNguoiNopTien"] = _dbContext.SysUser.ToList();
            return View("Form", new FiaBangKeNopTien());
        }
        [HttpPost]
        public JsonResult Add(FiaBangKeNopTien model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            var existingListDocumentType = _dbContext.FiaBangKeNopTien.FirstOrDefault(x => x.MaChiNhanhNop == loggedInUser.Ma);
            if (existingListDocumentType != null)
            {
                return Json(new { success = false, message = "Không được thêm bảng kê từ chi nhánh khác!" });
            }

            var listBangKe = new FiaBangKeNopTien
            {

                Ma = model.Ma,
                MaChiNhanhNhan = model.MaChiNhanhNhan,
                MaChiNhanhNop = model.MaChiNhanhNop,
                MaPhongBanNhan = model.MaPhongBanNhan,
                MaPhongBanNop = model.MaPhongBanNop,
                SoPhieu = model.SoPhieu,
                NgayLap = model.NgayLap,
                NgayNopTien = model.NgayNopTien,
                NguoiNopTien = model.NguoiNopTien,
                TenNguoiNopTien = model.TenNguoiNopTien,
                DiaChi = model.DiaChi,
                LyDo = model.LyDo,
                MaTienTe = model.MaTienTe,
                TyGia = model.TyGia,
                SoTien = model.SoTien,
                GhiChu = model.GhiChu,
                NguoiNhanTien = model.NguoiNhanTien,
                MaNoiDung = model.MaNoiDung,
                TrangThai = model.TrangThai,
                MaHinhThuc = model.MaHinhThuc,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,
            };

            _dbContext.FiaBangKeNopTien.Add(listBangKe);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Thêm thành công!" });
        }

    }

}