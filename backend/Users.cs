using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using backend.models;
using Microsoft.WindowsAzure.Storage.Table;

namespace backend
{
  public static class Users
  {

    [FunctionName("Users_GetAll")]
    public static async Task<IActionResult> GetAll(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users")] HttpRequest req,
        ILogger log,
        [Table("users")] CloudTable userTable)
    {
      var users = await userTable.GetAllUsers();
      return new OkObjectResult(users);
    }

    [FunctionName("Users_Get")]
    public static async Task<IActionResult> Get(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{code}")] HttpRequest req,
        string code,
        ILogger log,
        [Table("users")] CloudTable userTable)
    {
      var user = await userTable.GetUser(code);
      return new OkObjectResult(user);
    }

    [FunctionName("Users_Update")]
    public static async Task<IActionResult> Update(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/{code}")] UserDto userUpdate,
        ILogger log,
        [Table("users")] CloudTable userTable,
        [Queue("texts-to-send", Connection = "AzureWebJobsStorage")] ICollector<TextMessage> queueCollector)
    {
      var user = await userTable.UpdateUser(userUpdate.ConvertToTableEntity());
      if (userUpdate.address != null)
      {
        var secretSanta = await userTable.GetUser(user.receivingFromId);
        queueCollector.Add(new TextMessage { To = secretSanta.phone, Message = $"Send {user.name}'s gift to:\n\n {user.address}" });
      }
      return new OkObjectResult(user);
    }
  }
}
