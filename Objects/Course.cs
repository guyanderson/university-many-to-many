using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace University
{
  public class Course
  {
    private int _id;
    private string _name;
    private string _courseName;

    public Course(string Name, string CourseName, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _courseName = CourseName;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool nameEquality = this.GetName() == newCourse.GetName();
        bool courseNameEquality = this.GetCourseName() == newCourse.GetCourseName();

        return (idEquality && nameEquality && courseNameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
     _name = newName;
    }

    public string GetCourseName()
    {
      return _courseName;
    }
    public void SetCourseName(string newCourseName)
    {
      _courseName = newCourseName;
    }

    public static List<Course> GetAll()
    {
      List<Course> AllCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseCourseName = rdr.GetString(2);
        Course newCourse = new Course(courseName, courseCourseName, courseId);
        AllCourses.Add(newCourse);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllCourses;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, courseName) OUTPUT INSERTED.id VALUES (@CourseName, @CourseCourseName)", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@CourseName";
      nameParam.Value = this.GetName();

      SqlParameter courseNameParam = new SqlParameter();
      courseNameParam.ParameterName = "@CourseCourseName";
      courseNameParam.Value = this.GetCourseName();

      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(courseNameParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId", conn);
      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = id.ToString();
      cmd.Parameters.Add(courseIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;
      string foundCourseCourseName = null;

      while(rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(1);
        foundCourseCourseName = rdr.GetString(2);
      }
      Course foundCourse = new Course(foundCourseName, foundCourseCourseName, foundCourseId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCourse;
    }


    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
