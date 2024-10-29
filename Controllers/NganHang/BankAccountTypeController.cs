using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.NganHang
{
    public class BankAccountTypeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BankAccountTypeController(ILogger<BankAccountTypeController> logger, ApplicationDbContext context)
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
            var result = _dbContext.FiaLoaiTaiKhoanNganHang.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListBankAccountType()
        {
            var listBankAccountType = (from loaitaikhoan in _dbContext.FiaLoaiTaiKhoanNganHang
                                       join nganhang in _dbContext.FiaNganHang
                                       on loaitaikhoan.MaNganHang equals nganhang.Ma
                                       where !(loaitaikhoan.Deleted ?? false) // Kiểm tra điều kiện Deleted từ FiaLoaiTaiKhoanNganHang
                                       select new
                                       {
                                           Ma = loaitaikhoan.Ma + "",
                                           MaNganHang = nganhang.Ma + "",
                                           Code = loaitaikhoan.Code + "",
                                           Ten = loaitaikhoan.Ten + "",
                                           Status = loaitaikhoan.Status,
                                           TenNganHang = nganhang.Ten + "",
                                           CreatedDate = loaitaikhoan.CreatedDate,
                                       })
                                     .OrderByDescending(role => role.CreatedDate)
                                     .ToList();

            return Json(new { success = true, Data = listBankAccountType });
        }


        public IActionResult Add()
        {
            ViewBag.NganHang = _dbContext.FiaNganHang.ToList();
            return View("Form", new FiaLoaiTaiKhoanNganHang());
        }

        [HttpPost]
        public JsonResult Add(string Code, string Ten, string NganHang, bool Status)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(Ten))
            {
                return Json(new { success = false, message = "Tên ngân hàng không được để trống!" });
            }

            var existingListBankAccountType = _dbContext.FiaTienTe.FirstOrDefault(x => x.Ten == Ten);
            if (existingListBankAccountType != null)
            {
                return Json(new { success = false, message = "Tên ngân hàng đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var BankAccountType = new FiaLoaiTaiKhoanNganHang
            {
                Ten = Ten,
                Code = Code,
                MaNganHang = NganHang.GetGuid(),
                Status = Status,
                CreatedDate = DateTime.Now,
                UserCreated = loggedInUser.Ma,
            };

            _dbContext.FiaLoaiTaiKhoanNganHang.Add(BankAccountType);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var listBankAccountType = _dbContext.FiaLoaiTaiKhoanNganHang.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (listBankAccountType == null)
            {
                return NotFound();
            }
            return View("Form", listBankAccountType);
        }
        [HttpPost]
        public JsonResult Edit(FiaLoaiTaiKhoanNganHang model)
        {
            var listBankAccountType = _dbContext.FiaLoaiTaiKhoanNganHang.FirstOrDefault(x => x.Ma == model.Ma);
            if (listBankAccountType == null)
            {
                return Json(new { success = false, message = "Tiền tệ này không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            listBankAccountType.Ten = model.Ten;
            listBankAccountType.Code = model.Code;
            listBankAccountType.Status = model.Status;
            listBankAccountType.UserModified = loggedInUser.Ma;
            listBankAccountType.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.FiaLoaiTaiKhoanNganHang.Update(listBankAccountType);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật tiền tệ thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var listBankAccountType = _dbContext.FiaLoaiTaiKhoanNganHang.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (listBankAccountType == null)
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
            listBankAccountType.UserDeleted = loggedInUser.Ma;
            listBankAccountType.Deleted = true;  // Đánh dấu đã bị xoá
            listBankAccountType.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FiaLoaiTaiKhoanNganHang.Update(listBankAccountType);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá tiền tệ thành công!" });
        }

    }
}

