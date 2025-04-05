using SIMS_App.Models;
using System.Text.Json;

namespace SIMS_App.Services
{
    public class UserService : IUserService // Service quản lý người dùng
    {
        private readonly string filePath = "Resources/Student.json"; // File lưu user

        public UserService()
        {
            Console.WriteLine($"🔍 UserService initialized. File path: {filePath}");
        }

        public List<User> GetUsers() // Lấy danh sách user
        {
            if (!File.Exists(filePath))
                return new List<User>();

            var jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(jsonData) ?? new List<User>();
        }

        public List<User> GetAllUsers() // Tương tự GetUsers()
        {
            return GetUsers();
        }

        public User? GetUserById(int id) // Lấy user theo ID
        {
            return GetUsers().FirstOrDefault(u => u.Id == id);
        }

        public void AddUser(User user) // Thêm user mới
        {
            var users = GetUsers();
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1; // Tạo ID mới
            users.Add(user);
            SaveUsers(users); // Lưu thay đổi
        }

        public void UpdateUser(User user) // Cập nhật user
        {
            var users = GetUsers();
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);

            if (existingUser != null)
            {
                // Cập nhật thông tin
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.DateOfBirth = user.DateOfBirth;
                SaveUsers(users);
            }
        }

        public void DeleteUser(int id) // Xóa user
        {
            var users = GetUsers().Where(u => u.Id != id).ToList();
            SaveUsers(users);
        }

        private void SaveUsers(List<User> users) // Lưu danh sách user
        {
            var jsonData = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }
    }
}