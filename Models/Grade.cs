using System.ComponentModel.DataAnnotations;

namespace SIMS_App.Models
{
    // Model điểm số
    public class Grade
    {
        public int StudentId { get; set; } // ID sinh viên

        [Required] // Bắt buộc nhập
        public string StudentName { get; set; } // Tên sinh viên

        [Required]
        public string Subject { get; set; } // Môn học

        [Required]
        public string ClassName { get; set; } // Tên lớp

        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")] // Giới hạn điểm
        public double Score { get; set; } // Điểm số

        [Required]
        public string Type { get; set; } // Loại điểm (kiểm tra, thi...)
    }

    // Interface cho Decorator Pattern định dạng điểm
    public interface IGradeFormatter
    {
        string FormatGrade(Grade grade); // Phương thức định dạng điểm
    }

    // Lớp định dạng điểm cơ bản (thang 10)
    public class BaseGradeFormatter : IGradeFormatter
    {
        public virtual string FormatGrade(Grade grade)
        {
            return $"{grade.Score}/10"; // Định dạng điểm thang 10
        }
    }

    // Lớp Decorator định dạng điểm thi (thang 100)
    public class ExamGradeFormatter : BaseGradeFormatter
    {
        public override string FormatGrade(Grade grade)
        {
            return $"{grade.Score * 10}/100"; // Định dạng điểm thang 100
        }
    }
}