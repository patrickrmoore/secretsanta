using System;

namespace backend.models
{
  public class User
  {
    public string PartitionKey { get; set; } = "Test1";
    public string RowKey
    {
      get { return this.code.ToString(); }
    }
    public string id { get; set; }
    public int code { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string address { get; set; }
    public string givingToId { get; set; }
    public string receivingFromId { get; set; }
  }
}