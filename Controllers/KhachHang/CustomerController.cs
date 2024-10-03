using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.KhachHang
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
