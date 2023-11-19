using System.ComponentModel.Design;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserSalaryController : ControllerBase
{
    DataContextDapper _dapper;
    public UserSalaryController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetAllUserSalary")]

    public IEnumerable<UserSalary> GetAllUserSalary()
    {
        string sql = @"
        SELECT  [UserId]
            , [Salary]
        FROM  TutorialAppSchema.UserSalary;";
        IEnumerable<UserSalary> allUserSalary = _dapper.LoadData<UserSalary>(sql);
        return allUserSalary;
    }

    [HttpGet("GetSingleUserSalary/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
        string sql = @"
        SELECT  [UserId]
            , [Salary]
        FROM  TutorialAppSchema.UserSalary
        WHERE UserId = " + userId.ToString();
        UserSalary singleUserSalary = _dapper.LoadDataSingle<UserSalary>(sql);
        return singleUserSalary;
    }

    [HttpPut("EditUserSalary")]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
      string sql = @"
      UPDATE TutorialAppSchema.UserSalary
              SET [Salary] = " + userSalary.Salary +
          "WHERE UserId = " + userSalary.UserId;

          Console.WriteLine(sql);

      if (_dapper.ExecuteSQL(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to Update UserSalary");
    }


    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
      string sql = @"
      INSERT INTO TutorialAppSchema.UserSalary(
          [UserId],
          [Salary]
            ) VALUES ('" + userSalary.UserId + "', '"
            + userSalary.Salary + "'";

      Console.WriteLine(sql);

      if (_dapper.ExecuteSQL(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to Add UserSalary");
    }

    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.UserSalary 
              WHERE UserId = " + userId.ToString();
      
      Console.WriteLine(sql);

      if (_dapper.ExecuteSQL(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to Delete UserSalary");
    }
}
