using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SIMS_App.Models;

namespace SIMS_App.Data
{
    public class TimetableService // Service quản lý thời khóa biểu
    {
        private const string FilePath = "Resources/Timetable.json"; // File lưu thời khóa biểu
        private List<Timetable> _timetables; // Danh sách thời khóa biểu

        public TimetableService()
        {
            LoadTimetables(); // Tải dữ liệu khi khởi tạo
        }

        private void LoadTimetables() // Tải thời khóa biểu từ file
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                Console.WriteLine($"Loaded JSON: {json}"); // Log nội dung file
                _timetables = JsonSerializer.Deserialize<List<Timetable>>(json) ?? new List<Timetable>();

                foreach (var timetable in _timetables) // Đảm bảo ID là số nguyên
                {
                    timetable.Id = Convert.ToInt32(timetable.Id);
                }
            }
            else
            {
                _timetables = new List<Timetable>();
            }
        }

        public List<Timetable> GetTimetables() => _timetables; // Lấy danh sách thời khóa biểu

        public void AddTimetable(Timetable timetable) // Thêm thời khóa biểu mới
        {
            if (_timetables.Count > 0)
            {
                timetable.Id = _timetables.Max(t => t.Id) + 1; // Tạo ID mới
            }
            else
            {
                timetable.Id = 1; // Bắt đầu từ 1 nếu danh sách rỗng
            }

            _timetables.Add(timetable);
            SaveTimetables(); // Lưu thay đổi
        }

        public void UpdateTimetable(Timetable updatedTimetable) // Cập nhật thời khóa biểu
        {
            var existingTimetable = _timetables.FirstOrDefault(t => t.Id == updatedTimetable.Id);
            if (existingTimetable != null)
            {
                // Cập nhật thông tin
                existingTimetable.StudentName = updatedTimetable.StudentName;
                existingTimetable.Subject = updatedTimetable.Subject;
                existingTimetable.ClassName = updatedTimetable.ClassName;
                existingTimetable.TeacherName = updatedTimetable.TeacherName;
                existingTimetable.StartTime = updatedTimetable.StartTime;
                existingTimetable.EndTime = updatedTimetable.EndTime;
                SaveTimetables();
            }
        }

        public bool DeleteTimetable(int id) // Xóa thời khóa biểu
        {
            var timetable = _timetables.FirstOrDefault(t => t.Id == id);
            if (timetable != null)
            {
                _timetables.Remove(timetable);
                SaveTimetables();
                return true;
            }
            return false;
        }

        private void SaveTimetables() // Lưu danh sách thời khóa biểu
        {
            string json = JsonSerializer.Serialize(_timetables, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}