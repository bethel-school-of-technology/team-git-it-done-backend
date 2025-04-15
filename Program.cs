//using team.Migrations;

using fareShare.Migrations;
using fareShare.Models;
using fareShare.Repositories;
using fareShare.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

//using fareShare.Repositories;

//using fareShare.BillDatabaseSettings;

// using fareShare.AddControllers;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<ICoffeeRepository, CoffeeRepository>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "l11_tokens", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBillRepository, BillRepository>();


//MAIA~ implement DatatBase Here (https://bethel.populiweb.com/router/courseofferings/10739695/lessons/10916749/pages/12026035/show)

builder.Services.AddSqlite<BillDbContext>("Data Source=fareShare.db");

//builder.Services.AddScoped<IBillRepository, BillRepository>();

// builder.Services.Configure<BillDatabaseSettings>(
//     builder.Configuration.GetSection("BillDatabaseSettings")
// );

//builder.Services.AddScoped<IBillRepository, NoSqlBillRepository>();


var secretKey = builder.Configuration["TokenSecret"];

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = true;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters =
            new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(secretKey)
                ),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
            };
    });

    var app = builder.Build();

app.UseCors(builder =>
    builder
        .WithOrigins("http://localhost:8100", "http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
