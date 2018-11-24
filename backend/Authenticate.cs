using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using backend.models;
using System.Linq;

namespace backend
{
  public static class Authenticate
  {
    [FunctionName("Authenticate")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "authenticate")] HttpRequest req,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      string code = req.Query["code"];
      var users = RetrieveUsers();
      User currentUser = users.GetUserByCode(code);
      if (currentUser != null)
      {
        return new OkObjectResult(currentUser);
      }
      return new BadRequestResult();
    }

    public static List<User> Users { get; set; }

    public static List<User> RetrieveUsers()
    {
      return Users;
    }
  }
}
