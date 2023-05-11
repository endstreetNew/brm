using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Sassa.BRM.Services;
using Sassa.BRMDcSocpenService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<RawSqlService>();
//builder.Services.AddHostedService<TimedService>();
//builder.Services.AddSingleton<IHostedService, TimedService>();
builder.Services.AddSingleton<TimedService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<TimedService>());
builder.Services.AddSingleton<JsonFileUtils>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
