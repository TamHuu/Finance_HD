using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.BaoCao
{
    public class WarningLimitController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public WarningLimitController(ILogger<WarningLimitController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult getListWarningLimit()
        {
            var listWarningLimit = (from hanmuc in _dbContext.FiaHanMucCanhBao
                                join phongban in _dbContext.TblPhongBan on hanmuc.MaBoPhan equals phongban.Ma
                                where !(hanmuc.Deleted ?? false)
                                select new
                                {
                                    Ma = hanmuc.Ma,
                                    MaBoPhan = phongban.Ma,
                                    TenBoPhan = phongban.Ten,
                                    HanMuc = hanmuc.HanMuc,
                                    Status = hanmuc.Status,
                                    CreatedDate = DateTime.Now,
                                }).OrderByDescending(x=>x.CreatedDate).ToList();

            return Json(new { Data = listWarningLimit });
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["listPhongBan"] = _dbContext.TblPhongBan.ToList();
            return View("Form", new FiaHanMucCanhBao());
        }
        [HttpPost]
        public JsonResult Add(FiaHanMucCanhBao model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (model.HanMuc<=0)
            {
                return Json(new { success = false, message = " Hạn mức không được để trống!" });
            }

            var existingWarningLimit = _dbContext.FiaHanMucCanhBao.FirstOrDefault(x => x.HanMuc == model.HanMuc);
            if (existingWarningLimit != null)
            {
                return Json(new { success = false, message = "Hạn mức đã tồn tại!" });
            }

            var WarningLimit = new FiaHanMucCanhBao
            {
                MaBoPhan = model.MaBoPhan,
                Ma = model.Ma,
                HanMuc = model.HanMuc,
                Status = model.Status,
                UserCreated = model.UserCreated,
                CreatedDate = DateTime.Now,
            };

            _dbContext.FiaHanMucCanhBao.Add(WarningLimit);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listCN"] = _dbContext.SysBranch.ToList();
            var WarningLimit = _dbContext.FiaHanMucCanhBao.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (WarningLimit == null)
            {
                return NotFound();
            }
            return View("Form", WarningLimit);
        }
        [HttpPost]
        public JsonResult Edit(FiaHanMucCanhBao model)
        {
            var WarningLimit = _dbContext.FiaHanMucCanhBao.FirstOrDefault(x => x.Ma == model.Ma);
            if (WarningLimit == null)
            {
                return Json(new { success = false, message = "Hạn mức này không tồn tại!" });
            }

            WarningLimit.Ma = model.Ma;
            WarningLimit.MaBoPhan = model.MaBoPhan;
            WarningLimit.HanMuc = model.HanMuc;
            WarningLimit.Status = model.Status;
            WarningLimit.UserModified = model.UserModified;
            WarningLimit.ModifiedDate = WarningLimit.ModifiedDate ?? DateTime.Now;
            _dbContext.FiaHanMucCanhBao.Update(WarningLimit);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var WarningLimit = _dbContext.FiaHanMucCanhBao.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (WarningLimit == null)
            {
                return Json(new { success = false, message = "Hạn mức không tồn tại!" });
            }

            // Kích hoạt soft delete
            WarningLimit.Deleted = true;  // Đánh dấu đã bị xoá
            WarningLimit.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FiaHanMucCanhBao.Update(WarningLimit);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }
}
