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
            return View();
        }
        [HttpPost]
        public JsonResult getListReceipt(string TuNgay, string DenNgay, string ChiNhanh) 
        {
            var listResult= from donvinhan in _dbContext.SysBranch
                            join phongbannhan in _dbContext.TblPhongBan
                            on donvinhan.Ma equals phongbannhan.MaChiNhanh into phongbannhanGroup
                            from phongbannhan in phongbannhanGroup.DefaultIfEmpty()

                            from donvichi in _dbContext.SysBranch
                            join phongbanchi in _dbContext.TblPhongBan
                            on donvichi.Ma equals phongbanchi.MaChiNhanh into phongbanchiGroup


                            select new
                            {
                                DonViNhan = donvinhan,
                                PhongBanNhan = phongbannhan
                            };
            return Json(new {succes=true,Data=listResult });
        }
    }
}
