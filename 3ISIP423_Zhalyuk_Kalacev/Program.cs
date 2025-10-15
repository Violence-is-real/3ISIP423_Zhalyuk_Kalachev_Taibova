using System;
using System.Collections.Generic;
using System.Linq;
public abstract class Person
{
    public int Id { get; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }

    protected Person(int id, string name, int age, string email)
    {
        Id = id;
        Name = name;
        Age = age;
        Email = email;
    }

    public abstract string GetInfo();
}
public class Course
{
    public int Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    private Teacher _teacher;
    private List<Student> _students = new List<Student>();

    public Course(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
public class Student : Person
{
    private List<Course> _courses = new List<Course>();

    public Student(int id, string name, int age, string email)
        : base(id, name, age, email) { }

    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    public void EnrollInCourse(Course course)
    {
        if (!_courses.Contains(course))
        {
            _courses.Add(course);
            course.AddStudent(this);
        }
    }

    public override string GetInfo()
    {
        return $"Студент: {Name} (ID: {Id}), Возраст: {Age}, Email: {Email}";
    }
}

public class Teacher : Person
{
    private List<Course> _courses = new List<Course>();

    public Teacher(int id, string name, int age, string email)
        : base(id, name, age, email) { }

    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    public void AssignToCourse(Course course)
    {
        if (!_courses.Contains(course))
        {
            _courses.Add(course);
            course.AssignTeacher(this);
        }
    }

    public override string GetInfo()
    {
        return $"Преподаватель: {Name} (ID: {Id}), Возраст: {Age}, Email: {Email}";
    }
}
public class UniversityManager
{
    private List<Student> _students = new List<Student>();
    private List<Teacher> _teachers = new List<Teacher>();
    private List<Course> _courses = new List<Course>();
    private int _nextStudentId = 1;
    private int _nextTeacherId = 1;
    private int _nextCourseId = 1;

    public void AddStudent(string name, int age, string email)
    {
        _students.Add(new Student(_nextStudentId++, name, age, email));
    }

    public void AddTeacher(string name, int age, string email)
    {
        _teachers.Add(new Teacher(_nextTeacherId++, name, age, email));
    }

    public void CreateCourse(string name, string description)
    {
        _courses.Add(new Course(_nextCourseId++, name, description));
    }

    public Student GetStudentById(int id) => _students.FirstOrDefault(s => s.Id == id);
    public Teacher GetTeacherById(int id) => _teachers.FirstOrDefault(t => t.Id == id);
    public Course GetCourseById(int id) => _courses.FirstOrDefault(c => c.Id == id);

    public void EnrollStudentInCourse(int studentId, int courseId)
    {
        var student = GetStudentById(studentId);
        var course = GetCourseById(courseId);
        student?.EnrollInCourse(course);
    }

    public void AssignTeacherToCourse(int teacherId, int courseId)
    {
        var teacher = GetTeacherById(teacherId);
        var course = GetCourseById(courseId);
        teacher?.AssignToCourse(course);
    }

    public void DisplayAllStudents()
    {
        foreach (var student in _students)
        {
            Console.WriteLine(student.GetInfo());
        }
    }

    public void DisplayAllTeachers()
    {
        foreach (var teacher in _teachers)
        {
            Console.WriteLine(teacher.GetInfo());
        }
    }

    public void DisplayAllCourses()
    {
        foreach (var course in _courses)
        {
            Console.WriteLine(course.GetInfo() + "\n");
        }
    }

    public void DisplayStudentCourses(int studentId)
    {
        var student = GetStudentById(studentId);
        if (student != null)
        {
            Console.WriteLine($"Курсы студента {student.Name}:");
            foreach (var course in student.Courses)
            {
                Console.WriteLine($"- {course.Name}");
            }
        }
    }

    public void DisplayCourseStudents(int courseId)
    {
        var course = GetCourseById(courseId);
        if (course != null)
        {
            Console.WriteLine($"Студенты курса {course.Name}:");
            foreach (var student in course.Students)
            {
                Console.WriteLine($"- {student.Name}");
            }
        }
    }
}
