using System.ComponentModel.DataAnnotations;

namespace SIMS_App.Models
{
    // Model thời khóa biểu
    public class Timetable
    {
        public int Id { get; set; } // ID thời khóa biểu

        [Required]
        public string StudentName { get; set; } // Tên sinh viên

        [Required]
        public string Subject { get; set; } // Môn học

        [Required]
        public string ClassName { get; set; } // Tên lớp

        [Required]
        public string TeacherName { get; set; } // Tên giáo viên

        [Required]
        public string StartTime { get; set; } // Thời gian bắt đầu (dạng string)

        [Required]
        public string EndTime { get; set; } // Thời gian kết thúc (dạng string)
    }
}