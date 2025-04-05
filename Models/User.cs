namespace SIMS_App.Models
{
    // Model người dùng
    public class User
    {
        public int Id { get; set; } // ID người dùng
        public int UserId { get; set; } // ID người dùng (có thể thừa)
        public string Username { get; set; } // Tên đăng nhập
        public string Password { get; set; } // Mật khẩu
        public string Name { get; set; } // Họ tên
        public string Email { get; set; } // Email
        public string Phone { get; set; } // Số điện thoại
        public DateTime DateOfBirth { get; set; } // Ngày sinh
        public string Role { get; set; } // Vai trò (Admin, Teacher, Student)

        public int StudentId { get; set; } // ID sinh viên (nếu là sinh viên)
    }
}