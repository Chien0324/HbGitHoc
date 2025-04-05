using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SIMS_App.Models;
using SIMS_App.Services;

namespace SIMS_App.Controllers
{
    [Route("Auth")] // Định nghĩa base route cho controller là "Auth"
    public class AuthController : Controller
    {
        private readonly AuthService _authService; // Khai báo service xử lý authentication

        public AuthController()
        {
            _authService = new AuthService(); // Khởi tạo AuthService
        }

        [HttpGet("Login")] // Xử lý request GET đến /Auth/Login
        public IActionResult Login()
        {
            return View(); // Trả về view đăng nhập
        }

        [HttpPost("Login")] // Xử lý request POST đến /Auth/Login
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid) // Kiểm tra dữ liệu nhập có hợp lệ
            {
                var user = _authService.ValidateUser(model.Username, model.Password); // Xác thực người dùng

                if (user == null) // Nếu xác thực thất bại
                {
                    Console.WriteLine($"Login failed for user: {model.Username}");
                    ModelState.AddModelError("", "Invalid username or password."); // Thêm lỗi vào ModelState
                    return View(model); // Trả về view với thông báo lỗi
                }

                // Lưu thông tin user vào Session
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                Console.WriteLine($"Login successful for user: {user.Username}");

                // Chuyển hướng sau khi đăng nhập thành công
                return RedirectToAction("Index", "Home");

                if (user != null) // Đoạn code thừa (không bao giờ chạy tới)
                {
                    HttpContext.Session.SetString("Role", user.Role);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid credentials"); // Thêm lỗi vào ModelState
            }

            return View(model); // Trả về view nếu dữ liệu không hợp lệ
        }

        [HttpGet("Register")] // Xử lý request GET đến /Auth/Register
        public IActionResult Register()
        {
            return View(); // Trả về view đăng ký
        }

        [HttpPost("Register")] // Xử lý request POST đến /Auth/Register
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid) // Kiểm tra dữ liệu nhập có hợp lệ
            {
                var result = _authService.RegisterUser(model); // Đăng ký người dùng
                if (result) // Nếu đăng ký thành công
                {
                    return RedirectToAction("Login"); // Chuyển hướng đến trang đăng nhập
                }
                ModelState.AddModelError("", "Registration failed. Username may already exist."); // Thêm lỗi nếu đăng ký thất bại
            }
            return View(model); // Trả về view nếu dữ liệu không hợp lệ

            // Đoạn code thừa (không bao giờ chạy tới)
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout() // Xử lý đăng xuất
        {
            HttpContext.Session.Clear(); // Xóa tất cả session
            return RedirectToAction("Login"); // Chuyển hướng đến trang đăng nhập
        }
    }
}