using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.QuanLyHeThong
{
    public class PermissionsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PermissionsController(ILogger<PermissionsController> logger, ApplicationDbContext context)
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
            var result = _dbContext.SysPermission.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListPermission()
        {
            var listPermission = _dbContext.SysPermission.ToList();
            return Json(new { Data = listPermission });
        }

        public IActionResult Add()
        {
            return View("Form", new SysPermission());
        }

        [HttpPost]
        public JsonResult Add(SysPermission model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return Json(new { success = false, message = "Tên quyền không được để trống!" });
            }

            var existingPermission = _dbContext.SysPermission.FirstOrDefault(x => x.Name == model.Name);
            if (existingPermission != null)
            {
                return Json(new { success = false, message = "Tên quyền đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var permission = new SysPermission
            {
               Name = model.Name,
               Code = model.Code,
               Description = model.Description,
               FormAccess = model.FormAccess,
               Status = model.Status,
               CreatedDate = model.CreatedDate ?? DateTime.Now,
               UserCreated = loggedInUser.Ma,
            };

            _dbContext.SysPermission.Add(permission);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Quyền đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var permission = _dbContext.SysPermission.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (permission == null)
            {
                return NotFound();
            }
            return View("Form", permission);
        }
        [HttpPost]
        public JsonResult Edit(SysPermission model)
        {
            var permission = _dbContext.SysPermission.FirstOrDefault(x => x.Ma == model.Ma);
            if (permission == null)
            {
                return Json(new { success = false, message = "Quyền này không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            permission.Name = model.Name;
            permission.Code = model.Code;
            permission.Description = model.Description;
            permission.FormAccess = model.FormAccess;
            permission.Status = model.Status;
            permission.UserModified = loggedInUser.Ma;
            permission.ModifiedDate = permission.ModifiedDate ?? DateTime.Now;
            _dbContext.SysPermission.Update(permission);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật quyền thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var permission = _dbContext.SysPermission.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (permission == null)
            {
                return Json(new { success = false, message = "Quyền không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            permission.UserDeleted = loggedInUser.Ma;   
            permission.Deleted = true;  // Đánh dấu đã bị xoá
            permission.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.SysPermission.Update(permission);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá quyền thành công!" });
        }

    }
}
