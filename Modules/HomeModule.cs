using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace University
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Student> allStudents = Student.GetAll();
        List<Course> allCourses = Course.GetAll();
        model.Add("students", allStudents);
        model.Add("courses", allCourses);
        return View["index.cshtml", model];
      };
      Get["students/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        Student selectedStudent = Student.Find(parameters.id);
        model.Add("selected", selectedStudent);
        model.Add("studentCourses", selectedStudent.GetCourses());
        return View["student.cshtml", model];
      };
    }
  }
}
