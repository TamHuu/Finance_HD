using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.KhachHang
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerController(ILogger<CustomerController> logger, ApplicationDbContext context)
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
        public JsonResult getListCustomer()
        {
            var listCustomer = (from khachhang in _dbContext.FiaKhachHang
                                join chinhanh in _dbContext.SysBranch on khachhang.MaChiNhanh equals chinhanh.Ma
                                join phongban in _dbContext.TblPhongBan on khachhang.MaPhongBan equals phongban.Ma
                                where !(khachhang.Deleted ?? false)
                                select new
                                {
                                    Ma = khachhang.Ma,
                                    MaChiNhanh = chinhanh.Ma,
                                    MaPhongBan = phongban.Ma,
                                    Ten = khachhang.Ten,
                                    TenChiNhanh = chinhanh.Ten,
                                    TenPhongBan = phongban.Ten,
                                    Code = khachhang.Code,
                                    SoDienThoai = khachhang.SoDienThoai,
                                    DiaChi = khachhang.DiaChi,
                                    Status = khachhang.Status,
                                    CreatedDate = DateTime.Now,

                                }).OrderByDescending(x => x.CreatedDate).ToList();

            return Json(new { Data = listCustomer });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Form", new FiaKhachHang());
        }
        [HttpPost]
        public JsonResult Add(FiaKhachHang model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }
          
            var existingCustomer = _dbContext.FiaKhachHang.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingCustomer != null)
            {
                return Json(new { success = false, message = "Khách hàng đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var listCustomer = new FiaKhachHang
            {
                Ma = model.Ma,
                Code = model.Code,
                MaChiNhanh = model.MaChiNhanh,
                MaPhongBan = model.MaPhongBan,
                Ten = model.Ten,
                SoDienThoai = model.SoDienThoai,
                DiaChi = model.DiaChi,
                Status = model.Status,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,
            };

            _dbContext.FiaKhachHang.Add(listCustomer);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = " Đã thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var listCustomer = _dbContext.FiaKhachHang.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (listCustomer == null)
            {
                return NotFound();
            }
            return View("Form", listCustomer);
        }
        [HttpPost]
        public JsonResult Edit(FiaKhachHang model)
        {
            var listCustomer = _dbContext.FiaKhachHang.FirstOrDefault(x => x.Ma == model.Ma);
            if (listCustomer == null)
            {
                return Json(new { success = false, message = "Khách hàng này không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            listCustomer.Ma = model.Ma;
            listCustomer.Code = model.Code;
            listCustomer.MaChiNhanh = model.MaChiNhanh;
            listCustomer.MaPhongBan = model.MaPhongBan;
            listCustomer.Ten = model.Ten;
            listCustomer.SoDienThoai = model.SoDienThoai;
            listCustomer.DiaChi = model.DiaChi;
            listCustomer.Status = model.Status;
            listCustomer.UserModified = loggedInUser.Ma;
            listCustomer.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.FiaKhachHang.Update(listCustomer);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var listCustomer = _dbContext.FiaKhachHang.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (listCustomer == null)
            {
                return Json(new { success = false, message = "Khách hàng không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            listCustomer.UserDeleted = loggedInUser.Ma;
            listCustomer.Deleted = true;  
            listCustomer.DeletedDate = DateTime.Now; 

            _dbContext.FiaKhachHang.Update(listCustomer);  
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }
}
