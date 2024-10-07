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

            var IncomeExpenseType = new CatLoaiThuChi
            {
                
                Code = model.Code,
                Ten = model.Ten,
                Status = model.Status,
                UserCreated = model.UserCreated,
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

            IncomeExpenseType.Ma = model.Ma;
            IncomeExpenseType.Code = model.Code;
            IncomeExpenseType.Ten = model.Ten;
            IncomeExpenseType.Status = model.Status;
            IncomeExpenseType.UserModified = model.UserModified;
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

            // Kích hoạt soft delete
            IncomeExpenseType.Deleted = true;  // Đánh dấu đã bị xoá
            IncomeExpenseType.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.CatLoaiThuChi.Update(IncomeExpenseType);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }
}
