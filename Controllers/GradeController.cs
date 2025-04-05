using Microsoft.AspNetCore.Mvc;
using SIMS_App.Data;
using SIMS_App.Models;
using System.Collections.Generic;

namespace SIMS_App.Controllers
{
    [Route("Grade")] // Base route cho controller
    public class GradeController : Controller
    {
        private readonly GradeService _gradeService; // Service xử lý điểm số

        public GradeController()
        {
            _gradeService = new GradeService(); // Khởi tạo service
        }

        [Route("ManageGrade")] // Xử lý route /Grade/ManageGrade
        public IActionResult ManageGrade()
        {
            var grades = _gradeService.GetGrades(); // Lấy danh sách điểm
            return View("~/Views/Teacher/ManageGrade.cshtml", grades); // Trả về view quản lý điểm
        }

        [Route("ViewGrade")] // Xử lý route /Grade/ViewGrade
        public IActionResult ViewGrade()
        {
            var grades = _gradeService.GetGrades(); // Lấy danh sách điểm
            return View("~/Views/Student/ViewGrade.cshtml", grades); // Trả về view xem điểm
        }

        [HttpPost]
        [Route("AddGrade")] // Xử lý POST /Grade/AddGrade
        public IActionResult AddGrade([FromBody] Grade grade)
        {
            if (grade == null) // Kiểm tra dữ liệu đầu vào
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            _gradeService.AddGrade(grade); // Thêm điểm mới
            return Json(new { success = true, message = "Grade added successfully!" }); // Trả về kết quả
        }

        [HttpPost]
        [Route("UpdateGrade")] // Xử lý POST /Grade/UpdateGrade
        public IActionResult UpdateGrade([FromBody] Grade grade)
        {
            if (grade == null) // Kiểm tra dữ liệu đầu vào
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            var existingGrade = _gradeService.GetGrades().FirstOrDefault(g => g.StudentId == grade.StudentId && g.Subject == grade.Subject);
            if (existingGrade != null) // Nếu tìm thấy điểm cần cập nhật
            {
                _gradeService.UpdateGrade(grade); // Cập nhật điểm
                return Json(new { success = true, message = "Grade updated successfully!" });
            }
            else // Nếu không tìm thấy
            {
                return Json(new { success = false, message = "Grade not found!" });
            }
        }

        [HttpPost]
        [Route("DeleteGrade")] // Xử lý POST /Grade/DeleteGrade
        public IActionResult DeleteGrade([FromBody] Grade grade)
        {
            if (grade == null) // Kiểm tra dữ liệu đầu vào
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            _gradeService.DeleteGrade(grade.StudentId, grade.Subject); // Xóa điểm
            return Json(new { success = true, message = "Grade deleted successfully!" }); // Trả về kết quả
        }
    }
}