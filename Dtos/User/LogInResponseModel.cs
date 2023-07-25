namespace AgroExpressAPI.Dtos.User;
public class LogInResponseModel<T>
{
    public string? Token {get; set;}
    public string? RefreshToken { get; set; }
    public T Data {get; set;}
    public bool IsSuccess {get; set;}
}