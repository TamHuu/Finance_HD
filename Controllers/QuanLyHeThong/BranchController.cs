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
                UserCreated = model.UserCreated,
                CreatedDate = DateTime.Now, // Thay đổi thời gian tạo
                UserModified = model.UserModified,
                ModifiedDate = null, // Không cần thiết khi tạo mới
                DeletedDate = null, // Không cần thiết khi tạo mới
                Deleted = false, // Mới tạo nên chưa xóa
                UserDeleted = null, // Không cần thiết khi tạo mới
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

            // Cập nhật các thuộc tính
            branch.Ten = model.Ten;
            branch.Code = model.Code;
            branch.MaSoThue = model.MaSoThue;
            branch.PhapNhan = model.PhapNhan;
            branch.DiaChi = model.DiaChi;
            branch.CoSoQuy = model.CoSoQuy;
            branch.Status = model.Status;
            branch.UserModified = model.UserModified;
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

            branch.Deleted = true;  // Đánh dấu đã bị xoá
            branch.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.SysBranch.Update(branch);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá sản phẩm thành công!" });
        }

        [HttpGet]
        public JsonResult LoadDanhSachBanTheoChiNhanh()
        {
            var listCN = (from chinhanh in _dbContext.SysBranch
                         join ban in _dbContext.TblBan
                         on chinhanh.Ma equals ban.MaChiNhanh into banGroup
                         from ban in banGroup.DefaultIfEmpty() 
                         select new
                         {
                             MaChiNhanh = chinhanh.Ma,
                             TenChiNhanh = chinhanh.Ten,
                             MaBan = ban != null ? ban.Ma : (Guid?)null, 
                             TenBan = ban != null ? ban.Ten : "" ,
                         }).ToList();

            return Json(new { Data = listCN });
        }

    }
}
