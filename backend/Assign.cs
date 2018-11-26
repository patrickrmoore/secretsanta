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
  public static class Assign
  {
    [FunctionName("Assign")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "assign")] HttpRequest req,
        ILogger log,
        [Table("users")] CloudTable userTable,
        [Queue("texts-to-send", Connection = "AzureWebJobsStorage")] ICollector<TextMessage> queueCollector)
    {
      var users = AssignUsers();
      await userTable.InsertUsers(users);

      users.ForEach(user =>
      {
        queueCollector.Add(new TextMessage { To = user.phone, Message = $"Hi {user.name}, visit https://secretsantafe.z21.web.core.windows.net/ to see who you are the Secret Santa for.\n\nYour access code is {user.code}" });
      });

      return new OkObjectResult(users);
    }

    public static List<User> AssignUsers()
    {
      var participants = CreateList();
      var accessCodes = GenerateCodes(participants.Count);

      for (int i = 0; i < participants.Count; i++)
      {
        participants[i].code = accessCodes[i];
      }

      participants = participants.OrderBy(p => p.code).ToList();

      for (int i = 0; i < participants.Count; i++)
      {
        if (i == 0)
        {
          participants[i].givingToId = participants[i + 1].code.ToString();
          participants[i].receivingFromId = participants[participants.Count - 1].code.ToString();

        }
        else if (i != participants.Count - 1)
        {
          participants[i].givingToId = participants[i + 1].code.ToString();
          participants[i].receivingFromId = participants[i - 1].code.ToString();
        }
        else
        {
          participants[i].givingToId = participants[0].code.ToString();
          participants[i].receivingFromId = participants[i - 1].code.ToString();
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
                new User {name="Clay",    phone="+12544241976"},
                new User {name="Trey",    phone="+12547235364"},
                new User {name="Rustin",  phone="+12548555282"},
                new User {name="Adam",    phone="+18172476941"},
                new User {name="Todd",    phone="+12544981119"},
                new User {name="Scott",   phone="+12546525394"},
                new User {name="Anthony", phone="+12543661114"},
                new User {name="Hunter",  phone="+12543157266"},
                new User {name="Michael", phone="+12546447796"},
                new User {name="Blake",   phone="+12547227040"},
                new User {name="Zach",    phone="+12544247505"},
                new User {name="Patrick", phone="+12547444274"}
            };
    }
  }
}
