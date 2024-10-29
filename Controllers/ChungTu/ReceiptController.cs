using Finance_HD.Helpers;
using Finance_HD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.ChungTu
{
    public class ReceiptController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ReceiptController(ILogger<ReceiptController> logger, ApplicationDbContext context)
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
            var result = _dbContext.FiaPhieuThuChi.ToList();
            return View(result);
        }
        [HttpGet]
        public JsonResult getListReceipt()
        {
            var listReceipt = _dbContext.TblLoaiChungTu
                              .Where(role => !(role.Deleted ?? false))
                              .OrderByDescending(role => role.CreatedDate)
                              .ToList();

            return Json(new { success = true, Data = listReceipt });
        }

        public IActionResult Add()
        {
            ViewBag.DongTienThu = _dbContext.CatNoiDungThuChi.ToList();
            ViewBag.DongTienChi = _dbContext.CatNoiDungThuChi.ToList();
            ViewBag.LoaiTaiKhoanNganHang = _dbContext.FiaLoaiTaiKhoanNganHang
           .GroupBy(x => x.Ten)
           .Select(g => g.First())
           .ToList();

            return View("Form", new FiaPhieuThuChi());
        }

        [HttpPost]
        public JsonResult Add(string ChiNhanh, string PhongBan, string NganHang, string TienTe, string SoTaiKhoan, string DienGiai, string DongTienThu, string DongTienChi, bool Status, string LoaiTaiKhoan)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            if (string.IsNullOrWhiteSpace(ChiNhanh))
            {
                return Json(new { success = false, message = "Tên chi nhánh không được để trống!" });
            }

            //var existingListReceipt = _dbContext.FiaPhieuThuChi.FirstOrDefault(x => x.SoTaiKhoan == SoTaiKhoan);
            //if (existingListReceipt != null)
            //{
            //    return Json(new { success = false, message = "Số tài khoản ngân hàng đã tồn tại!" });
            //}
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            var Receipt = new FiaPhieuThuChi
            {
                //MaChiNhanh = ChiNhanh.GetGuid(),
                //MaPhongBan = PhongBan.GetGuid(),
                //MaNganHang = NganHang.GetGuid(),
                //MaTienTe = TienTe.GetGuid(),
                //SoTaiKhoan = SoTaiKhoan,
                //MaLoai = LoaiTaiKhoan.GetGuid(),
                //DienGiai = DienGiai,
                //DongTienThu = DongTienThu.GetGuid(),
                //DongTienChi = DongTienChi.GetGuid(),
                //Status = Status,
                //CreatedDate = DateTime.Now,
                //UserCreated = loggedInUser.Ma,
            };

            _dbContext.FiaPhieuThuChi.Add(Receipt);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Thêm thành công!" });
        }


        [HttpGet]
        public IActionResult Edit(string Ma)
        {
            var listReceipt = _dbContext.FiaPhieuThuChi.FirstOrDefault(c => c.Ma == Ma.GetGuid());
            if (listReceipt == null)
            {
                return NotFound();
            }
            return View("Form", listReceipt);
        }
        [HttpPost]
        //public JsonResult Edit(FiaPhieuThuChi model)
        //{
        //    var listReceipt = _dbContext.FiaPhieuThuChi.FirstOrDefault(x => x.Ma == model.Ma);
        //    if (listReceipt == null)
        //    {
        //        return Json(new { success = false, message = "Tiền tệ này không tồn tại!" });
        //    }
        //    string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

        //    var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
        //    if (loggedInUser == null)
        //    {
        //        return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
        //    }

        //    listReceipt.Ten = model.Ten;
        //    listReceipt.Code = model.Code;
        //    listReceipt.Status = model.Status;
        //    listReceipt.UserModified = loggedInUser.Ma;
        //    listReceipt.ModifiedDate = model.ModifiedDate ?? DateTime.Now;
        //    _dbContext.FiaPhieuThuChi.Update(listReceipt);
        //    _dbContext.SaveChanges();

        //    return Json(new { success = true, message = "Cập nhật tiền tệ thành công!" });
        //}
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var listReceipt = _dbContext.FiaPhieuThuChi.FirstOrDefault(x => x.Ma == Id.GetGuid());
            if (listReceipt == null)
            {
                return Json(new { success = false, message = "Tiền tệ không tồn tại!" });
            }
            string loggedInUserName = UserHelper.GetLoggedInUserGuid(Request);

            var loggedInUser = _dbContext.SysUser.FirstOrDefault(x => x.Username == loggedInUserName);
            if (loggedInUser == null)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng hiện tại!" });
            }
            // Kích hoạt soft delete
            listReceipt.UserDeleted = loggedInUser.Ma;
            listReceipt.Deleted = true;  // Đánh dấu đã bị xoá
            listReceipt.DeletedDate = DateTime.Now;  // Lưu thời gian xoá

            _dbContext.FiaPhieuThuChi.Update(listReceipt);  // Cập nhật vào CSDL
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Xoá tiền tệ thành công!" });
        }

    }
}
