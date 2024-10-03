using Microsoft.AspNetCore.Mvc;

namespace Finance_HD.Controllers.TaiChinh
{
    public class ExchangeRateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
