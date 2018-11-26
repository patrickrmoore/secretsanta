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
using Microsoft.WindowsAzure.Storage.Table;

namespace backend
{
  public static class Extensions
  {
    public async static Task InsertUsers(this CloudTable table, List<User> users)
    {
      await table.CreateIfNotExistsAsync();
      var insertOperation = new TableBatchOperation();
      users.ForEach(user =>
      {
        insertOperation.Insert(user);
      });
      await table.ExecuteBatchAsync(insertOperation);
    }

    public async static Task<User> UpdateUser(this CloudTable table, User user)
    {
      var operation = new TableBatchOperation();
      operation.InsertOrMerge(user);
      await table.ExecuteBatchAsync(operation);
      return user;
    }

    public async static Task<List<User>> GetAllUsers(this CloudTable table)
    {
      var partitionKey = System.Environment.GetEnvironmentVariable("PartitionKey");
      var query = new TableQuery<User>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
      var users = await table.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());
      return users.ToList();
    }

    public async static Task<User> GetUser(this CloudTable table, string code)
    {
      var partitionKey = System.Environment.GetEnvironmentVariable("PartitionKey");
      var query = TableOperation.Retrieve<User>(partitionKey, code);
      var retrieve = await table.ExecuteAsync(query);
      return (User)retrieve.Result;
    }

    public static User ConvertToTableEntity(this UserDto userDto)
    {
      User user = new User();
      user.address = userDto.address;
      user.name = userDto.name;
      user.code = userDto.code;
      user.email = userDto.email;
      user.phone = userDto.phone;
      user.givingToId = userDto.givingToId;
      user.receivingFromId = userDto.receivingFromId;
      return user;
    }
  }
}
