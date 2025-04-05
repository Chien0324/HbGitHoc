using Microsoft.AspNetCore.Mvc;

namespace SIMS_App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() // Xử lý trang chủ
        {
            var role = HttpContext.Session.GetString("Role") ?? "Guest"; // Lấy role từ session

            // Chuyển hướng theo role
            if (role == "Teacher")
            {
                return RedirectToAction("DashboardTeacher", "User");
            }
            else if (role == "Admin")
            {
                return RedirectToAction("AdminDashboard", "User");
            }
            else if (role == "Student")
            {
                return RedirectToAction("DashboardStudent", "User");
            }

            return RedirectToAction("Login", "Auth"); // Mặc định chuyển đến trang đăng nhập
        }
    }
}