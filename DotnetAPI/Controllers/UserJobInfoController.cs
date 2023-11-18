using System.ComponentModel.Design;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserJobInfoController : ControllerBase
{
    DataContextDapper _dapper;
    public UserJobInfoController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetAllUserJobInfo")]

    public IEnumerable<UserJobInfo> GetAllUserJobInfo()
    {
        string sql = @"
        SELECT  [UserId]
            , [JobTitle]
            , [Department]
        FROM  TutorialAppSchema.UserJobInfo;";
        IEnumerable<UserJobInfo> allUserJobInfo = _dapper.LoadData<UserJobInfo>(sql);
        return allUserJobInfo;
    }

    [HttpGet("GetSingleUserJobInfo/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        string sql = @"
        SELECT  [UserId]
            , [JobTitle]
            , [Department]
        FROM  TutorialAppSchema.UserJobInfo
        WHERE UserId = " + userId.ToString();
        UserJobInfo singleUserJobInfo = _dapper.LoadDataSingle<UserJobInfo>(sql);
        return singleUserJobInfo;
    }

    [HttpPut("EditUserJobInfo")]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
      string sql = @"
      UPDATE TutorialAppSchema.UserJobInfo
              SET [JobTitle] = '" + userJobInfo.JobTitle + @"', 
              [Department] = '" + userJobInfo.Department + @"'
          WHERE UserId = " + userJobInfo.UserId;

          Console.WriteLine(sql);

      if (_dapper.ExecuteSQL(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to Update UserJobInfo");
    }


    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
      string sql = @"
      INSERT INTO TutorialAppSchema.UserJobInfo(
          [UserId],
          [JobTitle],
          [Department]
            ) VALUES ('" + userJobInfo.UserId + @"', 
              '" + userJobInfo.JobTitle + @"',
              '" + userJobInfo.Department + "')";

      Console.WriteLine(sql);

      if (_dapper.ExecuteSQL(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to Add UserJobInfo");
    }

    [HttpDelete("DeleteUserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.UserJobInfo 
              WHERE UserId = " + userId.ToString();
      
      Console.WriteLine(sql);

      if (_dapper.ExecuteSQL(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to Delete UserJobInfo");
    }
}
