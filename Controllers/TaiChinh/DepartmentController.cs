using Finance_HD.Controllers.QuanLyHeThong;
using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.TaiChinh
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentController(ILogger<DepartmentController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult getListDepartment()
        {
            var listDepartment = (from phongban in _dbContext.TblPhongBan
                                  join chinhanh in _dbContext.SysBranch on phongban.MaChiNhanh equals chinhanh.Ma 
                                  join ban in _dbContext.TblBan on phongban.MaBan equals ban.Ma into banGroup 
                                  from ban in banGroup.DefaultIfEmpty()
                                  where !(phongban.Deleted ?? false)
                                  select new
                                  {
                                      MaPhongBan = phongban.Ma,
                                      TenPhongBan = phongban.Ten,
                                      MaChiNhanh = chinhanh.Ma, 
                                      TenChiNhanh = chinhanh.Ten,
                                      MaBan = ban != null ? ban.Ma : (Guid?)null,
                                      TenBan = ban != null ? ban.Ten : "", 
                                      Code = phongban.Code,
                                      IsCoSoQuy = phongban.CoSoQuy,
                                      IsBan = phongban.Ban,
                                      Status = phongban.Status,
                                  }).ToList();

            return Json(new { Data = listDepartment });
        }
        [HttpGet]
        public IActionResult Add()
        {
          
            return View("Form", new TblPhongBan());
        }

        [HttpPost]
        public JsonResult Add(TblPhongBan model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(model.Ten))
            {
                return Json(new { success = false, message = "Tên phòng ban không được để trống!" });
            }

            var existingDepartment = _dbContext.TblPhongBan.FirstOrDefault(x => x.Ten == model.Ten);
            if (existingDepartment != null)
            {
                return Json(new { success = false, message = "Tên phòng ban đã tồn tại!" });
            }

            var Department = new TblPhongBan
            {
                Ten = model.Ten,
                Code = model.Code,
                MaChiNhanh = model.MaChiNhanh,
                Ban = model.Ban,
                MaBan = model.MaBan,
                CoSoQuy= model.CoSoQuy,
                Status = model.Status,
                UserCreated = model.UserCreated,
                CreatedDate = DateTime.Now,
            };

            _dbContext.TblPhongBan.Add(Department);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Phòng ban đã được thêm thành công!" });
        }

        public IActionResult Edit(string Ma)
        {
            var listDepartment = _dbContext.TblPhongBan.FirstOrDefault(x=>x.Ma==Ma.GetGuid());
            if (listDepartment == null)
            {
                return NotFound();
            }
            return View("Form", listDepartment);
        }
        [HttpPost]
        public JsonResult Edit(TblPhongBan model)
        {
            var department = _dbContext.TblPhongBan.FirstOrDefault(x => x.Ma == model.Ma);
            if (department == null)
            {
                return Json(new { success = false, message = "Phòng ban này không tồn tại!" });
            }

            department.Ma = model.Ma;
            department.Code = model.Code;
            department.Ten = model.Ten;
            department.Ban = model.Ban;
            department.CoSoQuy = model.CoSoQuy;
            department.MaChiNhanh = model.MaChiNhanh;
            department.Status = model.Status;
            department.UserModified = model.UserModified;
            department.ModifiedDate = department.ModifiedDate ?? DateTime.Now;
            _dbContext.TblPhongBan.Update(department);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Cập nhật phòng ban thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var department = _dbContext.TblPhongBan.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (department == null)
            {
                return Json(new { success = false, message = "Phòng ban này không tồn tại!" });
            }

            department.Deleted = true;
            department.DeletedDate = DateTime.Now;  
            _dbContext.TblPhongBan.Update(department); 
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá phòng ban thành công!" });
        }
    }
}
