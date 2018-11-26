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
using Microsoft.WindowsAzure.Storage.Table;

namespace backend
{
  public static class Authenticate
  {
    [FunctionName("Authenticate")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "authenticate")] HttpRequest req,
        ILogger log,
        [Table("users")] CloudTable userTable)
    {
      string accessCode = req.Query["accessCode"];
      var user = await userTable.GetUser(accessCode);

      if (user != null)
      {
        return new OkObjectResult(user);
      }
      return new BadRequestResult();
    }
  }
}
