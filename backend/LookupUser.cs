using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using backend.models;
using System.Collections.Generic;
using System.Linq;

namespace backend
{
  public static class LookupUser
  {
    public static User GetUserByCode(this List<User> list, int code)
    {
      return list.FirstOrDefault(i => i.code == code);
    }

    public static User GetUserByCode(this List<User> list, string code)
    {
      return list.FirstOrDefault(i => i.code == int.Parse(code));
    }
  }
}
