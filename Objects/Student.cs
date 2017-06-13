using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace University
{
  public class Student
  {
    private int _id;
    private string _name;
    private DateTime _enrollDate;

    public Student(string Name, DateTime EnrollDate, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _enrollDate = EnrollDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool nameEquality = this.GetName() == newStudent.GetName();
        bool enrollDateEquality = this.GetEnrollDate() == newStudent.GetEnrollDate();

        return (idEquality && nameEquality && enrollDateEquality);
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

    public DateTime GetEnrollDate()
    {
      return _enrollDate;
    }
    public void SetEnrollDate(DateTime newEnrollDate)
    {
      _enrollDate = newEnrollDate;
    }

    public static List<Student> GetAll()
    {
      List<Student> AllStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime studentEnrollDate = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, studentEnrollDate, studentId);
        AllStudents.Add(newStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllStudents;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (name, enrollDate) OUTPUT INSERTED.id VALUES (@StudentName, @StudentEnrollDate)", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@StudentName";
      nameParam.Value = this.GetName();

      SqlParameter enrollDateParam = new SqlParameter();
      enrollDateParam.ParameterName = "@StudentEnrollDate";
      enrollDateParam.Value = this.GetEnrollDate();

      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(enrollDateParam);

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

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(studentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      DateTime foundStudentEnrollDate = default(DateTime);

      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
        foundStudentEnrollDate = rdr.GetDateTime(2);
      }
      Student foundStudent = new Student(foundStudentName, foundStudentEnrollDate, foundStudentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }


    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
