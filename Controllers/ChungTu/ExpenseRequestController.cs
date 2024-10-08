using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.ChungTu
{
    public class ExpenseRequestController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ExpenseRequestController(ILogger<ExpenseRequestController> logger, ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            var branches = _dbContext.SysBranch.ToList();
            var deNghiChis = _dbContext.FiaDeNghiChi.ToList(); 


            ViewData["listChiNhanh"] = branches;

            return View(deNghiChis); 
        }

        public JsonResult getListExpenseRequest()
        {
            var listExpenseRequest = (from denghichi in _dbContext.FiaDeNghiChi
                                      join chinhanhdenghi in _dbContext.SysBranch
                                      on denghichi.MaChiNhanhDeNghi equals chinhanhdenghi.Ma
                                      join bophandenghi in _dbContext.TblPhongBan
                                      on denghichi.MaPhongBanDeNghi equals bophandenghi.Ma into bophandenghiGroup
                                      from bophandenghi in bophandenghiGroup.DefaultIfEmpty() 
                                      join chinhanhchi in _dbContext.SysBranch
                                      on denghichi.MaChiNhanhChi equals chinhanhchi.Ma
                                      join bophanchi in _dbContext.TblBan
                                      on denghichi.MaPhongBanChi equals bophanchi.Ma into bophanchiGroup
                                      from bophanchi in bophanchiGroup.DefaultIfEmpty() 
                                      join noidungthuchi in _dbContext.CatNoiDungThuChi
                                      on denghichi.MaNoiDung equals noidungthuchi.Ma
                                      join tien in _dbContext.FaLoaiTien
                                      on denghichi.MaTienTe equals tien.Ma
                                      where !(denghichi.Deleted ?? false)
                                      orderby denghichi.CreatedDate descending
                                      select new
                                      {
                                          Ma = denghichi.Ma,
                                          MaChiNhanhDeNghi = chinhanhdenghi.Ma,
                                          MaChiNhanhChi = chinhanhchi.Ma,
                                          MaPhongBanDeNghi = bophandenghi.Ma,
                                          MaPhongBanChi = bophanchi.Ma,
                                          SoPhieu = denghichi.SoPhieu,
                                          NgayLap = denghichi.NgayLap,
                                          NgayYeuCauNhanTien = denghichi.NgayYeuCauNhanTien,
                                          MaNoiDung = noidungthuchi.Ma,
                                          MaTienTe = tien.Ma,
                                          TyGia = denghichi.TyGia,
                                          SoTien = denghichi.SoTien,
                                          NoiDungThuChi = noidungthuchi.Ten,
                                          TenChiNhanhDeNghi = chinhanhdenghi.Ten,
                                          TenChiNhanhChi = chinhanhchi.Ten,
                                          TenPhongBanDeNghi = bophandenghi.Ten ?? "Không có", 
                                          TenPhongBanChi = bophanchi.Ten ?? "Không có", 
                                          HinhThucChi = denghichi.HinhThucChi,
                                          GhiChu = denghichi.GhiChu,
                                          TrangThai = denghichi.TrangThai,
                                          NguoiDuyet = denghichi.NguoiDuyet,
                                          NgayDuyet = denghichi.NgayDuyet
                                      }).ToList();

            return Json(new { success = true, Data = listExpenseRequest });
        }


        [HttpGet]
        public IActionResult Add()
        {

            ViewData["listTienTe"] = _dbContext.FiaTienTe.ToList();
            ViewData["listNoiDung"] = _dbContext.CatNoiDungThuChi.ToList();
            return View("Form", new FiaDeNghiChi());
        }
        [HttpPost]
        public JsonResult Add(FiaDeNghiChi model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            var existingExpenseRequest = _dbContext.FiaDeNghiChi.FirstOrDefault(x => x.TyGia == model.TyGia);
            if (existingExpenseRequest != null)
            {
                return Json(new { success = false, message = "Đề nghị chi này đã tồn tại!" });
            }

            SysBranch? cn = _dbContext.SysBranch.FirstOrDefault(t => t.Ma == model.MaChiNhanhDeNghi); // Lấy thông tin chi nhánh
            if (cn == null)
            {
                return Json(new { success = false, message = "Chi nhánh không tồn tại!" });
            }

            // Tạo số thứ tự (có thể cần phải kiểm tra để đảm bảo không trùng lặp)
            int nextSequentialNumber = GetNextSequentialNumber(cn.Code);

            var ExpenseRequest = new FiaDeNghiChi
            {
                Ma = model.Ma,
                MaChiNhanhDeNghi = model.MaChiNhanhDeNghi,
                MaChiNhanhChi = model.MaChiNhanhChi,
                MaPhongBanDeNghi = model.MaPhongBanDeNghi,
                MaPhongBanChi = model.MaPhongBanChi,
                SoPhieu = string.Format("DNC/{0}/{1:yyyyMMdd}/{2:000}", cn.Code, DateTime.Today, nextSequentialNumber),
                NgayLap = model.NgayLap,
                NgayYeuCauNhanTien = model.NgayYeuCauNhanTien,
                MaNoiDung = model.MaNoiDung,
                MaTienTe = model.MaTienTe,
                TyGia = model.TyGia,
                SoTien = model.SoTien,
                HinhThucChi = model.HinhThucChi,
                GhiChu = model.GhiChu,
                TrangThai = model.TrangThai,
                NguoiDuyet = model.NguoiDuyet,
                NgayDuyet = model.NgayDuyet,
                CreatedDate = DateTime.Now,
            };

            _dbContext.FiaDeNghiChi.Add(ExpenseRequest);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Đề nghị chi đã được thêm thành công!" });
        }

        // Hàm để lấy số thứ tự tiếp theo
        private int GetNextSequentialNumber(string chiNhanhCode)
        {
            var lastRequest = _dbContext.FiaDeNghiChi
                .Where(x => x.SoPhieu.StartsWith($"DNC/{chiNhanhCode}/"))
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefault();

            if (lastRequest != null)
            {
                // Lấy số thứ tự từ số phiếu hiện tại
                var parts = lastRequest.SoPhieu.Split('/');
                if (parts.Length > 3 && int.TryParse(parts[3], out int lastNumber))
                {
                    return lastNumber + 1; // Tăng số thứ tự lên 1
                }
            }

            return 1; // Nếu không có số nào, bắt đầu từ 1
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            ViewData["listTienTe"] = _dbContext.FiaTienTe.ToList();
            var FiaDeNghiChi = _dbContext.FiaDeNghiChi.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (FiaDeNghiChi == null)
            {
                return NotFound();
            }
            return View("Form", FiaDeNghiChi);
        }
        [HttpPost]
        public JsonResult Edit(FiaDeNghiChi model)
        {
            var ExpenseRequest = _dbContext.FiaDeNghiChi.FirstOrDefault(x => x.Ma == model.Ma);
            if (ExpenseRequest == null)
            {
                return Json(new { success = false, message = "Đề nghị chi này không tồn tại!" });
            }

            ExpenseRequest.Ma = model.Ma;
            ExpenseRequest.MaChiNhanhDeNghi = model.MaChiNhanhDeNghi;
            ExpenseRequest.MaChiNhanhChi = model.MaChiNhanhChi;
            ExpenseRequest.MaPhongBanDeNghi = model.MaPhongBanDeNghi;
            ExpenseRequest.MaPhongBanChi = model.MaPhongBanChi;
            ExpenseRequest.SoPhieu = model.SoPhieu;
            ExpenseRequest.NgayLap = model.NgayLap;
            ExpenseRequest.NgayYeuCauNhanTien = model.NgayYeuCauNhanTien;
            ExpenseRequest.MaNoiDung = model.MaNoiDung;
            ExpenseRequest.MaTienTe = model.MaTienTe;
            ExpenseRequest.TyGia = model.TyGia;
            ExpenseRequest.SoTien = model.SoTien;
            ExpenseRequest.HinhThucChi = model.HinhThucChi;
            ExpenseRequest.GhiChu = model.GhiChu;
            ExpenseRequest.TrangThai = model.TrangThai;
            ExpenseRequest.NguoiDuyet = model.NguoiDuyet;
            ExpenseRequest.NgayDuyet = model.NgayDuyet;
            ExpenseRequest.UserModified = model.UserModified;
            ExpenseRequest.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
            _dbContext.FiaDeNghiChi.Update(ExpenseRequest);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Cập nhật Đề nghị chi thành công!" });
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var ExpenseRequest = _dbContext.FiaDeNghiChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (ExpenseRequest == null)
            {
                return Json(new { success = false, message = "Đề nghị chi không tồn tại!" });
            }

            // Kích hoạt soft delete
            ExpenseRequest.Deleted = true;  // Đánh dấu đã bị xoá
            ExpenseRequest.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FiaDeNghiChi.Update(ExpenseRequest);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá thành công!" });
        }
    }
}
