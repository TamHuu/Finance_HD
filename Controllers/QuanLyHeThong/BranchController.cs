using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.QuanLyHeThong
{
    public class BranchController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BranchController(ILogger<BranchController> logger, ApplicationDbContext context)
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
            var result = _dbContext.SysBranch.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListBranch()
        {
            var listBranch = _dbContext.SysBranch.ToList();
            return Json(new { Data = listBranch });
        }

        public IActionResult Add()
        {
            return View("Form", new SysBranch());
        }

        [HttpPost]
        public JsonResult Add(SysBranch model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Ten))
            {
                return Json(new { success = false, message = "Tên chi nhánh không được để trống!" });
            }

            var existingBranch = _dbContext.SysBranch.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingBranch != null)
            {
                return Json(new { success = false, message = "Tên chi nhánh đã tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var branch = new SysBranch
            {
                Ten = model.Ten,
                Code = model.Code,
                MaSoThue = model.MaSoThue,
                PhapNhan = model.PhapNhan,
                DiaChi = model.DiaChi,
                Logo = model.Logo,
                Status = model.Status,
                CoSoQuy = model.CoSoQuy,
                CreatedDate = DateTime.Now,
                UserCreated = loggedInUser.Ma,
            };

            _dbContext.SysBranch.Add(branch);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Chi nhánh đã được thêm thành công!" });
        }

        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var branch = _dbContext.SysBranch.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (branch == null)
            {
                return NotFound();
            }
            return View("Form", branch);
        }
        [HttpPost]
        public JsonResult Edit(SysBranch model)
        {
            var branch = _dbContext.SysBranch.FirstOrDefault(x => x.Ma == model.Ma);
            if (branch == null)
            {
                return Json(new { success = false, message = "Chi nhánh không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Cập nhật các thuộc tính
            
            branch.Ten = model.Ten;
            branch.Code = model.Code;
            branch.MaSoThue = model.MaSoThue;
            branch.PhapNhan = model.PhapNhan;
            branch.DiaChi = model.DiaChi;
            branch.CoSoQuy = model.CoSoQuy;
            branch.Status = model.Status;
            branch.UserModified = loggedInUser.Ma;
            branch.ModifiedDate = DateTime.Now;

            _dbContext.SysBranch.Update(branch);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật chi nhánh thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var branch = _dbContext.SysBranch.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (branch == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            branch.UserDeleted = loggedInUser.Ma;
            branch.Deleted = true; 
            branch.DeletedDate = DateTime.Now;  

            _dbContext.SysBranch.Update(branch); 
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá sản phẩm thành công!" });
        }
    }
}
