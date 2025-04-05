using Microsoft.AspNetCore.Mvc;
using SIMS_App.Models;
using SIMS_App.Services;
using SIMS_App.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SIMS_App.Controllers
{
    public class RecordController : Controller
    {
        private readonly SIMS_App.Services.RecordService _recordService; // Service xử lý bản ghi
        private readonly IDataService _dataService; // Service xử lý dữ liệu

        public RecordController(IDataService dataService)
        {
            _dataService = dataService;
            _recordService = new SIMS_App.Services.RecordService(dataService); // Khởi tạo service
            _recordService.AddObserver(new AttendanceObserver()); // Thêm observer cho service
        }

        public IActionResult Index() // Action mặc định
        {
            return RedirectToAction("ManageRecord"); // Chuyển hướng đến ManageRecord
        }

        public IActionResult ViewRecords() // Xem bản ghi điểm danh
        {
            Console.WriteLine("\n===== BẮT ĐẦU XỬ LÝ ViewRecords =====");

            // Lấy danh sách điểm danh của tất cả sinh viên có ID từ 1 đến 10
            List<Record> allStudentRecords = new List<Record>();

            for (int studentId = 1; studentId <= 10; studentId++)
            {
                Console.WriteLine($"⚡ Đang xử lý StudentId={studentId}");

                var studentRecords = _recordService.GetRecordsByStudentId(studentId); // Lấy bản ghi theo studentId
                if (studentRecords != null && studentRecords.Any())
                {
                    foreach (var record in studentRecords)
                    {
                        var classInfo = _dataService.GetClassById(record.ClassId); // Lấy thông tin lớp
                        record.ClassName = classInfo?.Name ?? "N/A";

                        if (classInfo != null)
                        {
                            var courseInfo = _dataService.GetCourseById(classInfo.CourseId); // Lấy thông tin khóa học
                            record.CourseName = courseInfo?.Name ?? "N/A";
                        }
                        else
                        {
                            record.CourseName = "N/A";
                        }
                    }

                    allStudentRecords.AddRange(studentRecords); // Thêm vào danh sách tổng
                }
                else
                {
                    Console.WriteLine($"❌ Không tìm thấy bản ghi nào cho StudentId={studentId}");
                }
            }

            // Tính toán tỷ lệ vắng mặt cho tất cả sinh viên
            Console.WriteLine("\n[2] Đang tính toán tỷ lệ vắng mặt...");
            double absenceRate = CalculateAbsenceRate(allStudentRecords); // Tính tỷ lệ vắng
            allStudentRecords.ForEach(r => r.AbsenceRate = absenceRate); // Gán tỷ lệ vắng cho từng bản ghi

            Console.WriteLine("\n===== KẾT THÚC XỬ LÝ =====\n");
            return View("~/Views/Student/ViewRecords.cshtml", allStudentRecords); // Trả về view
        }

        private double CalculateAbsenceRate(List<Record> records) // Tính tỷ lệ vắng mặt
        {
            if (records == null || records.Count == 0) return 0;

            int total = records.Count; // Tổng số bản ghi
            int absentCount = records.Count(r => !r.IsPresent); // Số lần vắng

            return (double)absentCount / total * 100; // Tính phần trăm
        }

        public IActionResult ManageRecord(int? classId) // Quản lý bản ghi điểm danh
        {
            var courses = _dataService.GetCourses(); // Lấy danh sách khóa học
            var classes = _dataService.GetClasses(); // Lấy danh sách lớp

            // Lấy danh sách điểm danh nếu có classId
            var records = classId.HasValue ? _recordService.GetStudentsByClass(classId.Value) : new List<Record>();

            ViewBag.Courses = courses; // Truyền dữ liệu qua ViewBag
            ViewBag.Classes = classes;
            ViewBag.SelectedClassId = classId;

            return View("~/Views/Admin/ManageRecord.cshtml", records); // Trả về view
        }

        [HttpPost]
        public IActionResult UpdateAttendance(int studentId, bool isPresent, int classId) // Cập nhật điểm danh
        {
            string studentName = _dataService.GetStudentName(studentId); // Lấy tên sinh viên
            if (string.IsNullOrEmpty(studentName))
            {
                return BadRequest("Student not found"); // Trả về lỗi nếu không tìm thấy
            }

            _recordService.SetAttendance(studentId, studentName, isPresent, classId); // Cập nhật điểm danh

            return RedirectToAction("ManageRecord", new { classId }); // Chuyển hướng về trang quản lý
        }
    }
}