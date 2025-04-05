using System;

namespace SIMS_App.Models
{
    // Model bản ghi điểm danh
    public class Record
    {
        public int StudentId { get; set; } // ID sinh viên
        public int ClassId { get; set; } // ID lớp
        public string StudentName { get; set; } // Tên sinh viên
        public bool IsPresent { get; set; } // Trạng thái điểm danh (true: có mặt)
        public string ClassName { get; set; } // Tên lớp  
        public string CourseName { get; set; } // Tên khóa học
        public double AbsenceRate { get; set; } // Tỷ lệ vắng mặt (%)
        public DateTime Date { get; set; } // Ngày điểm danh
    }
}