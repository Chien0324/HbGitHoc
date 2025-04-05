using SIMS_App.Models;
using System.Collections.Generic;

namespace SIMS_App.Data
{
    public interface IDataService // Interface cho DataService
    {
        // Quản lý khóa học
        List<Course> GetCourses();
        Course GetCourseById(int courseId);
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(int courseId);

        // Quản lý lớp học
        List<Class> GetClasses();
        Class GetClassById(int classId);
        void AddClass(Class cls);
        void UpdateClass(Class cls);
        void DeleteClass(int classId);
        string GetStudentName(int studentId);

        // Quản lý sinh viên
        List<Student> GetStudentsByClassId(int classId);
    }
}