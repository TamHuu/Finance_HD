using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.TaiChinh
{
    public class IncomeExpenseTypeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public IncomeExpenseTypeController(ILogger<IncomeExpenseTypeController> logger, ApplicationDbContext context)
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
        [HttpGet]
        public JsonResult getListIncomeExpenseType()
        {
            var listIncomeExpenseType = _dbContext.CatLoaiThuChi
                             .Where(role => !(role.Deleted ?? false))
                             .OrderByDescending(role => role.CreatedDate)
                             .ToList();

            return Json(new { success = true, Data = listIncomeExpenseType });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Form", new CatLoaiThuChi());
        }
        [HttpPost]
        public JsonResult Add(CatLoaiThuChi model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }
            var existingIncomeExpenseType = _dbContext.CatLoaiThuChi.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingIncomeExpenseType != null)
            {
                return Json(new { success = false, message = "Loại thu chi này đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var IncomeExpenseType = new CatLoaiThuChi
            {
                
                Code = model.Code,
                Ten = model.Ten,
                Status = model.Status,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,
            };

            _dbContext.CatLoaiThuChi.Add(IncomeExpenseType);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Loại thu chi đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var IncomeExpenseType = _dbContext.CatLoaiThuChi.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (IncomeExpenseType == null)
            {
                return NotFound();
            }
            return View("Form", IncomeExpenseType);
        }
        [HttpPost]
        public JsonResult Edit(CatLoaiThuChi model)
        {
            var IncomeExpenseType = _dbContext.CatLoaiThuChi.FirstOrDefault(x => x.Ma == model.Ma);
            if (IncomeExpenseType == null)
            {
                return Json(new { success = false, message = "Loại thu chi này không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            IncomeExpenseType.UserModified = loggedInUser.Ma;
            IncomeExpenseType.Ma = model.Ma;
            IncomeExpenseType.Code = model.Code;
            IncomeExpenseType.Ten = model.Ten;
            IncomeExpenseType.Status = model.Status;
            IncomeExpenseType.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.CatLoaiThuChi.Update(IncomeExpenseType);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật Loại thu chi thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var IncomeExpenseType = _dbContext.CatLoaiThuChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (IncomeExpenseType == null)
            {
                return Json(new { success = false, message = "Loại thu chi không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            IncomeExpenseType.Deleted = true;  // Đánh dấu đã bị xoá
            IncomeExpenseType.DeletedDate = DateTime.Now;  // Lưu thời gian xoá
            IncomeExpenseType.UserDeleted = loggedInUser.Ma;
            _dbContext.CatLoaiThuChi.Update(IncomeExpenseType);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }
}
