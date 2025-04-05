using Microsoft.AspNetCore.Mvc;
using SIMS_App.Models;
using SIMS_App.Services;

namespace SIMS_App.Controllers
{
    [Route("User")] // Base route cho controller
    public class UserController : Controller
    {
        private readonly IUserService _userService; // Service xử lý người dùng

        public UserController(IUserService userService) // Constructor với dependency injection
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService)); // Khởi tạo service
        }

        [Route("AdminDashboard")] // Xử lý route /User/AdminDashboard
        public IActionResult AdminDashboard()
        {
            return View("~/Views/Admin/DashboardAdmin.cshtml"); // Trả về dashboard admin
        }

        [Route("DashboardTeacher")] // Xử lý route /User/DashboardTeacher
        public IActionResult DashboardTeacher()
        {
            return View("~/Views/Teacher/DashboardTeacher.cshtml"); // Trả về dashboard giáo viên
        }

        [Route("DashboardStudent")] // Xử lý route /User/DashboardStudent
        public IActionResult DashboardStudent()
        {
            return View("~/Views/Student/DashboardStudent.cshtml"); // Trả về dashboard sinh viên
        }

        [HttpGet]
        [Route("")]
        [Route("Index")] // Xử lý route /User hoặc /User/Index
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role"); // Lấy role từ session

            // Chuyển hướng theo role
            if (role == "Teacher")
            {
                return RedirectToAction("TeacherDashboard");
            }
            else if (role == "Admin")
            {
                return RedirectToAction("AdminDashboard");
            }

            return RedirectToAction("Login", "Auth"); // Mặc định chuyển đến trang đăng nhập
        }

        [HttpGet]
        [Route("GetUsers")] // Xử lý GET /User/GetUsers
        public JsonResult GetUsers()
        {
            var users = _userService.GetAllUsers(); // Lấy tất cả người dùng
            return Json(new { success = true, users }); // Trả về dạng JSON
        }

        [HttpPost]
        [Route("AddUser")] // Xử lý POST /User/AddUser
        public JsonResult AddUser([FromBody] User user)
        {
            if (user == null) // Kiểm tra dữ liệu đầu vào
                return Json(new { success = false, message = "Invalid data" });

            _userService.AddUser(user); // Thêm người dùng mới
            return Json(new { success = true, message = "User added successfully" }); // Trả về kết quả
        }

        [HttpPost]
        [Route("UpdateUser")] // Xử lý POST /User/UpdateUser
        public JsonResult UpdateUser([FromBody] User user)
        {
            if (user == null) // Kiểm tra dữ liệu đầu vào
                return Json(new { success = false, message = "Invalid data" });

            _userService.UpdateUser(user); // Cập nhật người dùng
            return Json(new { success = true, message = "User updated successfully" }); // Trả về kết quả
        }

        [HttpPost]
        [Route("DeleteUser")] // Xử lý POST /User/DeleteUser
        public JsonResult DeleteUser([FromBody] int id)
        {
            _userService.DeleteUser(id); // Xóa người dùng
            return Json(new { success = true, message = "User deleted successfully" }); // Trả về kết quả
        }
    }
}