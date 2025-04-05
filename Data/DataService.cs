using SIMS_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace SIMS_App.Data
{
    public class DataService : IDataService // Triển khai interface IDataService
    {
        private static List<Course> courses = new List<Course> // Danh sách khóa học (hiện đang trống)
        {
        };

        private List<Class> classes; // Danh sách lớp học
        private const string FilePath = "Resources/Classes.json"; // Đường dẫn file lưu trữ lớp học

        public DataService() // Constructor
        {
            LoadClasses(); // Tải dữ liệu lớp học khi khởi tạo
        }

        private void LoadClasses() // Phương thức tải dữ liệu lớp học từ file
        {
            if (File.Exists(FilePath)) // Kiểm tra file tồn tại
            {
                string json = File.ReadAllText(FilePath); // Đọc toàn bộ nội dung file
                classes = JsonSerializer.Deserialize<List<Class>>(json) ?? new List<Class>(); // Chuyển đổi JSON sang danh sách lớp
            }
            else
            {
                classes = new List<Class>(); // Nếu file không tồn tại, tạo danh sách mới
            }
        }

        public List<Student> GetStudentsByClassId(int classId) // Lấy danh sách sinh viên theo classId
        {
            var students = new List<Student>(); // Khởi tạo danh sách sinh viên
            string filePath = "Resources/Student.CSV"; // Đường dẫn file CSV

            if (!File.Exists(filePath)) // Kiểm tra file tồn tại
            {
                Console.WriteLine("Student file not found.");
                return students; // Trả về danh sách rỗng nếu file không tồn tại
            }

            var lines = File.ReadAllLines(filePath); // Đọc tất cả dòng từ file
            foreach (var line in lines.Skip(1)) // Bỏ qua dòng tiêu đề
            {
                var data = line.Split(','); // Tách dữ liệu bằng dấu phẩy
                if (data.Length >= 5 && int.TryParse(data[4], out int studentClassId) && studentClassId == classId) // Kiểm tra dữ liệu hợp lệ
                {
                    students.Add(new Student // Thêm sinh viên mới vào danh sách
                    {
                        StudentId = int.Parse(data[0]),
                        Name = data[1],
                        Age = int.Parse(data[2]),
                        Email = data[3],
                        ClassId = studentClassId
                    });
                }
            }

            Console.WriteLine($"Total students found in class {classId}: {students.Count}");
            return students; // Trả về danh sách sinh viên
        }

        private void SaveClasses() // Lưu danh sách lớp học ra file
        {
            string json = JsonSerializer.Serialize(classes, new JsonSerializerOptions { WriteIndented = true }); // Chuyển đổi sang JSON
            File.WriteAllText(FilePath, json); // Ghi ra file
        }

        public string GetStudentName(int studentId) // Lấy tên sinh viên theo ID
        {
            var student = classes.SelectMany(c => c.Students).FirstOrDefault(s => s.Id == studentId); // Tìm sinh viên trong tất cả lớp
            return student?.Name ?? string.Empty; // Trả về tên hoặc chuỗi rỗng nếu không tìm thấy
        }

        public List<Course> GetCourses() => courses; // Lấy danh sách khóa học

        public Course GetCourseById(int courseId) // Lấy khóa học theo ID
        {
            Console.WriteLine($"\n🔍 Đang tìm khóa học với CourseId: {courseId}");
            var result = courses.FirstOrDefault(c => c.CourseId == courseId); // Tìm khóa học
            Console.WriteLine($"Kết quả: {(result != null ? $"Tìm thấy: {result.Name}" : "Không tìm thấy")}");
            return result; // Trả về kết quả
        }

        public void AddCourse(Course course) // Thêm khóa học mới
        {
            course.CourseId = courses.Max(c => c.CourseId) + 1; // Tạo ID mới
            courses.Add(course); // Thêm vào danh sách
        }

        public void UpdateCourse(Course course) // Cập nhật khóa học
        {
            var existingCourse = GetCourseById(course.CourseId); // Tìm khóa học hiện có
            if (existingCourse != null) // Nếu tìm thấy
            {
                // Cập nhật tất cả thông tin
                existingCourse.Name = course.Name;
                existingCourse.TotalStudents = course.TotalStudents;
                existingCourse.StartDate = course.StartDate;
                existingCourse.EndDate = course.EndDate;
                existingCourse.NumberOfSessions = course.NumberOfSessions;
            }
        }

        public void DeleteCourse(int courseId) // Xóa khóa học
        {
            var course = GetCourseById(courseId); // Tìm khóa học
            if (course != null) // Nếu tìm thấy
            {
                courses.Remove(course); // Xóa khỏi danh sách
            }
        }

        public List<Class> GetClasses() => classes; // Lấy danh sách lớp học

        public Class GetClassById(int classId) // Lấy lớp học theo ID
        {
            Console.WriteLine($"\n🔍 Đang tìm lớp với ClassId: {classId}");
            var result = classes.FirstOrDefault(c => c.Id == classId); // Tìm lớp học
            Console.WriteLine($"Kết quả: {(result != null ? $"Tìm thấy: {result.Name}" : "Không tìm thấy")}");
            return result; // Trả về kết quả
        }

        public void AddClass(Class newClass) // Thêm lớp học mới
        {
            newClass.Id = classes.Any() ? classes.Max(c => c.Id) + 1 : 1; // Tạo ID mới

            // Tìm ID sinh viên lớn nhất hiện có
            int maxStudentId = classes.SelectMany(c => c.Students).Any()
                ? classes.SelectMany(c => c.Students).Max(s => s.Id)
                : 0;

            newClass.StudentId = maxStudentId + 1; // Tạo ID sinh viên mới
            classes.Add(newClass); // Thêm vào danh sách
            SaveClasses(); // Lưu thay đổi
        }

        public void UpdateClass(Class cls) // Cập nhật lớp học
        {
            var existingClass = GetClassById(cls.ClassId); // Tìm lớp hiện có
            if (existingClass != null) // Nếu tìm thấy
            {
                // Cập nhật thông tin
                existingClass.Name = cls.Name;
                existingClass.CourseId = cls.CourseId;
                SaveClasses(); // Lưu thay đổi
            }
        }

        public void DeleteClass(int classId) // Xóa lớp học
        {
            var cls = GetClassById(classId); // Tìm lớp
            if (cls != null) // Nếu tìm thấy
            {
                classes.Remove(cls); // Xóa khỏi danh sách
                SaveClasses(); // Lưu thay đổi
            }
        }
    }

    public class RecordService // Service quản lý bản ghi điểm danh
    {
        private readonly IDataService _dataService; // Service dữ liệu
        private List<IAttendanceObserver> _observers = new List<IAttendanceObserver>(); // Danh sách observer

        public RecordService(IDataService dataService) // Constructor
        {
            _dataService = dataService; // Khởi tạo service
        }

        public void AddObserver(IAttendanceObserver observer) // Thêm observer
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IAttendanceObserver observer) // Xóa observer
        {
            _observers.Remove(observer);
        }

        public List<Record> GetStudentsByClass(int classId) // Lấy sinh viên theo lớp
        {
            var selectedClass = _dataService.GetClassById(classId); // Lấy lớp theo ID
            if (selectedClass != null) // Nếu tìm thấy
            {
                return selectedClass.Students
                    .Select(s => new Record { StudentId = s.UserId, StudentName = s.Name, IsPresent = false })
                    .ToList(); // Tạo danh sách bản ghi điểm danh
            }
            return new List<Record>(); // Trả về danh sách rỗng nếu không tìm thấy
        }

        public void SetAttendance(int studentId, bool isPresent) // Điểm danh sinh viên
        {
            var student = _dataService.GetClasses()
                .SelectMany(c => c.Students)
                .FirstOrDefault(s => s.UserId == studentId); // Tìm sinh viên

            if (student != null) // Nếu tìm thấy
            {
                var record = new Record { StudentId = student.UserId, StudentName = student.Name, IsPresent = isPresent };
                foreach (var observer in _observers) // Thông báo cho tất cả observer
                {
                    observer.UpdateAttendance(record);
                }
            }
        }

        public string GetStudentName(int studentId) // Lấy tên sinh viên từ file CSV
        {
            string filePath = "Resources/Student.CSV"; // Đường dẫn file
            if (!File.Exists(filePath)) return string.Empty; // Kiểm tra file tồn tại

            var lines = File.ReadAllLines(filePath); // Đọc tất cả dòng
            foreach (var line in lines.Skip(1)) // Bỏ qua dòng tiêu đề
            {
                var data = line.Split(','); // Tách dữ liệu
                if (data.Length >= 5 && int.TryParse(data[0], out int csvStudentId) && csvStudentId == studentId)
                {
                    return data[1]; // Trả về tên sinh viên
                }
            }
            return string.Empty; // Trả về rỗng nếu không tìm thấy
        }
    }
}