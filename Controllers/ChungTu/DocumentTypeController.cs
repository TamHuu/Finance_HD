using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.ChungTu
{
    public class DocumentType : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public DocumentType(ILogger<DocumentType> logger, ApplicationDbContext context)
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
        public JsonResult getListDocumentType()
        {
            var listDocumentType = _dbContext.TblLoaiChungTu
                               .Where(role => !(role.Deleted ?? false))
                               .OrderByDescending(role => role.CreatedDate)
                               .ToList();

            return Json(new { success = true, Data = listDocumentType });
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View("Form", new TblLoaiChungTu());
        }
        [HttpPost]
        public JsonResult Add(TblLoaiChungTu model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }
            var existingListDocumentType = _dbContext.TblLoaiChungTu.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingListDocumentType != null)
            {
                return Json(new { success = false, message = "Loại chứng từ này đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var ListDocumentType = new TblLoaiChungTu
            {

                Ma = model.Ma,
                Code = model.Code,
                Ten = model.Ten,
                Status = model.Status,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,
            };

            _dbContext.TblLoaiChungTu.Add(ListDocumentType);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Loại chứng từ đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var TblLoaiChungTu = _dbContext.TblLoaiChungTu.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (TblLoaiChungTu == null)
            {
                return NotFound();
            }
            return View("Form", TblLoaiChungTu);
        }
        [HttpPost]
        public JsonResult Edit(TblLoaiChungTu model)
        {
            var TblLoaiChungTu = _dbContext.TblLoaiChungTu.FirstOrDefault(x => x.Ma == model.Ma);
            if (TblLoaiChungTu == null)
            {
                return Json(new { success = false, message = "Loại chứng từ này không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            TblLoaiChungTu.Ma = model.Ma;
            TblLoaiChungTu.Code = model.Code;
            TblLoaiChungTu.Ten = model.Ten;
            TblLoaiChungTu.Status = model.Status;
            TblLoaiChungTu.UserModified = loggedInUser.Ma;
            TblLoaiChungTu.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.TblLoaiChungTu.Update(TblLoaiChungTu);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật loại chứng từ thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var TblLoaiChungTu = _dbContext.TblLoaiChungTu.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (TblLoaiChungTu == null)
            {
                return Json(new { success = false, message = "Loại chứng từ không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            TblLoaiChungTu.UserDeleted = loggedInUser.Ma;
            TblLoaiChungTu.Deleted = true;  // Đánh dấu đã bị xoá
            TblLoaiChungTu.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.TblLoaiChungTu.Update(TblLoaiChungTu);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }
}
