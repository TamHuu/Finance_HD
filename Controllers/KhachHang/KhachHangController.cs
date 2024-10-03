using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.KhachHang
{
    public class KhachHangController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
