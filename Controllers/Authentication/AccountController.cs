using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Finance_HD.Models;
using Microsoft.AspNetCore.Authentication.Cookies; // Đảm bảo bạn import đúng namespace cho model

public class AccountController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public AccountController(ILogger<AccountController> logger, ApplicationDbContext context)
    {
        _dbContext = context;
    }
    // GET: /Account/Login
    [AllowAnonymous] 
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home"); 
        }
        ViewData["listCN"]= _dbContext.SysBranch.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid && model.Username == "admin" && model.Password == "password") 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            return RedirectToAction("Index", "Home"); 
        }

        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
