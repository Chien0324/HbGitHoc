namespace SIMS_App.Models
{
    // Model khóa học
    public class Course
    {
        public int CourseId { get; set; } // ID khóa học
        public string Name { get; set; } // Tên khóa học
        public string Description { get; set; } // Mô tả khóa học
        public int TotalStudents { get; set; } // Tổng số sinh viên
        public DateTime StartDate { get; set; } // Ngày bắt đầu
        public DateTime EndDate { get; set; } // Ngày kết thúc
        public int NumberOfSessions { get; set; } // Số buổi học
        public List<Class> Classes { get; set; } = new List<Class>(); // Danh sách lớp thuộc khóa học
    }
}