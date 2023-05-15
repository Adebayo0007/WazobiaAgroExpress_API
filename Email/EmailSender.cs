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
            Configuration.Default.ApiKey.Clear();
            var apiKey = _configuration.GetValue<string>("SendinblueAPIkey:ApiKey");
            Configuration.Default.ApiKey.Add("api-key", apiKey);
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
                string messageTemplate = EmailTemplate(x,email.Message);
                string HtmlContent = messageTemplate;
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
    private string EmailTemplate(string from, string content)
    {
        var emailTemplate = $$""""
        <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <link rel="icon" type="images/x-icon" href="~/Data/agro logo.png" />
                <meta http-equiv="X-UA-Compatible" content="IE=edge">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Wazobia Agro Express</title>
            </head>
            <body>
                <div style="  display: flex;flex-direction: column;justify-content: center;width: 100vw;height: 100vh;gap:1vh;">
                    <div style=" height: 20vh;display: flex;justify-content: center;align-items: center;">
                        <div style=" width: 23vw;height: 15vh;border-radius: 100%;background-image: url('https://media.istockphoto.com/id/1445788384/photo/agricultural-landscape-of-golden-wheat-field.jpg?s=612x612&w=0&k=20&c=QheMJyOWpwRwvzM_Z_fkraekI1WA62xp-9S5BVE-J08=');background-repeat: no-repeat;background-size: cover;box-shadow: rgba(50, 50, 93, 0.25) 0px 6px 12px -2px, rgba(0, 0, 0, 0.3) 0px 3px 7px -3px;"></div>
                    </div>
                    <div style=" height: 77vh;overflow-y: scroll;scroll-behavior: smooth;padding: 1vh 2vw;">
                        <div style=" margin: 2vh 0%;"><span style=" font-size: larger;font-weight: bolder;font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;">{{from}}</span></div>
                        <div ><span style=" font-size: large;font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;text-align: center;">{{content}}</span></div>
                    </div>
                </div>
            </body>
            </html>
    """";
               return emailTemplate;

    }
}
