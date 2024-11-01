using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.NganHang
{
    public class BankController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BankController(ILogger<BankController> logger, ApplicationDbContext context)
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
            var result = _dbContext.FiaNganHang.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListBank()
        {
            var listBank = _dbContext.FiaNganHang
                             .Where(role => !(role.Deleted ?? false))
                             .OrderByDescending(role => role.CreatedDate)
                             .ToList();

            return Json(new { success = true, Data = listBank });
        }

        public IActionResult Add()
        {
            return View("Form", new FiaNganHang());
        }

        [HttpPost]
        public JsonResult Add(string Code, string Ten, string CodeVa, string MaxLengthVa, bool Status)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(Ten))
            {
                return Json(new { success = false, message = "Tên ngân hàng không được để trống!" });
            }

            var existingListBank = _dbContext.FiaTienTe.FirstOrDefault(x => x.Ten == Ten);
            if (existingListBank != null)
            {
                return Json(new { success = false, message = "Tên ngân hàng đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var Bank = new FiaNganHang
            {
                Ten = Ten,
                Code = Code,
                CodeVa = CodeVa,
                MaxLengthVa = MaxLengthVa.ToInt(),
                Status = Status,
                CreatedDate = DateTime.Now,
                UserCreated = loggedInUser.Ma,
            };

            _dbContext.FiaNganHang.Add(Bank);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var listBank = _dbContext.FiaNganHang.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (listBank == null)
            {
                return NotFound();
            }
            return View("Form", listBank);
        }
        [HttpPost]
        public JsonResult Edit(FiaNganHang model)
        {
            var listBank = _dbContext.FiaNganHang.FirstOrDefault(x => x.Ma == model.Ma);
          
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            listBank.Ten = model.Ten;
            listBank.Code = model.Code;
            listBank.Status = model.Status;
            listBank.UserModified = loggedInUser.Ma;
            listBank.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.FiaNganHang.Update(listBank);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var listBank = _dbContext.FiaNganHang.FirstOrDefault(x => x.Ma == Id.GetGuid());
          
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            listBank.UserDeleted = loggedInUser.Ma;
            listBank.Deleted = true;  // Đánh dấu đã bị xoá
            listBank.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FiaNganHang.Update(listBank);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }

    }
}
