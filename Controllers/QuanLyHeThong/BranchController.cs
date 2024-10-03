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

    }
}
