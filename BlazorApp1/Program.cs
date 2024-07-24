using BlazorApp1;
using BlazorApp1.Code;
using BlazorApp1.HttpClients;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
using razor.Components;
using Sassa.BRM.Models;
using Sassa.BRM.Services;
using Sassa.Socpen.Data;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

string BrmConnectionString = builder.Configuration.GetConnectionString("BrmConnection");

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddAuthorizationCore(options => { options.FallbackPolicy = options.DefaultPolicy; });

//builder.Services.AddHttpContextAccessor();
//builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);



builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<StaticD>();
//Factory pattern
builder.Services.AddDbContextFactory<ModelContext>(options =>
options.UseOracle(BrmConnectionString));
//Services 
builder.Services.AddScoped<SessionService>().AddDbContext<ModelContext>(options =>
options.UseOracle(BrmConnectionString));
builder.Services.AddScoped<BRMDbService>().AddDbContext<ModelContext>(options =>
options.UseOracle(BrmConnectionString));
builder.Services.AddScoped<MisFileService>().AddDbContext<ModelContext>(options =>
options.UseOracle(BrmConnectionString));
builder.Services.AddScoped<DestructionService>().AddDbContext<ModelContext>(options =>
options.UseOracle(BrmConnectionString));
builder.Services.AddScoped<SocpenService>().AddDbContext<SocpenContext>(options =>
options.UseOracle(BrmConnectionString));
builder.Services.AddScoped<StaticService>().AddDbContext<ModelContext>(options =>
options.UseOracle(BrmConnectionString));
builder.Services.AddScoped<TdwBatchService>().AddDbContext<ModelContext>(options =>
options.UseOracle(BrmConnectionString));
builder.Services.AddSingleton<BarCodeService>();
builder.Services.AddSingleton<RawSqlService>();
builder.Services.AddSingleton<MailMessages>();
builder.Services.AddSingleton<FileService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<Navigation>();
builder.Services.AddScoped<CSService>();
builder.Services.AddScoped<ReportDataService>();
builder.Services.AddScoped<ProgressService>();

builder.Services.AddScoped<ActiveUser>();
builder.Services.AddSingleton<ActiveUserList>();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
//    options.SlidingExpiration = true;
//});

//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();

//builder.Services.AddSignalR(hubOptions =>
//{
//    hubOptions.EnableDetailedErrors = true;
//    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(2);
//    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(
//        name: "AllowOrigin",
//        builder => {
//            builder.AllowAnyOrigin()
//                    .AllowAnyMethod()
//                    .AllowAnyHeader();
//        });
//});

//builder.Services.AddControllers();
//// Register the Swagger generator, defining 1 or more Swagger documents
//builder.Services.AddSwaggerGen();

// Add services to the container.
//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddTransient<LoginService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddHttpClient<IBackendApiHttpClient, BackendApiHttpClient>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:BackendApi"));
    options.Timeout = TimeSpan.FromSeconds(30);
    options.DefaultRequestHeaders.TryAddWithoutValidation("Service", Assembly.GetAssembly(typeof(Program))?.GetName().Name);
});

await builder.Build().RunAsync();
