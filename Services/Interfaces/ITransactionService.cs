using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.BankVerification;
using AgroExpressAPI.Dtos.ReciepientGeneration;
using AgroExpressAPI.Dtos.RequestedProduct;
using AgroExpressAPI.Dtos.Transaction;

namespace AgroExpressAPI.Services.Interfaces;
    public interface ITransactionService
    {
          Task<BaseResponse<TransactionDto>> MakePayment(CreateRequestedProductRequestModel productRequestModel);
           Task<BaseResponse<TransactionDto>> SendMoneyToTheFarmer(string reciepient,double amount);
           Task<GenerateRecipient> GenerateRecipients(VerifyBank verifyBank);
           Task<VerifyBank> VerifyAccountNumber(string farmerEmail, string accountNumber, string bankCode, double amount);

           Task<BaseResponse<TransactionDto>> VerifyPayment(string referrenceNumber);

             Task<BaseResponse<IEnumerable<TransactionDto>>> GetAllPaymentsMadeByUserUsingEmail(string email);
    }
