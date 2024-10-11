using Finance_HD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Finance_HD.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AccountController(ApplicationDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login([FromBody] SysUser model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return Json(new { success = false, message = "Tên đăng nhập và mật khẩu không được để trống." });
            }

            if (!IsValidPassword(model.Password))
            {
                return Json(new { success = false, message = "Mật khẩu không hợp lệ." });
            }
            var user = _dbContext.SysUser.FirstOrDefault(x => x.Username == model.Username);

            if (user == null || user.Password != model.Password) 
            {
                return Json(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng." });
            }
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key) || Encoding.UTF8.GetByteCount(key) < 32)
            {
                return Json(new { success = false, message = "Khóa JWT không hợp lệ." });
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);

            // Tạo token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("FullName", user.FullName), // Thêm FullName vào Claims
            new Claim("SDT", user.SoDienThoai) // Thêm SDT vào Claims
        }),
                Expires = DateTime.UtcNow.AddHours(1), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var cookieOptions = new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true, 
                Expires = DateTimeOffset.UtcNow.AddHours(1) 
            };

            Response.Cookies.Append("UserName", user.Username, cookieOptions);
            Response.Cookies.Append("FullName", user.FullName, cookieOptions);
            Response.Cookies.Append("SDT", user.SoDienThoai, cookieOptions);
            return Json(new { success = true, message = "Đăng nhập thành công" });
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6; 
        }

    }
}
