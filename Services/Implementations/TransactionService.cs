using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.BankVerification;
using AgroExpressAPI.Dtos.ReciepientGeneration;
using AgroExpressAPI.Dtos.RequestedProduct;
using AgroExpressAPI.Dtos.Transaction;
using AgroExpressAPI.Dtos.Transfer;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using AgroExpressAPI.Services.Interfaces;


namespace AgroExpressAPI.Services.Implementations;
public class TransactionService : ITransactionService
{
    private readonly IRequestedProductService _requestedProductService;
     private readonly IProductRepository _productRepository;
     private readonly ITransactionRepository _transactionRepository;
     private readonly IHttpContextAccessor _httpContextAccessor;
    public TransactionService(IRequestedProductService requestedProductService, IProductRepository productRepository, ITransactionRepository transactionRepository,IHttpContextAccessor httpContextAccessor)
    {
       _requestedProductService = requestedProductService;
       _productRepository = productRepository;
       _transactionRepository = transactionRepository;
       _httpContextAccessor = httpContextAccessor;
    }
   

    
    public async Task<GenerateRecipient> GenerateRecipients(VerifyBank verifyBank)
    {

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var baseUri = httpClient.BaseAddress = new Uri($"https://api.paystack.co/transferrecipient");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_1dacd4891d686e4c9f616a96a1e868138cb9d067");
        var response = await httpClient.PostAsJsonAsync(baseUri, new
        {
            type = "nuban",
            name = verifyBank.data.account_name,
            account_number = verifyBank.data.account_number,
            bank_code = verifyBank.data.bank_code,
            currency = "NGN",
        });
        var responseAsString = await response.Content.ReadAsStringAsync();
        dynamic responseObject =  Newtonsoft.Json.JsonConvert.DeserializeObject(responseAsString);
        if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            if (!responseObject.status)
            {
                return new GenerateRecipient
                {
                    status = false,
                    message = responseObject.message
                };
            }
            return new GenerateRecipient
            {
                status = true,
                message = "Recipient Generated",
                data = responseObject.data
            };
        }

        return new GenerateRecipient()
        {
            status = false,
            message = responseObject.message
        };
    }



    //This behavour link "GenerateRecipients" and "SendModney"
    public async Task<VerifyBank> VerifyAccountNumber(string farmerEmail,string accountNumber, string bankCode, double amount)
    {
         var getHttpClient = new HttpClient();
        getHttpClient.DefaultRequestHeaders.Accept.Clear();
        getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var baseUri = getHttpClient.BaseAddress =
            new Uri($"https://api.paystack.co/bank/resolve?account_number={accountNumber}&bank_code={bankCode}");

        getHttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "sk_test_1dacd4891d686e4c9f616a96a1e868138cb9d067");
        var response = await getHttpClient.GetAsync(baseUri);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<VerifyBank>(responseString);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            if (!responseObject.status)
            {
                return new VerifyBank()
                {
                    status = false,
                    message = responseObject.message
                };
            }

            var generate = await GenerateRecipients(responseObject);
            if (!generate.status)
            {
                return new VerifyBank()
                {
                    status = false,
                    message = generate.message
                };
            }

            var makeTransfer = await SendMoney(generate.data.recipient_code, amount);
            if (!makeTransfer.status)
            {
                return new VerifyBank()
                {
                    status = false,
                    message = makeTransfer.message
                };
            }
             var transaction = new Transaction{
            Amount = Convert.ToDouble(makeTransfer.data.amount),
            DateCreated = makeTransfer.data.createdAt,
            ReferenceNumber = makeTransfer.data.reference,
            BuyerEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value,
            FarmerEmail = farmerEmail
           };
           await _transactionRepository.CreateAsync(transaction);
            return new VerifyBank()
            {
                status = true,
                message = makeTransfer.message,
                data = new VerifyBankData()
                {
                    reason = generate.data.reason,
                    reference = generate.data.reference,
                    recipient_code = generate.data.recipient_code,
                    amount = makeTransfer.data.amount,
                    currency = makeTransfer.data.currency,
                    status = makeTransfer.data.status,
                    transfer_code = makeTransfer.data.transfer_code
                }
            };
        }

        return new VerifyBank()
        {
            status = false,
            message = "Cannot verify account number"
        };

    }

         private async Task<MakeATransfer> SendMoney(string reciepient, double amount)
        {
            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = $"https://api.paystack.co/transfer";
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_1dacd4891d686e4c9f616a96a1e868138cb9d067");
            var response = await getHttpClient.PostAsJsonAsync(baseUri, new
            {
                
                recipient = reciepient,
                amount = amount * 100,
                currency = "NGN",
                source = "balance"
            });
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<MakeATransfer>(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!responseObject.status)
                {
                    return new MakeATransfer()
                    {
                        status = false,
                        message = responseObject.message
                    };
                }
                return new MakeATransfer()
                {
                    status = true,
                    message = responseObject.message,
                    data = responseObject.data
                };
            }
            return new MakeATransfer()
            {
                status = false,
                message = "Pls retry payment is not successfull"
            };
        }



  
    public async Task<BaseResponse<IEnumerable<TransactionDto>>> GetAllPaymentsMadeByUserUsingEmail(string email)
    {
         var transactions = await _transactionRepository.GetByEmailAsync(email);
         var transactionDto = transactions.Select(t => new TransactionDto{
                        Amount = t.Amount,
                        DateCreated = t.DateCreated,
                        ReferenceNumber = t.ReferenceNumber,
                        FarmerEmail = t.FarmerEmail,
                        BuyerEmail = t.BuyerEmail
         });
         return new BaseResponse<IEnumerable<TransactionDto>>
         {
            IsSuccess = true,
            Message = "All transaction retrieved successful",
            Data = transactionDto
         };
    }

    public async Task<BaseResponse<TransactionDto>> MakePayment(CreateRequestedProductRequestModel productRequestModel)
    {
        var product =  _productRepository.GetProductById(productRequestModel.ProductId);
        var amountToPay = productRequestModel.Quantity*productRequestModel.Price;
        
        var refNum  = GenerateReferrenceNumber();
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var url =  "https://api.paystack.co/transaction/initialize";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_1dacd4891d686e4c9f616a96a1e868138cb9d067");
        var content = new StringContent(JsonSerializer.Serialize(new {
            amount = amountToPay * 100,
            email = productRequestModel.BuyerEmail,
            referrenceNumber = refNum

        }), Encoding.UTF8, "application/json");
        var response  = await httpClient.PostAsync(url, content);
        var responseAsString = await response.Content.ReadAsStringAsync();
        dynamic responseObj =  Newtonsoft.Json.JsonConvert.DeserializeObject(responseAsString);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
           var createRequestedProduct = await _requestedProductService.CreateRequstedProductAsync(product.Id, productRequestModel);
           if(createRequestedProduct.IsSuccess == false)
           {
                return new BaseResponse<TransactionDto>
                {
                    IsSuccess = createRequestedProduct.IsSuccess,
                    Message = createRequestedProduct.Message
                };
           }   
           var transaction = new Transaction{
            Amount = amountToPay,
            DateCreated = DateTime.Now.ToString("dd/MM/yyyy"),
            ReferenceNumber = refNum,
            BuyerEmail = productRequestModel.BuyerEmail,
            FarmerEmail = product.FarmerEmail
           };
           await _transactionRepository.CreateAsync(transaction);
           return new BaseResponse<TransactionDto>
                {
                    IsSuccess = true,
                    Message = "You have successfully ordered your product,please await the farmer's response"
                };
        }

       
             return new BaseResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = "Internal error"
                };

    }


    public async Task<BaseResponse<TransactionDto>> VerifyPayment(string referrenceNumber)
    {
        var transaction = _transactionRepository.GetByReferenceNumberAsync(referrenceNumber);
        return new BaseResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = "Internal error",
                    Data = new TransactionDto{
                        Amount = transaction.Amount,
                        DateCreated = transaction.DateCreated,
                        ReferenceNumber = transaction.ReferenceNumber,
                        FarmerEmail = transaction.FarmerEmail,
                        BuyerEmail = transaction.BuyerEmail
                    }
                };
    }


     public async Task<BaseResponse<TransactionDto>> SendMoneyToTheFarmer(string reciepient,double amount)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var url = $"https://api.paystack.co/transfer";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_1dacd4891d686e4c9f616a96a1e868138cb9d067");
        var response = await httpClient.PostAsJsonAsync(url, new
        {

            recipient = reciepient,    //note
            amount = amount * 100,
            currency = "NGN",
            source = "balance"
        });
        var responseString = await response.Content.ReadAsStringAsync();
        dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseString);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            if (!responseObject.status)
            {
                return new BaseResponse<TransactionDto>
                {
                    IsSuccess = false,
                    Message = responseObject.message
                };
            }
            var trans = new TransactionDto{
                Amount = responseObject.data.amount,
                ReferenceNumber = responseObject.data.referrenceNumber
            };
      
            return new BaseResponse<TransactionDto>
            {
                IsSuccess = true,
                Message = responseObject.message,
                Data = responseObject.data
            };
        }
         return new BaseResponse<TransactionDto>
            {
                IsSuccess = false,
                Message = "Payment is not successfull"
                
            };
    }
    
    private string GenerateReferrenceNumber()
    {
                string alpha  ="abcdefghijklmnopqrstuvwxyz".ToUpper();
                var i = new Random().Next(25);
                var j = new Random().Next(25);
                var k = new Random().Next(25,99);
                return $"Ref{k}{i}{j}{alpha[i]}{alpha[j]}" ;
    }
}
