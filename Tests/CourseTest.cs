using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace University
{
  [Collection("UniversityExams")]
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_CourseGetAll_Return0()
    {
      int result = Course.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Save_CourseSavesToDatabase_CourseList()
    {
      //Arrange
      Course testCourse = new Course("History of Guy", "GUY101");
      testCourse.Save();

      //Act
      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Find_FindsCourseInDatabase_Course()
    {
      //Arrange
      Course testCourse = new Course("History of Guy", "GUY101");
      testCourse.Save();

      //Act
      Course result = Course.Find(testCourse.GetId());

      //Assert
      Assert.Equal(testCourse, result);
    }

    public void Dispose()
    {
      Course.DeleteAll();
    }

  }
}
