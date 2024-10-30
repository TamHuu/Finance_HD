using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.QuanLyHeThong
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public MenuController(ILogger<MenuController> logger, ApplicationDbContext context)
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
            var result = _dbContext.SysMenu.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListMenu()
        {
            var listMenu = (from user in _dbContext.SysUser
                            join chinhanh in _dbContext.SysBranch on user.BranchId equals chinhanh.Ma
                            join phongban in _dbContext.TblPhongBan on user.MaPhongBan equals phongban.Ma into banGroup
                            from ban in banGroup.DefaultIfEmpty()
                            where !(user.Deleted ?? false)
                            select new
                            {
                                MaUser = user.Ma + "",
                                UserName = user.Username + "",
                                Msnv = user.Msnv + "",
                                CCCD = user.Cccd + "",
                                SoDienThoai = user.SoDienThoai + "",
                                GioiTinh = user.GioiTinh + "",
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
                                Status = user.Status == true ? "Hoạt động" : "Hết hoạt động",
                                CreatedDate = DateTime.Now,
                            })

                             .OrderByDescending(role => role.CreatedDate)
                             .ToList();

            return Json(new { success = true, Data = listMenu });
        }

        public IActionResult Add()
        {

            return View("Form", new SysMenu());
        }

        [HttpPost]
        public JsonResult Add( string UsingFor, string Code, string MenuCha, string Name, string STT, string Url, string Icon, string MenuCon, bool Status)
        {

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            var user = new SysMenu
            {
                Code = Code,
                ParentId = MenuCha.GetGuid(),
                Name = Name,
                Link = Url,
                Icon = Icon,
                ChildOfMenu = MenuCon.GetGuid(),
                UsingFor = UsingFor,
                Status = Status,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,
            };

            _dbContext.SysMenu.Add(user);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Đã thêm thành công!" });
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

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            user.Username = model.Username;
            user.Msnv = model.Msnv;
            user.MaDinhDanh = model.MaDinhDanh;
            user.BranchId = model.BranchId;
            user.MaPhongBan = model.MaPhongBan;
            user.Cccd = model.Cccd;
            user.FullName = model.FullName;
            user.DiaChi = model.DiaChi;
            user.GioiTinh = model.GioiTinh;
            user.NgaySinh = model.NgaySinh;
            user.NgayVaoLam = model.NgayVaoLam;
            user.SoDienThoai = model.SoDienThoai;
            user.NgayKetThuc = model.NgayKetThuc;
            user.Status = model.Status;
            user.UserModified = loggedInUser.Ma;
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
