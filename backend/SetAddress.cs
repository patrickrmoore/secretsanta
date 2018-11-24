using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace backend
{
  public static class SetAddress
  {
    [FunctionName("SetAddress")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      int code = int.Parse(req.Query["code"]);

      var users = Authenticate.RetrieveUsers();
      var user = users.GetUserByCode(code);

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      Address address = JsonConvert.DeserializeObject<Address>(requestBody);

      user.address = address.address + ", " + address.address2 + ", " + address.city + ", " + address.state + " " + address.zip;

      return new OkObjectResult(user);
    }
  }

  public class Address
  {
    public string address { get; set; }
    public string address2 { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string zip { get; set; }
  }
}
