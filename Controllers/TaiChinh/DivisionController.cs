using Finance_HD.Controllers.QuanLyHeThong;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.TaiChinh
{
    public class DivisionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public DivisionController(ILogger<DivisionController> logger, ApplicationDbContext context)
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
        public JsonResult getListDivision()
        {
            var listDivision = (from ban in _dbContext.TblBan
                                join chinhanh in _dbContext.SysBranch on ban.MaChiNhanh equals chinhanh.Ma
                                where !(ban.Deleted ?? false)
                                select new
                                {
                                    Ma = ban.Ma,
                                    MaChiNhanh= chinhanh.Ma,
                                    TenChiNhanh = chinhanh.Ten,
                                    Code= ban.Code,
                                    Ten= ban.Ten,
                                    Status= ban.Status,
                                }).ToList();
                                
            return Json(new {Data= listDivision });
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["listCN"]= _dbContext.SysBranch.ToList();
            return View("Form", new TblBan());
        }
        [HttpPost]
        public JsonResult Add(TblBan model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Ten))
            {
                return Json(new { success = false, message = "Tên ban không được để trống!" });
            }

            var existingDivision = _dbContext.TblBan.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingDivision != null)
            {
                return Json(new { success = false, message = "Tên ban đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var Division = new TblBan
            {
                MaChiNhanh = model.MaChiNhanh,
                Code = model.Code,
                Ten = model.Ten,
                Status = model.Status,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,
            };

            _dbContext.TblBan.Add(Division);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Ban đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listCN"] = _dbContext.SysBranch.ToList();
            var division = _dbContext.TblBan.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (division == null)
            {
                return NotFound();
            }
            return View("Form", division);
        }
        [HttpPost]
        public JsonResult Edit(TblBan model)
        {
            var division = _dbContext.TblBan.FirstOrDefault(x => x.Ma == model.Ma);
            if (division == null)
            {
                return Json(new { success = false, message = "Ban này không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            division.Ma = model.Ma;
            division.Code = model.Code;
            division.Ten = model.Ten;
            division.MaChiNhanh = model.MaChiNhanh;
            division.Status = model.Status;
            division.UserModified = loggedInUser.Ma;
            division.ModifiedDate = division.ModifiedDate ?? DateTime.Now;
            _dbContext.TblBan.Update(division);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật ban thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var division = _dbContext.TblBan.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (division == null)
            {
                return Json(new { success = false, message = "Ban không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            division.UserDeleted = loggedInUser.Ma;
            division.Deleted = true;  // Đánh dấu đã bị xoá
            division.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.TblBan.Update(division);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá Ban thành công!" });
        }
    }
}
