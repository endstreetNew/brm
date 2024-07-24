using Microsoft.EntityFrameworkCore;
using Sassa.BRM.Models;
using Sassa.BRM.Services;


var cbuilder = new ConfigurationBuilder();
cbuilder.AddJsonFile("appsettings.json");
var configuration = cbuilder.Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<BRMDbService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(configuration.GetConnectionString("BrmConnection")!));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();