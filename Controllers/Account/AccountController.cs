﻿using Finance_HD.Models;
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
            var user = (from userLogin in _dbContext.SysUser
                        join chinhanh in _dbContext.SysBranch
                        on userLogin.BranchId equals chinhanh.Ma
                        select new
                        {
                            Ma = userLogin.Ma + "",
                            Username = userLogin.Username + "",
                            Password = userLogin.Password + "",
                            TenChiNhanhDangNhap = chinhanh.Ten + "",
                            FullName = userLogin.FullName + "",
                            SoDienThoai = userLogin.SoDienThoai + "",

                        }
                       ).FirstOrDefault(x => x.Username == model.Username);

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
            new Claim("SDT", user.SoDienThoai),
             new Claim("ChiNhanhDangNhap", user.TenChiNhanhDangNhap) 
        }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var cookieOptions = new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(10)
            };

            Response.Cookies.Append("UserName", user.Username, cookieOptions);
            Response.Cookies.Append("FullName", user.FullName, cookieOptions);
            Response.Cookies.Append("SDT", user.SoDienThoai, cookieOptions);
            Response.Cookies.Append("ChiNhanhDangNhap", user.TenChiNhanhDangNhap, cookieOptions);
            return Json(new { success = true, message = "Đăng nhập thành công" });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("UserName"))
            {
                Response.Cookies.Delete("UserName");
            }

            if (Request.Cookies.ContainsKey("FullName"))
            {
                Response.Cookies.Delete("FullName");
            }

            if (Request.Cookies.ContainsKey("SDT"))
            {
                Response.Cookies.Delete("SDT");
            }
            if (Request.Cookies.ContainsKey("ChiNhanhDangNhap"))
            {
                Response.Cookies.Delete("ChiNhanhDangNhap");
            }

            return RedirectToAction("Login", "Account");
        }


        private bool IsValidPassword(string password)
        {
            return password.Length >= 6;
        }

    }
}
