using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.NganHang
{
    public class BankAccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BankAccountController(ILogger<BankAccountController> logger, ApplicationDbContext context)
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
            var result = _dbContext.FiaTaiKhoanNganHang.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListBankAccount()
        {
            var listBankAccount = _dbContext.FiaTaiKhoanNganHang
                             .Where(role => !(role.Deleted ?? false))
                             .OrderByDescending(role => role.CreatedDate)
                             .ToList();

            return Json(new { success = true, Data = listBankAccount });
        }

        public IActionResult Add()
        {
            ViewBag.DongTienThu = _dbContext.CatNoiDungThuChi.ToList();
            ViewBag.DongTienChi = _dbContext.CatNoiDungThuChi.ToList();
            return View("Form", new FiaTaiKhoanNganHang());
        }

        [HttpPost]
        public JsonResult Add(string ChiNhanh, string PhongBan, string NganHang, string TienTe, string SoTaiKhoan, string DienGiai, string DongTienThu, string DongTienChi, bool Status, string LoaiTaiKhoan)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(ChiNhanh))
            {
                return Json(new { success = false, message = "Tên chi nhánh không được để trống!" });
            }

            var existingListBankAccount = _dbContext.FiaTaiKhoanNganHang.FirstOrDefault(x => x.SoTaiKhoan == SoTaiKhoan);
            if (existingListBankAccount != null)
            {
                return Json(new { success = false, message = "Số tài khoản ngân hàng đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var BankAccount = new FiaTaiKhoanNganHang
            {
                MaChiNhanh=ChiNhanh.GetGuid(),
                MaPhongBan = PhongBan.GetGuid(),
                MaNganHang= NganHang.GetGuid(),
                MaTienTe= TienTe.GetGuid(),
                SoTaiKhoan= SoTaiKhoan,
                MaLoai = LoaiTaiKhoan.GetGuid(),
                DienGiai = DienGiai,
                DongTienThu = DongTienThu.GetGuid(),
                DongTienChi = DongTienChi.GetGuid(),
                Status = Status,
                CreatedDate = DateTime.Now,
                UserCreated = loggedInUser.Ma,
            };

            _dbContext.FiaTaiKhoanNganHang.Add(BankAccount);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var listBankAccount = _dbContext.FiaTaiKhoanNganHang.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (listBankAccount == null)
            {
                return NotFound();
            }
            return View("Form", listBankAccount);
        }
        [HttpPost]
        //public JsonResult Edit(FiaTaiKhoanNganHang model)
        //{
        //    var listBankAccount = _dbContext.FiaTaiKhoanNganHang.FirstOrDefault(x => x.Ma == model.Ma);
        //    if (listBankAccount == null)
        //    {
        //        return Json(new { success = false, message = "Tiền tệ này không tồn tại!" });
        //    }
        //    string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

        //    var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
        //    if (loggedInUser == null)
        //    {
        //        return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
        //    }

        //    listBankAccount.Ten = model.Ten;
        //    listBankAccount.Code = model.Code;
        //    listBankAccount.Status = model.Status;
        //    listBankAccount.UserModified = loggedInUser.Ma;
        //    listBankAccount.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
        //    _dbContext.FiaTaiKhoanNganHang.Update(listBankAccount);
        //    _dbContext.SaveChanges();

        //    return Json(new { success = true, message = "Cập nhật tiền tệ thành công!" });
        //}
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var listBankAccount = _dbContext.FiaTaiKhoanNganHang.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (listBankAccount == null)
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
            listBankAccount.UserDeleted = loggedInUser.Ma;
            listBankAccount.Deleted = true;  // Đánh dấu đã bị xoá
            listBankAccount.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FiaTaiKhoanNganHang.Update(listBankAccount);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá tiền tệ thành công!" });
        }

    }
}
