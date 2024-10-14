using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.TaiChinh
{
    public class ExchangeRateController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ExchangeRateController(ILogger<ExchangeRateController> logger, ApplicationDbContext context)
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
        public JsonResult getListExchangeRate()
        {
            var listExchangeRate = (from tygia in _dbContext.TblTyGia
                                    join tiente in _dbContext.FiaTienTe
                                    on tygia.MaTienTe equals tiente.Ma
                                    where !(tygia.Deleted ?? false) 
                                    orderby tygia.CreatedDate descending 
                                    select new
                                    {
                                        MaTyGia = tygia.Ma,
                                        TenTyGia = tygia.TyGia,
                                        MaTienTe = tiente.Ma,
                                        TenTienTe = tiente.Ten,
                                        Status = tygia.Status,
                                        CreatedDate = tygia.CreatedDate,
                                        NgayApDung = tygia.NgayApDung ?? DateTime.Now,
                                    })
                              .ToList();

            return Json(new { success = true, Data = listExchangeRate });
        }


        [HttpGet]
        public IActionResult Add()
        {

            ViewData["listTienTe"] = _dbContext.FiaTienTe.ToList();
            return View("Form", new TblTyGia());
        }
        [HttpPost]
        public JsonResult Add(TblTyGia model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }
            var existingExchangeRate = _dbContext.TblTyGia.FirstOrDefault(x => x.TyGia == model.TyGia);
            if (existingExchangeRate != null)
            {
                return Json(new { success = false, message = "Tỷ giá này đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var ExchangeRate = new TblTyGia
            {

                Ma = model.Ma,
                MaTienTe = model.MaTienTe,
                TyGia = model.TyGia,
                Status = model.Status,
                UserCreated = loggedInUser.Ma,
                NgayApDung = model.NgayApDung,
                CreatedDate = DateTime.Now,
            };

            _dbContext.TblTyGia.Add(ExchangeRate);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Tỷ giá đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listTienTe"] = _dbContext.FiaTienTe.ToList();
            var TblTyGia = _dbContext.TblTyGia.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (TblTyGia == null)
            {
                return NotFound();
            }
            return View("Form", TblTyGia);
        }
        [HttpPost]
        public JsonResult Edit(TblTyGia model)
        {
            var ExchangeRate = _dbContext.TblTyGia.FirstOrDefault(x => x.Ma == model.Ma);
            if (ExchangeRate == null)
            {
                return Json(new { success = false, message = "Tỷ giá này không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            ExchangeRate.Ma = model.Ma;
            ExchangeRate.MaTienTe = model.MaTienTe;
            ExchangeRate.TyGia = model.TyGia;
            ExchangeRate.NgayApDung = model.NgayApDung;
            ExchangeRate.Status = model.Status;
            ExchangeRate.UserModified = loggedInUser.Ma;
            ExchangeRate.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.TblTyGia.Update(ExchangeRate);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật tỷ giá thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var ExchangeRate = _dbContext.TblTyGia.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (ExchangeRate == null)
            {
                return Json(new { success = false, message = "Tỷ giá không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            ExchangeRate.UserDeleted = loggedInUser.Ma;
            ExchangeRate.Deleted = true;  // Đánh dấu đã bị xoá
            ExchangeRate.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.TblTyGia.Update(ExchangeRate);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }
}
