using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

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
                            join chinhanh in _dbContext.SysBranch on user.BranchId equals chinhanh.Ma

                            join phongban in _dbContext.TblPhongBan on user.MaPhongBan equals phongban.Ma into banGroup
                            from ban in banGroup.DefaultIfEmpty()
                            where !(user.Deleted ?? false)
                            select new
                            {
                                Ma = user.Ma + "",
                                UserName = user.Username + "",
                                Msnv = user.Msnv + "",
                                CCCD = user.Cccd + "",
                                SoDienThoai = user.SoDienThoai + "",
                                GioiTinh = user.GioiTinh + "",
                                Status = user.Status == true ? "Hoạt động" : "Hết hoạt động",
                                FullName = user.FullName + "",
                                MaDinhDanh = user.MaDinhDanh + "",
                                NgaySinh = user.NgaySinh + "",
                                DiaChi = user.DiaChi + "",
                                NgayVaoLam = user.NgayVaoLam + "",
                                NgayKetThuc = user.NgayKetThuc + "",
                                MaChiNhanh = chinhanh.Ma + "",
                                TenChiNhanh = chinhanh.Ten + "",
                                MaPhongBan = ban.Ma + "",
                                TenPhongBan = ban.Ten + "",
                                CreatedDate = DateTime.Now,
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
        public JsonResult Add(
    string Username,
    string Msnv,
    string MaDinhDanh,
    string BranchId,
    string MaPhongBan,
    string Password,
    string CCCD,
    string FullName,
    string DiaChi,
    string Department,
    int GioiTinh,
    DateTime NgaySinh,
    DateTime NgayVaoLam,
    string SoDienThoai,
    DateTime NgayKetThuc,
    bool Status)
        {


            var existingUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == Username);
            if (existingUser != null)
            {
                return Json(new { success = false, message = "Tên người dùng đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var user = new SysUser
            {
                Username = Username,
                Msnv = Msnv,
                Password = Password,
                MaDinhDanh = MaDinhDanh,
                BranchId = BranchId.GetGuid(),
                MaPhongBan = MaPhongBan.GetGuid(),
                Cccd = CCCD,
                Status = Status,
                FullName = FullName,
                DiaChi = DiaChi,
                GioiTinh = GioiTinh,
                NgaySinh = NgaySinh,
                NgayVaoLam = NgayVaoLam,
                SoDienThoai = SoDienThoai,
                NgayKetThuc = NgayKetThuc,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,

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
        public JsonResult Edit(
     string Ma,
     string Username,
     string Msnv,
     string MaDinhDanh,
     string BranchId,
     string MaPhongBan,
     string Password,
     string CCCD,
     string FullName,
     string DiaChi,
     string Department,
     int GioiTinh,
     DateTime? NgaySinh,
     DateTime? NgayVaoLam,
     string SoDienThoai,
     DateTime? NgayKetThuc,
     bool Status
 )
        {
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            // Kiểm tra nếu người dùng cần cập nhật có tồn tại
            var user = _dbContext.SysUser.FirstOrDefault(x => x.Ma == Ma.GetGuid());
            if (user == null)
            {
                return Json(new { success = false, message = "Người dùng cần cập nhật không tồn tại!" });
            }

            // Cập nhật các thuộc tính
            user.Username = Username;
            user.Msnv = Msnv;
            user.Password = Password;
            user.MaDinhDanh = MaDinhDanh;
            user.BranchId = BranchId.GetGuid();
            user.MaPhongBan = MaPhongBan.GetGuid();
            user.Cccd = CCCD;
            user.Status = Status;
            user.FullName = FullName;
            user.DiaChi = DiaChi;
            user.GioiTinh = GioiTinh;
            user.NgaySinh = NgaySinh;
            user.NgayVaoLam = NgayVaoLam;
            user.SoDienThoai = SoDienThoai;
            user.NgayKetThuc = NgayKetThuc;
            user.UserModified = loggedInUser.Ma;
            user.ModifiedDate = DateTime.Now;

            // Thực hiện cập nhật vào database
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
                return Json(new { success = false, message = "Người dùng không tồn tại!" });
            }

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            user.Deleted = true;
            user.DeletedDate = DateTime.Now;
            user.UserDeleted = loggedInUser.Ma;

            _dbContext.SysUser.Update(user);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá người dùng thành công!" });
        }


    }
}
