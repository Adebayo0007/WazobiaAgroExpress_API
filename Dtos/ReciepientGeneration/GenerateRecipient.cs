using AgroExpressAPI.Dtos.BankVerification;

namespace AgroExpressAPI.Dtos.ReciepientGeneration;
   public class GenerateRecipient
    {
        public bool status { get; set; }
        public string message { get; set; }
        
        public GenerateRecipientData data { get; set; }
    }
    public class GenerateRecipientData
    {
        public bool active { get; set; }
           public DateTime createdAt { get; set; }
           public string currency { get; set; }
           public string domain { get; set; }
           public int id { get; set; }
           public int integration { get; set; }
           public string name { get; set; }
           public string reference { get; set; }
           public string reason { get; set; }
           public string recipient_code { get; set; }
           public string type { get; set; }
           public VerifyBankData details { get; set; }
    }
