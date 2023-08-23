using AgroExpressAPI.ProgramHelper;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ProgrameHelperClass.CrossOriginPolicy(builder);//Cross origin Policy

ProgrameHelperClass.AdminPolicy(builder);//Adding a policy to an End-point or Controller

ProgrameHelperClass.AddingContextAccessor(builder);//Adding contect accessor to the container

ProgrameHelperClass.RegisteringAndSortingDependencies(builder);//Registering,sorting and determining the life cycle of dependencies
ProgrameHelperClass.AddingDbContextToContainer(builder);// Adding BdContext class to the container

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wazobia Agro Express",Version = "v1"});
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Bearer and then token is the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }

    }) ;
});


ProgrameHelperClass.AddingJWTConfigurationToContainer(builder);//Adding JWT Configuration to the container


var app = builder.Build();


// Configure the HTTP request pipeline.
ProgrameHelperClass.HttpPipelineConfiguration(app);


app.Run();


