using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace backend.models
{
  public class User : TableEntity
  {
    public User()
    {
      PartitionKey = System.Environment.GetEnvironmentVariable("PartitionKey");
    }
    private int _code;
    public int code
    {
      get
      {
        return _code;
      }
      set
      {
        _code = value;
        RowKey = value.ToString();
      }
    }
    public string name { get; set; }
    public string email { get; set; }
    public string address { get; set; }
    public string givingToId { get; set; }
    public string receivingFromId { get; set; }
    public string phone { get; set; }
  }
}