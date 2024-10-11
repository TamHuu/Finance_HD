using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.TaiChinh
{
    public class IncomeContentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public IncomeContentController(ILogger<IncomeContentController> logger, ApplicationDbContext context)
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
        public JsonResult getListIncomeContent()
        {
            var listIncomeContent = (from noidungthuchi in _dbContext.CatNoiDungThuChi
                                join loaithuchi in _dbContext.CatLoaiThuChi on noidungthuchi.MaLoaiThuChi equals loaithuchi.Ma
                                where !(noidungthuchi.Deleted ?? false)
                                select new
                                {
                                    Ma = noidungthuchi.Ma,
                                    Code = noidungthuchi.Code,
                                    CreatedDate = noidungthuchi.CreatedDate,
                                    Ten = noidungthuchi.Ten,
                                    MaLoaiThuChi = loaithuchi.Ma,
                                    TenLoaiThuChi = loaithuchi.Ten,
                                    Status = noidungthuchi.Status,
                                    NoiBo = noidungthuchi.NoiBo,
                                }).ToList();

            return Json(new { Data = listIncomeContent });
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["listLoaiThuChi"] = _dbContext.CatLoaiThuChi.ToList();
            return View("Form", new CatNoiDungThuChi());
        }
        [HttpPost]
        public JsonResult Add(CatNoiDungThuChi model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Ten))
            {
                return Json(new { success = false, message = "Nội dung thi chi không được để trống!" });
            }

            var existingIncomeContent = _dbContext.CatNoiDungThuChi.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingIncomeContent != null)
            {
                return Json(new { success = false, message = "Nội dung thu chi đã tồn tại!" });
            }

            var IncomeContent = new CatNoiDungThuChi
            {
                MaLoaiThuChi = model.MaLoaiThuChi,
                Code = model.Code,
                Ten = model.Ten,
                Status = model.Status,
                UserCreated = model.UserCreated,
                CreatedDate = DateTime.Now,
                NoiBo = model.NoiBo,
            };

            _dbContext.CatNoiDungThuChi.Add(IncomeContent);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listLoaiThuChi"] = _dbContext.CatLoaiThuChi.ToList();
            var IncomeContent = _dbContext.CatNoiDungThuChi.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (IncomeContent == null)
            {
                return NotFound();
            }
            return View("Form", IncomeContent);
        }
        [HttpPost]
        public JsonResult Edit(CatNoiDungThuChi model)
        {
            var IncomeContent = _dbContext.CatNoiDungThuChi.FirstOrDefault(x => x.Ma == model.Ma);
            if (IncomeContent == null)
            {
                return Json(new { success = false, message = "Nội dung thu chi này không tồn tại!" });
            }

            IncomeContent.Ma = model.Ma;
            IncomeContent.Code = model.Code;
            IncomeContent.Ten = model.Ten;
            IncomeContent.MaLoaiThuChi = model.MaLoaiThuChi;
            IncomeContent.NoiBo = model.NoiBo;
            IncomeContent.Status = model.Status;
            IncomeContent.UserModified = model.UserModified;
            IncomeContent.ModifiedDate = IncomeContent.ModifiedDate ?? DateTime.Now;
            _dbContext.CatNoiDungThuChi.Update(IncomeContent);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var IncomeContent = _dbContext.CatNoiDungThuChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (IncomeContent == null)
            {
                return Json(new { success = false, message = "Nội dung thu chi không tồn tại!" });
            }

            // Kích hoạt soft delete
            IncomeContent.Deleted = true;  // Đánh dấu đã bị xoá
            IncomeContent.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.CatNoiDungThuChi.Update(IncomeContent);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }
}
