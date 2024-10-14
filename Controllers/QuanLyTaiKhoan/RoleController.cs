using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.QuanLyTaiKhoan
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleController(ILogger<RoleController> logger, ApplicationDbContext context)
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
            var result = _dbContext.SysRole.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListRole()
        {
            var listRole = _dbContext.SysRole
                             .Where(role => !(role.Deleted ?? false))
                             .OrderByDescending(role => role.CreatedDate) 
                             .ToList();

            return Json(new { success = true, Data = listRole });
        }

        public IActionResult Add()
        {
            return View("Form", new SysRole());
        }

        [HttpPost]
        public JsonResult Add(SysRole model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return Json(new { success = false, message = "Tên role không được để trống!" });
            }

            var existingRole = _dbContext.SysRole.FirstOrDefault(x => x.Name == model.Name);
            if (existingRole != null)
            {
                return Json(new { success = false, message = "Tên Role đã tồn tại!" });
            }

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var role = new SysRole
            {
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
                Status = model.Status,
                CreatedDate = model.CreatedDate ?? DateTime.Now,
                UserModified = loggedInUser.Ma,
            };

            _dbContext.SysRole.Add(role);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Role đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var role = _dbContext.SysRole.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (role == null)
            {
                return NotFound();
            }
            return View("Form", role);
        }
        [HttpPost]
        public JsonResult Edit(SysRole model)
        {
            var role = _dbContext.SysRole.FirstOrDefault(x => x.Ma == model.Ma);
            if (role == null)
            {
                return Json(new { success = false, message = "Role này không tồn tại!" });
            }

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            role.Name = model.Name;
            role.Code = model.Code;
            role.Description = model.Description;
            role.Status = model.Status;
            role.UserModified = model.UserModified;
            role.ModifiedDate = role.ModifiedDate ?? DateTime.Now;
            role.UserModified = loggedInUser.Ma;
            _dbContext.SysRole.Update(role);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật role thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var role = _dbContext.SysRole.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (role == null)
            {
                return Json(new { success = false, message = "Role không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            // Kích hoạt soft delete
            role.Deleted = true;  // Đánh dấu đã bị xoá
            role.DeletedDate = DateTime.Now;  // Lưu thời gian xoá
            role.UserDeleted = loggedInUser.Ma;
            role.DeletedDate= DateTime.Now;
            _dbContext.SysRole.Update(role);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá role thành công!" });
        }

    }
}
