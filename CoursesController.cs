using Microsoft.AspNetCore.Mvc;
using SIMS_App.Data;
using SIMS_App.Models;
using SIMS_App.Services;
using System.Collections.Generic;

namespace SIMS_App.Controllers
{
    [Route("Courses")] // Base route cho controller
    public class CourseController : Controller
    {
        private readonly CourseService _courseService; // Service xử lý khóa học

        public CourseController()
        {
            _courseService = new CourseService(); // Khởi tạo service
        }

        [Route("ManageCourses")] // Xử lý route /Courses/ManageCourses
        public IActionResult Index()
        {
            List<Course> courses = _courseService.GetCourses(); // Lấy danh sách khóa học
            return View("~/Views/Admin/ManageCourses.cshtml", courses); // Trả về view quản lý khóa học
        }

        [Route("ViewCourses")] // Xử lý route /Courses/ViewCourses
        public IActionResult ViewCourses()
        {
            var courses = _courseService.GetCourses(); // Lấy danh sách khóa học
            return View("~/Views/Student/ViewCourses.cshtml", courses); // Trả về view xem khóa học
        }

        [HttpPost]
        [Route("AddCourse")] // Xử lý POST /Courses/AddCourse
        public IActionResult AddCourse([FromBody] Course course)
        {
            _courseService.AddCourse(course); // Thêm khóa học mới
            return Json(new { success = true, message = "Course added successfully!" }); // Trả về kết quả JSON
        }

        [HttpPut]
        [Route("UpdateCourse")] // Xử lý PUT /Courses/UpdateCourse
        public IActionResult UpdateCourse([FromBody] Course course)
        {
            _courseService.UpdateCourse(course); // Cập nhật khóa học
            return Json(new { success = true, message = "Course updated successfully!" }); // Trả về kết quả JSON
        }

        [HttpDelete]
        [Route("DeleteCourse")] // Xử lý DELETE /Courses/DeleteCourse
        public IActionResult DeleteCourse(int id)
        {
            _courseService.DeleteCourse(id); // Xóa khóa học
            return Json(new { success = true, message = "Course deleted successfully!" }); // Trả về kết quả JSON
        }

        [HttpGet]
        [Route("ViewStudents")] // Xử lý GET /Courses/ViewStudents
        public IActionResult ViewStudents(int id)
        {
            Console.WriteLine($"Received class ID: {id}");
            var students = _courseService.GetStudentsByClassId(id); // Lấy danh sách sinh viên trong lớp

            if (students != null && students.Any()) // Nếu có sinh viên
            {
                Console.WriteLine($"Total students found: {students.Count}");
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
            else // Nếu không có sinh viên
            {
                Console.WriteLine("No students found for this class.");
                return Json(new
                {
                    success = false,
                    message = "No students found in this course."
                });
            }
        }
    }
}