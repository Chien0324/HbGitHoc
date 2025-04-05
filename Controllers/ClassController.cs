using Microsoft.AspNetCore.Mvc;
using SIMS_App.Data;
using SIMS_App.Models;
using System.Linq;

namespace SIMS_App.Controllers
{
    public class ClassController : Controller
    {
        private readonly IDataService _dataService; // Service xử lý dữ liệu

        public ClassController(IDataService dataService) // Constructor với dependency injection
        {
            _dataService = dataService; // Khởi tạo data service
        }

        public IActionResult ManageClass() // Quản lý lớp học (cho admin)
        {
            var classes = _dataService.GetClasses(); // Lấy danh sách lớp học
            return View("~/Views/Admin/ManageClass.cshtml", classes); // Trả về view quản lý lớp
        }

        [HttpGet]
        public IActionResult ViewClass() // Xem lớp học (cho sinh viên)
        {
            var classes = _dataService.GetClasses(); // Lấy danh sách lớp học
            return View("~/Views/Student/ViewClass.cshtml", classes); // Trả về view xem lớp
        }

        [HttpPost]
        public JsonResult AddClass(string name, int courseId) // Thêm lớp học mới
        {
            try
            {
                var newClass = new Class // Tạo đối tượng lớp mới
                {
                    Name = name,
                    CourseId = courseId
                };

                _dataService.AddClass(newClass); // Thêm lớp vào hệ thống

                return Json(new // Trả về kết quả JSON
                {
                    success = true,
                    message = "Class added successfully!",
                    newClassId = newClass.Id // Trả về ID mới tạo
                });
            }
            catch (Exception ex)
            {
                return Json(new // Trả về lỗi nếu có
                {
                    success = false,
                    message = $"Error adding class: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult Edit(int id, string name, int courseId) // Chỉnh sửa lớp học
        {
            var classItem = _dataService.GetClassById(id); // Lấy lớp theo ID
            if (classItem != null) // Nếu tìm thấy lớp
            {
                classItem.Name = name; // Cập nhật tên
                classItem.CourseId = courseId; // Cập nhật ID khóa học
                _dataService.UpdateClass(classItem); // Lưu thay đổi
                return Json(new { success = true, message = "Class updated successfully!" });
            }
            return Json(new { success = false, message = "Class not found!" }); // Trả về lỗi nếu không tìm thấy
        }

        [HttpPost]
        public JsonResult DeleteClass(int id) // Xóa lớp học
        {
            _dataService.DeleteClass(id); // Xóa lớp
            return Json(new { success = true, message = "Class deleted successfully!" }); // Trả về kết quả
        }

        [HttpGet]
        public JsonResult ViewStudents(int id) // Xem danh sách sinh viên trong lớp
        {
            Console.WriteLine($"Received Class ID: {id}");

            try
            {
                var students = _dataService.GetStudentsByClassId(id); // Lấy danh sách sinh viên

                if (students == null || !students.Any()) // Nếu không có sinh viên
                {
                    return Json(new { success = false, message = "No students found in this class!" });
                }

                return Json(new // Trả về danh sách sinh viên dạng JSON
                {
                    success = true,
                    students = students.Select(s => new {
                        id = s.StudentId,
                        name = s.Name,
                        email = s.Email
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "Error fetching students!" }); // Trả về lỗi nếu có
            }
        }
    }
}