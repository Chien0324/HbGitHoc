namespace SIMS_App.Models
{
    // Model lớp học
    public class Class
    {
        private static int _nextId = 1; // Biến tĩnh để tự động tăng ID
        public int Id { get; set; } // ID lớp
        public int ClassId { get; set; } // ID lớp (có thể thừa)
        public string Name { get; set; } // Tên lớp
        public int CourseId { get; set; } // ID khóa học mà lớp thuộc về

        public List<User> Students { get; set; } = new List<User>(); // Danh sách sinh viên trong lớp
        public int StudentId { get; set; } // ID sinh viên (có thể thừa)
    }
}