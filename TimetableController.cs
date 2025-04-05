using Microsoft.AspNetCore.Mvc;
using SIMS_App.Data;
using SIMS_App.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace SIMS_App.Controllers
{
    [Route("Timetable")] // Base route cho controller
    public class TimetableController : Controller
    {
        private readonly TimetableService _timetableService; // Service xử lý thời khóa biểu

        public TimetableController()
        {
            _timetableService = new TimetableService(); // Khởi tạo service
        }

        [Route("ManageTimetable")] // Xử lý route /Timetable/ManageTimetable
        public IActionResult ManageTimetable()
        {
            var timetables = _timetableService.GetTimetables(); // Lấy danh sách thời khóa biểu
            return View("~/Views/Admin/ManageTimetable.cshtml", timetables); // Trả về view quản lý
        }

        [Route("ViewTimetable")] // Xử lý route /Timetable/ViewTimetable
        public IActionResult ViewTimetable()
        {
            var timetables = _timetableService.GetTimetables(); // Lấy danh sách thời khóa biểu
            return View("~/Views/Student/ViewTimetable.cshtml", timetables); // Trả về view xem
        }

        [HttpPost]
        [Route("AddTimetable")] // Xử lý POST /Timetable/AddTimetable
        public IActionResult AddTimetable([FromBody] Timetable timetable)
        {
            if (timetable == null) // Kiểm tra dữ liệu đầu vào
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            _timetableService.AddTimetable(timetable); // Thêm thời khóa biểu mới
            return Json(new { success = true, message = "Timetable added successfully!" }); // Trả về kết quả
        }

        [HttpPost]
        [Route("UpdateTimetable")] // Xử lý POST /Timetable/UpdateTimetable
        public IActionResult UpdateTimetable([FromBody] Timetable timetable)
        {
            if (timetable == null) // Kiểm tra dữ liệu đầu vào
            {
                return Json(new { success = false, message = "Invalid data!" });
            }

            _timetableService.UpdateTimetable(timetable); // Cập nhật thời khóa biểu
            return Json(new { success = true, message = "Timetable updated successfully!" }); // Trả về kết quả
        }

        [HttpPost]
        [Route("DeleteTimetable")] // Xử lý POST /Timetable/DeleteTimetable
        public IActionResult DeleteTimetable([FromBody] JsonElement data)
        {
            try
            {
                Console.WriteLine($"Raw data received: {data}"); // Debug

                if (!data.TryGetProperty("id", out JsonElement idElement)) // Lấy ID từ dữ liệu
                {
                    return Json(new { success = false, message = "Invalid request: ID missing!" });
                }

                int id = idElement.GetInt32(); // Chuyển đổi ID sang số
                Console.WriteLine($"Extracted ID: {id}");

                bool deleted = _timetableService.DeleteTimetable(id); // Xóa thời khóa biểu

                if (deleted)
                    return Json(new { success = true, message = "Timetable deleted successfully!" });
                else
                    return Json(new { success = false, message = $"Timetable not found! ID: {id}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" }); // Trả về lỗi nếu có
            }
        }
    }
}