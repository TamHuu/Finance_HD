using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.TaiChinh
{
    public class CurrencyController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CurrencyController(ILogger<CurrencyController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult getListCurrency()
        {
            var listCurrency = (from loaitien in _dbContext.FaLoaiTien
                                join tiente in _dbContext.FiaTienTe on loaitien.MaTienTe equals tiente.Ma
                                where !(loaitien.Deleted ?? false)
                                select new
                                {
                                    Ma = loaitien.Ma,
                                    MaTienTe = tiente.Ma,
                                    TenTienTe = tiente.Ten,
                                    Code = loaitien.Code,
                                    GiaTri = loaitien.GiaTri,
                                    Status = loaitien.Status,
                                }).ToList();

            return Json(new { Data = listCurrency });
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["listTienTe"] = _dbContext.FiaTienTe.ToList();
            return View("Form", new FaLoaiTien());
        }
        [HttpPost]
        public JsonResult Add(FaLoaiTien model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }
            if (model.GiaTri == null || model.GiaTri <= 0)
            {
                return Json(new { success = false, message = "Giá trị không được để trống hoặc phải lớn hơn 0!" });
            }


            var existingDivision = _dbContext.FaLoaiTien.FirstOrDefault(x => x.GiaTri == model.GiaTri);
            if (existingDivision != null)
            {
                return Json(new { success = false, message = "Loại tiền đã tồn tại!" });
            }

            var listCurrency = new FaLoaiTien
            {
                MaTienTe = model.MaTienTe,
                Code = model.Code,
                GiaTri = model.GiaTri,
                Status = model.Status,
                UserCreated = model.UserCreated,
                CreatedDate = DateTime.Now,
            };

            _dbContext.FaLoaiTien.Add(listCurrency);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Loại tiền đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listCN"] = _dbContext.SysBranch.ToList();
            var listCurrency = _dbContext.FaLoaiTien.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (listCurrency == null)
            {
                return NotFound();
            }
            return View("Form", listCurrency);
        }
        [HttpPost]
        public JsonResult Edit(FaLoaiTien model)
        {
            var listCurrency = _dbContext.FaLoaiTien.FirstOrDefault(x => x.Ma == model.Ma);
            if (listCurrency == null)
            {
                return Json(new { success = false, message = "Loại tiền này không tồn tại!" });
            }

            listCurrency.Ma = model.Ma;
            listCurrency.Code = model.Code;
            listCurrency.GiaTri = model.GiaTri;
            listCurrency.MaTienTe = model.MaTienTe;
            listCurrency.Status = model.Status;
            listCurrency.UserModified = model.UserModified;
            listCurrency.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.FaLoaiTien.Update(listCurrency);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật loại tiền thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var listCurrency = _dbContext.FaLoaiTien.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (listCurrency == null)
            {
                return Json(new { success = false, message = "Loại tiền không tồn tại!" });
            }

            // Kích hoạt soft delete
            listCurrency.Deleted = true;  // Đánh dấu đã bị xoá
            listCurrency.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FaLoaiTien.Update(listCurrency);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá loại tiền thành công!" });
        }
    }
}
