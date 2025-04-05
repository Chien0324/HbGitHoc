using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SIMS_App.Models;

namespace SIMS_App.Services
{
    public class AuthService // Service xử lý xác thực người dùng
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Users.json"); // Đường dẫn file lưu user

        public User ValidateUser(string username, string password) // Xác thực đăng nhập
        {
            var users = LoadUsers(); // Tải danh sách user
            return users.Find(u => u.Username == username && u.Password == password); // Tìm user khớp
        }

        public bool RegisterUser(RegisterModel model) // Đăng ký user mới
        {
            var users = LoadUsers();

            if (users.Exists(u => u.Username == model.Username)) // Kiểm tra username tồn tại
            {
                return false;
            }

            users.Add(new User // Thêm user mới
            {
                Username = model.Username,
                Password = model.Password,
                Role = model.Role
            });

            SaveUsers(users); // Lưu danh sách user
            return true;
        }

        private List<User> LoadUsers() // Tải danh sách user từ file
        {
            if (!File.Exists(_filePath)) // Kiểm tra file tồn tại
            {
                Console.WriteLine("Users.json not found, returning empty list.");
                return new List<User>();
            }

            var json = File.ReadAllText(_filePath).Trim();
            Console.WriteLine($"JSON content read from file: {json}"); // Log nội dung file

            if (string.IsNullOrEmpty(json) || json == "null") // Kiểm tra nội dung rỗng
            {
                Console.WriteLine("JSON file is empty or contains null, resetting to empty list.");
                return new List<User>();
            }

            try
            {
                var users = JsonConvert.DeserializeObject<List<User>>(json); // Chuyển đổi JSON
                if (users == null)
                {
                    Console.WriteLine("Deserialization returned null, resetting to empty list.");
                    return new List<User>();
                }
                return users;
            }
            catch (JsonException ex) // Xử lý lỗi JSON
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}. Resetting file.");
                ResetUsersFile(); // Reset file nếu lỗi
                return new List<User>();
            }
        }

        private void ResetUsersFile() // Reset file user về mặc định
        {
            var defaultUsers = new List<User>
            {
                new User { Username = "admin", Password = "123", Role = "Admin" },
                new User { Username = "user1", Password = "password123", Role = "User" }
            };

            SaveUsers(defaultUsers);
            Console.WriteLine("Users.json was reset to default users.");
        }

        private void SaveUsers(List<User> users) // Lưu danh sách user
        {
            try
            {
                var json = JsonConvert.SerializeObject(users, Formatting.Indented); // Chuyển sang JSON
                File.WriteAllText(_filePath, json);
                Console.WriteLine("User data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
            }
        }
    }
}