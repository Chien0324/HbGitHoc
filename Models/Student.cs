namespace SIMS_App.Models
{
    // Model sinh viên
    public class Student
    {
        public int StudentId { get; set; } // ID sinh viên
        public string Name { get; set; } // Tên sinh viên
        public int Age { get; set; } // Tuổi
        public string Email { get; set; } // Email
        public int ClassId { get; set; } // ID lớp
    }
}