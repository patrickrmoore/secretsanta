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
  public static class Assign
  {
    [FunctionName("Assign")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "assign")] HttpRequest req,
        ILogger log,
        [Table("participants", Connection = "AzureWebJobsStorage")] IAsyncCollector<User> userTable)
    {
      var participants = AssignUsers();
      Authenticate.Users = participants;
      // participants.ForEach(async participant =>
      // {
      //   await userTable.AddAsync(participant);
      // });
      return new OkObjectResult(participants);
    }

    public static List<User> AssignUsers()
    {
      var participants = CreateList();
      var accessCodes = GenerateCodes(participants.Count);

      for (int i = 0; i < participants.Count; i++)
      {
        participants[i].code = accessCodes[i];
      }

      participants = participants.OrderBy(p => p.id).ToList();

      for (int i = 0; i < participants.Count; i++)
      {
        if (i == 0)
        {
          participants[i].givingToId = participants[i + 1].name;
          participants[i].receivingFromId = participants[participants.Count - 1].name;

        }
        else if (i != participants.Count - 1)
        {
          participants[i].givingToId = participants[i + 1].name;
          participants[i].receivingFromId = participants[i - 1].name;
        }
        else
        {
          participants[i].givingToId = participants[0].name;
          participants[i].receivingFromId = participants[i - 1].name;
        }

      }

      return participants;
    }

    public static Random Random { get; set; } = new Random();

    public static List<int> GenerateCodes(int length)
    {
      HashSet<int> accessCodes = new HashSet<int>();

      while (accessCodes.Count < length)
      {
        accessCodes.Add(Random.Next(1000, 9999));
      }

      return accessCodes.ToList();
    }

    public static List<User> CreateList()
    {
      return new List<User>
            {
                new User {name="Patrick", id = Guid.NewGuid().ToString()},
                new User {name="Elise", id = Guid.NewGuid().ToString()},
                new User {name="Andrew", id = Guid.NewGuid().ToString()},
                new User {name="Travis", id = Guid.NewGuid().ToString()},
                new User {name="Ashley", id = Guid.NewGuid().ToString()}
            };
    }
  }
}
