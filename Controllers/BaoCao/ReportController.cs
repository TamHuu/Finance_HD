using Finance_HD.Helpers;
using Finance_HD.Models;
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
        public JsonResult Add(TblDanhSachBaoCao model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Ten))
            {
                return Json(new { success = false, message = "Tên báo cáo không được để trống!" });
            }

            var existingReport = _dbContext.TblDanhSachBaoCao.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingReport != null)
            {
                return Json(new { success = false, message = "Tên báo cáo đã tồn tại!" });
            }

            var report = new TblDanhSachBaoCao
            {
                Ten = model.Ten,
                Code = model.Code,
                MenuId = model.MenuId,
                Status = model.Status,
                CreatedDate = model.CreatedDate ?? DateTime.Now,
            };

            _dbContext.TblDanhSachBaoCao.Add(report);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Báo cáo đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var report = _dbContext.TblDanhSachBaoCao.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (report == null)
            {
                return NotFound();
            }
            return View("Form", report);
        }
        [HttpPost]
        public JsonResult Edit(TblDanhSachBaoCao model)
        {
            var report = _dbContext.TblDanhSachBaoCao.FirstOrDefault(x => x.Ma == model.Ma);
            if (report == null)
            {
                return Json(new { success = false, message = "Báo cáo này không tồn tại!" });
            }


            report.Ten = model.Ten;
            report.Code = model.Code;
            report.MenuId = model.MenuId;
            report.Status = model.Status;
            report.UserModified = model.UserModified;
            report.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
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

            // Kích hoạt soft delete
            report.Deleted = true;  // Đánh dấu đã bị xoá
            report.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.TblDanhSachBaoCao.Update(report);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá báo cáo thành công!" });
        }

    }
}
