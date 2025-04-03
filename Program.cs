//using team.Migrations;
using fareShare.Migrations;
using fareShare.Models;
using fareShare.Repositories;

//using fareShare.Repositories;

//using fareShare.BillDatabaseSettings;

//using MyApi.AddControllers;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<ICoffeeRepository, CoffeeRepository>();
builder.Services.AddControllers();
//builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();

//MAIA~ implement DatatBase Here (https://bethel.populiweb.com/router/courseofferings/10739695/lessons/10916749/pages/12026035/show)

builder.Services.AddSqlite<BillDbContext>("Data Source=fareShare.db");

//builder.Services.AddScoped<IBillRepository, BillRepository>();

// builder.Services.Configure<BillDatabaseSettings>(
//     builder.Configuration.GetSection("BillDatabaseSettings")
// );

//builder.Services.AddScoped<IBillRepository, NoSqlBillRepository>();

var app = builder.Build();

app.UseCors(builder =>
    builder
        .WithOrigins("http://localhost:8100", "http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
);

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
