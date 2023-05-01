namespace AgroExpressAPI.Dtos.BankVerification
{
    public class VerifyBank
    {
        public bool status { get; set; }
        public string message { get; set; }
        public VerifyBankData data { get; set; }

    }
    public class VerifyBankData
    {
         public string account_number { get; set; }
        public string account_name { get; set; }
        public string bank_code { get; set; }
        public int bank_id { get; set; }
        public string recipient_code { get; set; }
        
        
        public string reference { get; set; }
        public int integration { get; set; }
        public string domain { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string source{ get; set; }
        public string reason { get; set; }
        public int recipient { get; set; }
        public string status { get; set; }
        public string transfer_code { get; set; }
        public int id { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }

    }
}