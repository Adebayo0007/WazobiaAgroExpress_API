
using System.Configuration;
using System.Security.Claims;
using System.Text;
using AgroExpressAPI.ApplicationAuthentication;
using AgroExpressAPI.ApplicationContext;
using AgroExpressAPI.Controllers;
using AgroExpressAPI.Email;
using AgroExpressAPI.Repositories.Implementations;
using AgroExpressAPI.Repositories.Interfaces;
using AgroExpressAPI.Services.Implementations;
using AgroExpressAPI.Services.Interfaces;
using AgroExpressAPI.VersionConstrain;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace AgroExpressAPI.ProgramHelper;
    public class ProgrameHelperClass
    {
        public static void CrossOriginPolicy(WebApplicationBuilder builder)
        {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

            builder.Services.AddCors(a => a.AddPolicy("CorsPolicy", b => 
                {
                    //b.WithOrigins("http://localhost:5000/")
                    b.AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyHeader();
                    
                }));
        }
        public static void AdminPolicy(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization( x =>
            x.AddPolicy("AdminPolicy", policy =>{
                policy.RequireRole("Admin");
                policy.RequireClaim(ClaimTypes.Email, new string[] {"tijaniadebayoabdllahi@gmail.com","johnwilson5864@gmail.com"});
    
                }));
        }
        public static void AddingContextAccessor(WebApplicationBuilder builder)
        {
             builder.Services.AddHttpContextAccessor();
              builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        public static void RegisteringAndSortingDependencies(WebApplicationBuilder builder)
        {
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
            builder.Services.AddTransient<IJWTAuthentication, JWTAuthentication>();
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version"); // Specifying the header name that holds the version information
            });

             builder.Services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                new CacheProfile{
                    Duration = 3600
                });
            });//using cache for fast performace that will be implemented globally

          builder.Services.AddMemoryCache();  //using memory cache for storing data to avoiding hitting the db always
            

           
          //builder.Services.AddFastEndpoints();

        //builder.Services.AddSingleton<Configuration>();

    }

    public static void AddingDbContextToContainer(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseMySQL(
            builder.Configuration.GetConnectionString("AgroExpressConnectionString")
            ));

        }
        public static void AddingJWTConfigurationToContainer(WebApplicationBuilder builder)
        {

            var key = builder.Configuration.GetValue<string>("JWTConnectionKey:JWTKey");
            builder.Services.AddSingleton<IJWTAuthentication>(new JWTAuthentication());

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                });

        }

        public static void HttpPipelineConfiguration(WebApplication app)
        {
            app.Logger.LogInformation("Starting Application...");
            if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wazobia Agro Express v1"));
                }
               
                app.Logger.LogInformation("Adding Route...");
                app.UseRouting();
                app.UseHttpsRedirection();
                app.UseCors("CorsPolicy");
                app.UseStaticFiles();
                app.UseAuthentication();
                app.UseAuthorization();
               // app.UseFastEndpoints();
                app.MapControllers();
                //cancellation token for long proccesses
                app.MapGet("/hello", async (CancellationToken token) =>{
                    app.Logger.LogInformation("Request started at: " +
                    DateTime.Now.ToLongTimeString());
                    await Task.Delay(TimeSpan.FromSeconds(5),token);
                    app.Logger.LogInformation("Request completed at: "+
                    DateTime.Now.ToLongTimeString());
                    return "Success";
                    
                });

        }
    }
