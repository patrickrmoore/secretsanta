using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace backend.models
{
  public class UserDto
  {
    public int code { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string address { get; set; }
    public string givingToId { get; set; }
    public string receivingFromId { get; set; }
  }
}