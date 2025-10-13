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