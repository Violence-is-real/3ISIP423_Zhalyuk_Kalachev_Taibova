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