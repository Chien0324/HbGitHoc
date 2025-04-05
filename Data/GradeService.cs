using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SIMS_App.Models;

namespace SIMS_App.Data
{
    public class GradeService // Service quản lý điểm số
    {
        private const string FilePath = "Resources/Grade.json"; // Đường dẫn file lưu điểm
        private List<Grade> _grades; // Danh sách điểm

        public GradeService() // Constructor
        {
            LoadGrades(); // Tải dữ liệu khi khởi tạo
        }

        private void LoadGrades() // Tải dữ liệu từ file
        {
            if (File.Exists(FilePath)) // Kiểm tra file tồn tại
            {
                string json = File.ReadAllText(FilePath); // Đọc file
                _grades = JsonSerializer.Deserialize<List<Grade>>(json) ?? new List<Grade>(); // Chuyển đổi JSON
            }
            else
            {
                _grades = new List<Grade>(); // Tạo mới nếu file không tồn tại
            }
        }

        public List<Grade> GetGrades() => _grades; // Lấy danh sách điểm

        public void AddGrade(Grade grade) // Thêm điểm mới
        {
            if (_grades.Count > 0) // Nếu danh sách không rỗng
            {
                int newId = _grades.Max(g => g.StudentId) + 1; // Tạo ID mới
                grade.StudentId = newId;
            }
            else
            {
                grade.StudentId = 1; // Bắt đầu từ 1 nếu danh sách rỗng
            }

            _grades.Add(grade); // Thêm vào danh sách
            SaveGrades(); // Lưu thay đổi
        }

        public void UpdateGrade(Grade updatedGrade) // Cập nhật điểm
        {
            var existingGrade = _grades.FirstOrDefault(g => g.StudentId == updatedGrade.StudentId && g.Subject == updatedGrade.Subject);
            if (existingGrade != null) // Nếu tìm thấy
            {
                // Cập nhật tất cả thông tin
                existingGrade.StudentName = updatedGrade.StudentName;
                existingGrade.ClassName = updatedGrade.ClassName;
                existingGrade.Score = updatedGrade.Score;
                existingGrade.Type = updatedGrade.Type;
                SaveGrades(); // Lưu thay đổi
            }
        }

        public void DeleteGrade(int studentId, string subject) // Xóa điểm
        {
            var grade = _grades.Find(g => g.StudentId == studentId && g.Subject == subject); // Tìm điểm
            if (grade != null) // Nếu tìm thấy
            {
                _grades.Remove(grade); // Xóa khỏi danh sách
                SaveGrades(); // Lưu thay đổi
            }
        }

        private void SaveGrades() // Lưu danh sách điểm
        {
            string json = JsonSerializer.Serialize(_grades, new JsonSerializerOptions { WriteIndented = true }); // Chuyển đổi sang JSON
            File.WriteAllText(FilePath, json); // Ghi ra file
        }
    }
}