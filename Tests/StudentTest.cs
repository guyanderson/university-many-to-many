using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace University
{
  [Collection("UniversityExams")]
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_StudentGetAll_Return0()
    {
      int result = Student.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Save_StudentSavesToDatabase_StudentList()
    {
      //Arrange
      Student testStudent = new Student("Guy", new DateTime (2017, 1, 1));
      testStudent.Save();

      //Act
      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Find_FindsStudentInDatabase_Student()
    {
      //Arrange
      Student testStudent = new Student("Molly", new DateTime (2017, 1, 1));
      testStudent.Save();

      //Act
      Student result = Student.Find(testStudent.GetId());

      //Assert
      Assert.Equal(testStudent, result);
    }

    public void Dispose()
    {
      Student.DeleteAll();
    }

  }
}
