using System.Configuration;
using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Text.RegularExpressions;

namespace AgroExpressAPI.Email;
public class EmailSender : IEmailSender
{
       public readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    public async Task<bool> EmailValidaton(string email)
    {
                  if(!(string.IsNullOrWhiteSpace(email)))
               {
                   string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + 
                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (re.IsMatch(email))return (true);
                else
                    return (false);
                }
                return false;

    }

    public async Task<bool> SendEmail(EmailRequestModel email)
    {
           
            string x;
              sib_api_v3_sdk.Client.Configuration.Default.ApiKey.Clear();
            var apiKey = _configuration.GetValue<string>("SendinblueAPIkey:ApiKey");
           sib_api_v3_sdk.Client.Configuration.Default.ApiKey.Add("api-key", apiKey);
            if(email.SenderEmail == null) 
            {
               email.SenderEmail = "tijaniadebayoabdllahi@gmail.com";
               x = "Wazobia Agro Express";
            }
            else{
                x = email.SenderEmail;
            }

                var apiInstance = new TransactionalEmailsApi();
                string SenderName = "Wazobia Agro Express";
                string SenderEmail = email.SenderEmail;
                SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
                string ToEmail = email.ReceiverEmail;
                string ToName = email.ReceiverName;
                SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
                List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
                To.Add(smtpEmailTo);

                //Bcc the reciever also have the copy of the message but name do not visisble to othe reciepient of the email
                string BccName = email.ReceiverName;
                string BccEmail = email.ReceiverEmail;
                SendSmtpEmailBcc BccData = new SendSmtpEmailBcc(BccEmail, BccName);
                List<SendSmtpEmailBcc> Bcc = new List<SendSmtpEmailBcc>();
                Bcc.Add(BccData);

                //CC Sender also recieve the email
                string CcName = "Wazobia agro Wxpress";
                string CcEmail = email.SenderEmail;
                SendSmtpEmailCc CcData = new SendSmtpEmailCc(CcEmail, CcName);
                List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();
                Cc.Add(CcData);
                string HtmlContent = $"<html><body><h1>From {x}</h1><br><h3> {email.Message}</h3></body></html>";
                string TextContent = null;
                string Subject = "{{params.subject}}";
                string ReplyToName = "Wazobia Agro Express";
                string ReplyToEmail = email.SenderEmail;
                SendSmtpEmailReplyTo ReplyTo = new SendSmtpEmailReplyTo(ReplyToEmail, ReplyToName);
                string AttachmentUrl = null;
                 string stringInBase64 = "aGVsbG8gdGhpcyBpcyB0ZXN0";
                 byte[] Content = System.Convert.FromBase64String(stringInBase64);
                 string AttachmentName = "test.txt";
                SendSmtpEmailAttachment AttachmentContent = new SendSmtpEmailAttachment(AttachmentUrl, Content, AttachmentName);
                List<SendSmtpEmailAttachment> Attachment = new List<SendSmtpEmailAttachment>();
                Attachment.Add(AttachmentContent);
                JObject Headers = new JObject();
                Headers.Add("Some-Custom-Name", "unique-id-1234");
                long? TemplateId = null;
                JObject Params = new JObject();

                //this is subtituted by the params.parameter
                Params.Add("parameter", email.Message);

                //this is subtituted by the params.subbject
                Params.Add("subject", email.Subject);
                List<string> Tags = new List<string>();
                Tags.Add("mytag");
                SendSmtpEmailTo1 smtpEmailTo1 = new SendSmtpEmailTo1(ToEmail, ToName);
                List<SendSmtpEmailTo1> To1 = new List<SendSmtpEmailTo1>();
                To1.Add(smtpEmailTo1);
                Dictionary<string, object> _parmas = new Dictionary<string, object>();
                _parmas.Add("params", Params);
                SendSmtpEmailReplyTo1 ReplyTo1 = new SendSmtpEmailReplyTo1(ReplyToEmail, ReplyToName);
                SendSmtpEmailMessageVersions messageVersion = new SendSmtpEmailMessageVersions(To1, _parmas, Bcc, Cc, ReplyTo1, Subject);
                List<SendSmtpEmailMessageVersions> messageVersiopns = new List<SendSmtpEmailMessageVersions>();
                messageVersiopns.Add(messageVersion);
                try{

                 var sendSmtpEmail = new SendSmtpEmail(Email, To, Bcc, Cc, HtmlContent, TextContent, Subject, ReplyTo, Attachment, Headers, TemplateId, Params, messageVersiopns, Tags);
                 CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                 return true;  
                }
                catch(Exception ex)
                {
                    return false;
                }
    }
}
