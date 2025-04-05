using System;

namespace SIMS_App.Models
{
    // Interface định nghĩa Observer cho hệ thống điểm danh
    public interface IAttendanceObserver
    {
        void UpdateAttendance(Record record); // Phương thức cập nhật trạng thái điểm danh
    }

    // Lớp triển khai IAttendanceObserver để theo dõi điểm danh
    public class AttendanceObserver : IAttendanceObserver
    {
        public void UpdateAttendance(Record record) // Cập nhật và hiển thị trạng thái điểm danh
        {
            Console.WriteLine($"Attendance status of {record.StudentName}: {(record.IsPresent ? "✅ Present" : "❌ Absent")}");
            // Hiển thị thông báo trạng thái điểm danh của sinh viên
        }
    }
}