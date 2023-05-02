using System.Text;
using AgroExpressAPI.ApplicationAuthentication;
using AgroExpressAPI.ApplicationContext;
using AgroExpressAPI.Email;
using AgroExpressAPI.Repositories.Implementations;
using AgroExpressAPI.Repositories.Interfaces;
using AgroExpressAPI.Services.Implementations;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(a => a.AddPolicy("CorsPolicy", b => 
 {
     b.WithOrigins("http://localhost:5000")
     .AllowAnyMethod()
     .AllowAnyHeader();
     
 }));


 builder.Services.AddHttpContextAccessor();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    builder.Services.AddScoped<IUserRepository , UserRepository>();
    builder.Services.AddScoped<IUserService , UserService>();

  builder.Services.AddScoped<IAdminRepository , AdminRepository>();
    builder.Services.AddScoped<IAdminService , AdminService>();

   builder.Services.AddScoped<IFarmerRepository , FarmerRepository>();
    builder.Services.AddScoped<IFarmerService , FarmerService>();

    builder.Services.AddScoped<IBuyerRepository , BuyerRepository>();
    builder.Services.AddScoped<IBuyerService , BuyerService>();

     builder.Services.AddScoped<IProductRepository , ProductRepository>();
    builder.Services.AddScoped<IProductService , ProductService>();

     builder.Services.AddScoped<IRequestedProductRepository , RequestedProductRepository>();
    builder.Services.AddScoped<IRequestedProductService , RequestedProductService>();

     builder.Services.AddScoped<ITransactionRepository , TransactionRepository>();
    builder.Services.AddScoped<ITransactionService , TransactionService>();

      builder.Services.AddScoped<IEmailSender , EmailSender>();
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseMySQL(
  builder.Configuration.GetConnectionString("AgroExpressConnectionString")
  ));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Wazobia Agro Express",Version = "v1"});
});

var key = "Wazobia Authorization key";
builder.Services.AddSingleton<IJWTAuthentication>(new JWTAuthentication(key));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wazobia Agro Express v1"));
}
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/hello", async (CancellationToken token) =>{
    app.Logger.LogInformation("Request started at: " +
    DateTime.Now.ToLongTimeString());
    await Task.Delay(TimeSpan.FromSeconds(5),token);
    app.Logger.LogInformation("Request completed at: "+
    DateTime.Now.ToLongTimeString());
    return "Success";
    
});

app.Run();
