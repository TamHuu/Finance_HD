using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.TaiChinh
{
    public class MonetaryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public MonetaryController(ILogger<MonetaryController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            var result = _dbContext.FiaTienTe.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListMonetary()
        {
            var listMonetary = _dbContext.FiaTienTe
                             .Where(role => !(role.Deleted ?? false))
                             .OrderByDescending(role => role.CreatedDate)
                             .ToList();

            return Json(new { success = true, Data = listMonetary });
        }

        public IActionResult Add()
        {
            return View("Form", new FiaTienTe());
        }

        [HttpPost]
        public JsonResult Add(FiaTienTe model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Ten))
            {
                return Json(new { success = false, message = "Tên tiền tệ không được để trống!" });
            }

            var existingListMonetary = _dbContext.FiaTienTe.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingListMonetary != null)
            {
                return Json(new { success = false, message = "Tên tiền tệ đã tồn tại!" });
            }

            var monetary = new FiaTienTe
            {
                Ten = model.Ten,
                Code = model.Code,
                Status = model.Status,
                CreatedDate = model.CreatedDate ?? DateTime.Now,
            };

            _dbContext.FiaTienTe.Add(monetary);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Tiền tệ đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var listMonetary = _dbContext.FiaTienTe.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (listMonetary == null)
            {
                return NotFound();
            }
            return View("Form", listMonetary);
        }
        [HttpPost]
        public JsonResult Edit(FiaTienTe model)
        {
            var listMonetary = _dbContext.FiaTienTe.FirstOrDefault(x => x.Ma == model.Ma);
            if (listMonetary == null)
            {
                return Json(new { success = false, message = "Tiền tệ này không tồn tại!" });
            }


            listMonetary.Ten = model.Ten;
            listMonetary.Code = model.Code;
            listMonetary.Status = model.Status;
            listMonetary.UserModified = model.UserModified;
            listMonetary.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.FiaTienTe.Update(listMonetary);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật tiền tệ thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var listMonetary = _dbContext.FiaTienTe.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (listMonetary == null)
            {
                return Json(new { success = false, message = "Tiền tệ không tồn tại!" });
            }

            // Kích hoạt soft delete
            listMonetary.Deleted = true;  // Đánh dấu đã bị xoá
            listMonetary.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FiaTienTe.Update(listMonetary);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá tiền tệ thành công!" });
        }

    }
}
