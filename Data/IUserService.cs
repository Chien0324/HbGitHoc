using System.Collections.Generic;
using SIMS_App.Models;

namespace SIMS_App.Services
{
    public interface IUserService // Interface cho UserService
    {
        List<User> GetAllUsers(); // Lấy tất cả người dùng
        User GetUserById(int id); // Lấy người dùng theo ID
        void AddUser(User user); // Thêm người dùng mới
        void UpdateUser(User user); // Cập nhật người dùng
        void DeleteUser(int id); // Xóa người dùng
    }
}