using System.ComponentModel.DataAnnotations;

namespace SIMS_App.Models
{
    // Model cho đăng nhập
    public class LoginModel
    {
        [Required] // Yêu cầu bắt buộc
        public string Username { get; set; } // Tên đăng nhập

        [Required]
        [DataType(DataType.Password)] // Kiểu dữ liệu mật khẩu
        public string Password { get; set; } // Mật khẩu
    }

    // Model cho đăng ký
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; } // Tên đăng nhập

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Mật khẩu

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")] // So sánh với Password
        public string ConfirmPassword { get; set; } // Xác nhận mật khẩu

        [Required]
        public string Role { get; set; } // Vai trò (Admin, Teacher, Student)
    }
}