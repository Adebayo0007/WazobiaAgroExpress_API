namespace AgroExpressAPI.Email;
    public interface IEmailSender
    {
         Task<bool> SendEmail(EmailRequestModel email);
          Task<bool> EmailValidaton (string email);
    }
