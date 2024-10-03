using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.NganHang
{
    public class BankAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
