using Finance_HD.Helpers;
using Finance_HD.Models;
using iText.Signatures.Validation.V1.Report;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.BaoCao
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportController(ILogger<ReportController> logger, ApplicationDbContext context)
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
            var result = _dbContext.TblDanhSachBaoCao.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListReport()
        {
            var listReport = (from dsBaoCao in _dbContext.TblDanhSachBaoCao
                              join menu in _dbContext.SysMenu on dsBaoCao.MenuId equals menu.Ma
                              where !(dsBaoCao.Deleted ?? false)
                              select new
                              {
                                  Ma = dsBaoCao.Ma,
                                  TenBaoCao = dsBaoCao.Ten,
                                  MaMenu = menu.Ma,
                                  TenMenu = menu.Name,
                                  Status = dsBaoCao.Status,
                                  Code = dsBaoCao.Code,
                                  CreatedDate = dsBaoCao.CreatedDate
                              })
                              .OrderByDescending(dsBaoCao => dsBaoCao.CreatedDate)
                              .ToList();

            return Json(new { success = true, Data = listReport });
        }


        public IActionResult Add()
        {
            ViewData["listMenu"] = _dbContext.SysMenu.ToList();
            return View("Form", new TblDanhSachBaoCao());
        }

        [HttpPost]
        public JsonResult Add(string Name, string Code, string Menu, bool Status)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return Json(new { success = false, message = "Tên báo cáo không được để trống!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var existingReport = _dbContext.TblDanhSachBaoCao.FirstOrDefault(x => x.Ten == Name);
            if (existingReport != null)
            {
                return Json(new { success = false, message = "Tên báo cáo đã tồn tại!" });
            }

            var report = new TblDanhSachBaoCao
            {
                Status = Status,
                MenuId = Menu.GetGuid(),
                Code = Code,
                Ten = Name,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,

            };

            _dbContext.TblDanhSachBaoCao.Add(report);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Báo cáo đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listMenu"] = _dbContext.SysMenu.ToList();
            ViewBag.MaDanhMuc = _dbContext.TblDanhSachBaoCao.FirstOrDefault()?.MenuId;
            var report = _dbContext.TblDanhSachBaoCao.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (report == null)
            {
                return NotFound();
            }
            return View("Form", report);
        }
        [HttpPost]
        public JsonResult Edit(string Ma, string Name, string Code, string Menu, bool Status)
        {

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var report = _dbContext.TblDanhSachBaoCao.FirstOrDefault(x => x.Ma == Ma.GetGuid());
            if (report == null)
            {
                return Json(new { success = false, message = "Người dùng cần cập nhật không tồn tại!" });
            }

            report.Status = Status;
            report.MenuId = Menu.GetGuid();
            report.Code = Code;
            report.Ten = Name;
            report.UserModified = loggedInUser.Ma;
            report.ModifiedDate = DateTime.Now;


            _dbContext.TblDanhSachBaoCao.Update(report);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật báo cáo thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var report = _dbContext.TblDanhSachBaoCao.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (report == null)
            {
                return Json(new { success = false, message = "Báo cáo không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            report.Deleted = true;  // Đánh dấu đã bị xoá
            report.DeletedDate = DateTime.Now;  // Lưu thời gian xoá
            report.UserDeleted = loggedInUser.Ma;
            _dbContext.TblDanhSachBaoCao.Update(report);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá báo cáo thành công!" });
        }

    }
}
