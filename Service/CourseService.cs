using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SIMS_App.Models;

namespace SIMS_App.Services
{
    public class CourseService // Service quản lý khóa học
    {
        private const string FilePath = "Resources/Course.json"; // Đường dẫn file
        private List<Course> courses; // Danh sách khóa học

        public CourseService()
        {
            LoadCourses(); // Tải dữ liệu khi khởi tạo
        }

        private void LoadCourses() // Tải danh sách khóa học
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);

                    if (string.IsNullOrWhiteSpace(json)) // Kiểm tra JSON rỗng
                    {
                        courses = new List<Course>();
                        return;
                    }

                    courses = JsonSerializer.Deserialize<List<Course>>(json) ?? new List<Course>(); // Chuyển đổi JSON
                }
                else
                {
                    courses = new List<Course>(); // Tạo mới nếu file không tồn tại
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc file JSON: {ex.Message}");
                courses = new List<Course>();
            }
        }

        private void SaveCourses() // Lưu danh sách khóa học
        {
            try
            {
                string json = JsonSerializer.Serialize(courses, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi file JSON: {ex.Message}");
            }
        }

        public List<Course> GetCourses() => courses; // Lấy danh sách khóa học

        public Course GetCourseById(int courseId) => courses.FirstOrDefault(c => c.CourseId == courseId); // Lấy khóa học theo ID

        public void AddCourse(Course course) // Thêm khóa học mới
        {
            course.CourseId = courses.Any() ? courses.Max(c => c.CourseId) + 1 : 1; // Tạo ID mới
            courses.Add(course);
            SaveCourses(); // Lưu thay đổi
        }

        public void UpdateCourse(Course updatedCourse) // Cập nhật khóa học
        {
            var existingCourse = GetCourseById(updatedCourse.CourseId);
            if (existingCourse != null)
            {
                // Cập nhật thông tin
                existingCourse.Name = updatedCourse.Name;
                existingCourse.TotalStudents = updatedCourse.TotalStudents;
                existingCourse.StartDate = updatedCourse.StartDate;
                existingCourse.EndDate = updatedCourse.EndDate;
                existingCourse.NumberOfSessions = updatedCourse.NumberOfSessions;
                SaveCourses();
            }
        }

        public void DeleteCourse(int courseId) // Xóa khóa học
        {
            var course = GetCourseById(courseId);
            if (course != null)
            {
                courses.Remove(course);
                SaveCourses();
            }
        }

        public List<Student> GetStudentsByClassId(int classId) // Lấy sinh viên theo lớp
        {
            var students = new List<Student>();
            string filePath = "Resources/Student.CSV";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("⚠ Student file not found.");
                return students;
            }

            var lines = File.ReadAllLines(filePath);
            Console.WriteLine($"📌 Total lines in CSV: {lines.Length}");

            // Bỏ qua dòng header nếu có
            var startLine = lines[0].StartsWith("StudentId,Name,Age,Email,ClassId") ? 1 : 0;

            for (int i = startLine; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var data = line.Split(',');
                if (data.Length < 5)
                {
                    Console.WriteLine($"⚠ Invalid data format in line {i + 1}: {line}");
                    continue;
                }

                try
                {
                    if (!int.TryParse(data[4], out int studentClassId) || studentClassId != classId)
                        continue;

                    var student = new Student // Tạo đối tượng sinh viên
                    {
                        StudentId = int.Parse(data[0]),
                        Name = data[1].Trim(),
                        Age = int.Parse(data[2]),
                        Email = data[3].Trim(),
                        ClassId = studentClassId
                    };
                    students.Add(student);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠ Error processing line {i + 1}: {ex.Message}");
                }
            }

            Console.WriteLine($"📌 Found {students.Count} students for class {classId}");
            return students;
        }
    }
}