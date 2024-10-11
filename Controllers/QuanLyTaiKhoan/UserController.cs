using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.QuanLyTaiKhoan
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ILogger<UserController> logger, ApplicationDbContext context)
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
            var result = _dbContext.SysUser.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListUser()
        {
            var listUser = (from user in _dbContext.SysUser
                           join chinhanh in _dbContext.SysBranch on user.BranchId equals  chinhanh.Ma
                           join phongban in _dbContext.TblPhongBan on user.MaPhongBan equals phongban.Ma into banGroup
                           from ban in banGroup.DefaultIfEmpty()
                           where !(user.Deleted ?? false)
                           select new
                           {
                               MaUser = user.Ma,
                               UserName = user.Username,
                               Msnv = user.Msnv,
                               CCCD= user.Cccd,
                               SoDienThoai = user.SoDienThoai,
                               GioiTinh= user.GioiTinh,
                               Status = user.Status,
                               FullName =  user.FullName,
                               MaDinhDanh = user.MaDinhDanh,
                               NgaySinh = user.NgaySinh,
                               DiaChi= user.DiaChi,
                               NgayVaoLam = user.NgayVaoLam,
                               NgayKetThuc = user.NgayKetThuc,
                               MaChiNhanh = chinhanh.Ma,
                               TenChiNhanh = chinhanh.Ten,
                               MaPhongBan = ban.Ma,
                               TenPhongBan = ban.Ten,
                               CreatedDate= DateTime.Now,
                           })
                           
                             .OrderByDescending(role => role.CreatedDate)
                             .ToList();

            return Json(new { success = true, Data = listUser });
        }

        public IActionResult Add()
        {
            return View("Form", new SysUser());
        }

        [HttpPost]
        public JsonResult Add(SysUser model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Username))
            {
                return Json(new { success = false, message = "Tên đăng nhập không được để trống!" });
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return Json(new { success = false, message = "Mật khẩu không được để trống!" });
            }
            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                return Json(new { success = false, message = "Họ tên không được để trống!" });
            }
            if (model.BranchId == Guid.Empty)
            {
                return Json(new { success = false, message = "Chi nhánh không được để trống!" });
            }
            if (model.MaPhongBan == Guid.Empty)
            {
                return Json(new { success = false, message = "Phòng ban không được để trống!" });
            }
            if (!model.NgaySinh.HasValue)
            {
                return Json(new { success = false, message = "Ngày sinh không được để trống!" });
            }
          
            var existingUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == model.Username);
            if (existingUser != null)
            {
                return Json(new { success = false, message = "Tên người dùng đã tồn tại!" });
            }

            var user = new SysUser
            {
                Username = model.Username,
                Msnv = model.Msnv,
                Password = model.Password,
                MaDinhDanh = model.MaDinhDanh,
                BranchId = model.BranchId,
                MaPhongBan = model.MaPhongBan,
                Cccd = model.Cccd,
                Status = model.Status,
                FullName = model.FullName,
                DiaChi = model.DiaChi,
                GioiTinh = model.GioiTinh,
                NgaySinh = model.NgaySinh,
                NgayVaoLam = model.NgayVaoLam,
                SoDienThoai = model.SoDienThoai,
                NgayKetThuc = model.NgayKetThuc,
                CreatedDate = model.CreatedDate ?? DateTime.Now,
            };

            _dbContext.SysUser.Add(user);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Người dùng đã được thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var user = _dbContext.SysUser.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (user == null)
            {
                return NotFound();
            }
            return View("Form", user);
        }
        [HttpPost]
        public JsonResult Edit(SysUser model)
        {
            var user = _dbContext.SysUser.FirstOrDefault(x => x.Ma == model.Ma);
            if (user == null)
            {
                return Json(new { success = false, message = "người dùng này này không tồn tại!" });
            }
            user.Username = model.Username;
            user.Msnv = model.Msnv;
            user.MaDinhDanh = model.MaDinhDanh;
            user.BranchId = model.BranchId;
            user.MaPhongBan=model.MaPhongBan;
            user.Cccd = model.Cccd;
            user.FullName = model.FullName;
            user.DiaChi = model.DiaChi;
            user.GioiTinh = model.GioiTinh;
            user.NgaySinh = model.NgaySinh; 
            user.NgayVaoLam = model.NgayVaoLam;
            user.SoDienThoai = model.SoDienThoai;
            user.NgayKetThuc = model.NgayKetThuc;
            user.Status = model.Status;
            user.UserModified = model.UserModified;
            user.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.SysUser.Update(user);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật người dùng thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var user = _dbContext.SysUser.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (user == null)
            {
                return Json(new { success = false, message = "người dùng không tồn tại!" });
            }

            // Kích hoạt soft delete
            user.Deleted = true;  // Đánh dấu đã bị xoá
            user.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.SysUser.Update(user);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá người dùng thành công!" });
        }

    }
}
