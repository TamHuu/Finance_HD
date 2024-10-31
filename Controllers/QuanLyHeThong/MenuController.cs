using Finance_HD.Common;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Humanizer.Localisation;
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
            // Lấy tất cả menu
            var allMenus = _dbContext.SysMenu
                .Where(menu => !(menu.Deleted ?? false))
                .Select(menu => new
                {
                    website = menu.UsingFor,
                    ma = menu.Ma,
                    code = menu.Code,
                    ten = menu.Name,
                    maMenuCha = menu.ParentId,
                    stt = menu.Sequence,
                    url = menu.Link,
                    icon = menu.Icon,
                    status = menu.Status,
                    tenMenuCha = _dbContext.SysMenu
            .Where(parent => parent.Ma == menu.ParentId)
            .Select(parent => parent.Name)
           .FirstOrDefault() ?? "" ,

                    tenMenuCon = _dbContext.SysMenu
            .Where(child => child.ParentId == menu.Ma) // Lọc menu con theo ma
            .Select(parent => parent.Name)
            .FirstOrDefault() ?? "",
                
                })
                .ToList();

            // Lấy danh sách menu cha
            var parentMenus = allMenus
                .Where(menu => menu.maMenuCha == Guid.Empty) // Lọc menu cha
                .Select(menu => new
                {
                    website = menu.website,
                    ma = menu.ma,
                    code = menu.code,
                    ten = menu.ten,
                    stt = menu.stt,
                    url = menu.url,
                    icon = menu.icon,
                    status = menu.status,
                    danhSachMenuCon = allMenus
                        .Where(child => child.maMenuCha == menu.ma) // Lọc menu con theo maMenuCha
                        .Select(child => new
                        {
                            ma = child.ma,
                            ten = child.ten,
                            url = child.url,
                            icon = child.icon
                        })
                        .ToList()
                })
                .ToList();

            // Lấy danh sách menu con (không lặp lại cha)
            var childMenus = allMenus
                .Where(menu => menu.maMenuCha != Guid.Empty) // Lọc menu con
                .Select(menu => new
                {
                    ma = menu.ma,
                    ten = menu.ten,
                    url = menu.url,
                    icon = menu.icon,
                    maMenuCha = menu.maMenuCha
                })
                .ToList();

            // Gộp tất cả vào một biến
            var result = new
            {
                success = true,
                menus = new
                {
                    allMenus,
                    parentMenus,
                    childMenus
                }
            };

            return Json(result);
        }



        public IActionResult Add()
        {

            return View("Form", new SysMenu());
        }

        [HttpPost]
        public JsonResult Add(string UsingFor, string Code, string MenuCha, string Name, int STT, string Url, string Icon, string MenuCon, bool Status)
        {

            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            var MENU = new SysMenu
            {
                Code = Code,
                ParentId = MenuCha.GetGuid(),
                Name = Name,
                Link = Url,
                Icon = Icon,
                //ChildOfMenu = MenuCon.GetGuid(),
                UsingFor = UsingFor,
                Status = Status,
                Sequence = STT,
                UserCreated = loggedInUser.Ma,
                CreatedDate = DateTime.Now,
            };

            _dbContext.SysMenu.Add(MENU);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Đã thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var menu = _dbContext.SysMenu.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (menu == null)
            {
                return NotFound();
            }
            return View("Form", menu);
        }
        [HttpPost]
        public JsonResult Edit(string Ma, string UsingFor, string Code, string MenuCha, string Name, int STT, string Url, string Icon, string MenuCon, bool Status)
        {
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);
            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);

            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }

            // Tìm menu hiện tại trong cơ sở dữ liệu dựa trên mã
            var existingMenu = _dbContext.SysMenu.FirstOrDefault(m => m.Code == Code); // Hoặc dựa trên thuộc tính duy nhất khác

            if (existingMenu == null)
            {
                return Json(new { success = false, message = "Menu không tồn tại!" });
            }

            // Cập nhật thuộc tính của menu đã tồn tại
            existingMenu.ParentId = MenuCha.GetGuid();
            existingMenu.Name = Name;
            existingMenu.Sequence = STT;
            existingMenu.Link = Url;
            existingMenu.Icon = Icon;
            existingMenu.ChildOfMenu = MenuCon.GetGuid();
            existingMenu.Status = Status;
            existingMenu.UsingFor = UsingFor;
            existingMenu.UserModified = loggedInUser.Ma;
            existingMenu.ModifiedDate = DateTime.Now;

            _dbContext.SysMenu.Update(existingMenu); // Cập nhật đối tượng hiện có
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật người dùng thành công!" });
        }

        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var user = _dbContext.SysMenu.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (user == null)
            {
                return Json(new { success = false, message = "Menu không tồn tại!" });
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

            _dbContext.SysMenu.Update(user);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }


    }
}
