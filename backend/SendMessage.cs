using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace backend
{
  public static class SendMessage
  {
    [FunctionName("SendMessage")]
    [return: TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "+12816127031")]
    public static CreateMessageOptions Run([QueueTrigger("texts-to-send")]TextMessage textMessage,
    ILogger log)
    {
      var message = new CreateMessageOptions(new PhoneNumber(textMessage.To))
      {
        Body = textMessage.Message
      };

      return message;
    }
  }

  public class TextMessage
  {
    public string To { get; set; }
    public string Message { get; set; }
  }
}
