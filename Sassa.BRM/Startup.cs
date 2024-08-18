using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
using razor.Components;
using Sassa.Brm.Common.Helpers;
using Sassa.Brm.Common.Models;
using Sassa.Brm.Common.Services;
using Sassa.BRM.Models;
using Sassa.BRM.Services;
using Sassa.Socpen.Data;
using System;

namespace Sassa.BRM
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string BrmConnectionString = Configuration.GetConnectionString("BrmConnection");
            var ActivityApi = new Uri(Configuration["Urls:ActivityApi"]);

            services.AddHttpContextAccessor();
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            //Factory pattern
            services.AddDbContextFactory<ModelContext>(options =>
            options.UseOracle(BrmConnectionString));
            //Services 
            services.AddScoped<BRMDbService>();//.AddDbContext<ModelContext>(options =>
            //options.UseOracle(BrmConnectionString));
            //services.AddSingleton<StaticDataService>();
            services.AddScoped<SessionService>();
            services.AddSingleton<StaticService>();
            services.AddSingleton<ActivityService>();
            services.AddScoped<MisFileService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(BrmConnectionString));
            services.AddScoped<DestructionService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(BrmConnectionString));
            services.AddScoped<SocpenService>().AddDbContext<SocpenContext>(options =>
            options.UseOracle(BrmConnectionString));
            services.AddScoped<TdwBatchService>();
            services.AddSingleton<BarCodeService>();
            services.AddSingleton<RawSqlService>();
            services.AddSingleton<FileService>();

            services.AddSingleton<IEmailSettings, EmailSettings>(c =>
            {
                EmailSettings emailSettings = new EmailSettings();
                emailSettings.ContentRootPath = _env.ContentRootPath;
                emailSettings.WebRootPath = _env.WebRootPath;
                emailSettings.ReportFolder = Configuration.GetValue<string>("Folders:Reports")!;
                emailSettings.DocumentFolder = Configuration.GetValue<string>("Folders:Documents")!;
                emailSettings.SmtpServer = Configuration.GetValue<string>("Email:SMTPServer")!;
                emailSettings.SmtpPort = Configuration.GetValue<int>("Email:SMTPPort");
                emailSettings.SmtpUser = Configuration.GetValue<string>("Email:SMTPUser")!;
                emailSettings.SmtpPassword = Configuration.GetValue<string>("Email:SMTPPassword")!;
                emailSettings.TdwReturnedBox = Configuration.GetValue<string>("TDWReturnedBox");
                emailSettings.RegionEmails.Add("GAUTENG", Configuration.GetValue<string>("TDWEmail:GAUTENG")!);
                emailSettings.RegionEmails.Add("FREE STATE", Configuration.GetValue<string>("TDWEmail:FREE STATE")!);
                emailSettings.RegionEmails.Add("KWA-ZULU NATAL", Configuration.GetValue<string>("TDWEmail:KWA-ZULU NATAL")!);
                emailSettings.RegionEmails.Add("KWAZULU NATAL", Configuration.GetValue<string>("TDWEmail:KWA-ZULU NATAL")!);
                emailSettings.RegionEmails.Add("NORTH WEST", Configuration.GetValue<string>("TDWEmail:NORTH WEST")!);
                emailSettings.RegionEmails.Add("MPUMALANGA", Configuration.GetValue<string>("TDWEmail:MPUMALANGA")!);
                emailSettings.RegionEmails.Add("EASTERN CAPE", Configuration.GetValue<string>("TDWEmail:EASTERN CAPE")!);
                emailSettings.RegionEmails.Add("WESTERN CAPE", Configuration.GetValue<string>("TDWEmail:WESTERN CAPE")!);
                emailSettings.RegionEmails.Add("LIMPOPO", Configuration.GetValue<string>("TDWEmail:LIMPOPO")!);
                emailSettings.RegionEmails.Add("NORTHERN CAPE", Configuration.GetValue<string>("TDWEmail:NORTHERN CAPE")!);
                emailSettings.RegionIDEmails.Add("7", Configuration.GetValue<string>("TDWEmail:GAUTENG")!);
                emailSettings.RegionIDEmails.Add("4", Configuration.GetValue<string>("TDWEmail:FREE STATE")!);
                emailSettings.RegionIDEmails.Add("5", Configuration.GetValue<string>("TDWEmail:KWA-ZULU NATAL")!);
                emailSettings.RegionIDEmails.Add("6", Configuration.GetValue<string>("TDWEmail:NORTH WEST")!);
                emailSettings.RegionIDEmails.Add("8", Configuration.GetValue<string>("TDWEmail:MPUMALANGA")!);
                emailSettings.RegionIDEmails.Add("2", Configuration.GetValue<string>("TDWEmail:EASTERN CAPE")!);
                emailSettings.RegionIDEmails.Add("1", Configuration.GetValue<string>("TDWEmail:WESTERN CAPE")!);
                emailSettings.RegionIDEmails.Add("9", Configuration.GetValue<string>("TDWEmail:LIMPOPO")!);
                emailSettings.RegionIDEmails.Add("3", Configuration.GetValue<string>("TDWEmail:NORTHERN CAPE")!);
                return emailSettings;
            });
            services.AddSingleton<EmailClient>();
            services.AddSingleton<MailMessages>();

            

            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<Navigation>();
            services.AddScoped<CSService>();
            services.AddScoped<ReportDataService>();
            services.AddScoped<ProgressService>();
            services.AddScoped<Helper>();

            services.AddScoped<ActiveUser>();
            services.AddSingleton<ActiveUserList>();

            services.AddHttpClient();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.SlidingExpiration = true;
            });
            services.AddRazorPages();
            services.AddServerSideBlazor();
            //.AddCircuitOptions(options =>
            // {
            //     options.DetailedErrors = true;
            //     options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromSeconds(10);
            //     options.DisconnectedCircuitMaxRetained = 0;
            // });


            //services.AddSignalR(hubOptions =>
            //{
            //    hubOptions.EnableDetailedErrors = true;
            //    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(2);
            //    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
            //});

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(
            //        name: "AllowOrigin",
            //        builder => {
            //            builder.AllowAnyOrigin()
            //                    .AllowAnyMethod()
            //                    .AllowAnyHeader();
            //        });
            //});

            //services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Error");
            }
            //app.ApplicationServices.GetService<Navigation>();
            app.UseStaticFiles();
            app.UseRouting();
            var wsOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(10)
            };
            app.UseWebSockets(wsOptions);
            app.UseHttpsRedirection();
            //app.UseExceptionHandler(c => c.Run(async context =>
            //{
            //    var exception = context.Features
            //        .Get<IExceptionHandlerPathFeature>()
            //        .Error;
            //    var response = new { error = exception.Message };
            //    await context.Response.WriteAsJsonAsync(response);
            //}));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                //endpoints.MapBlazorHub(options => options.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1));
                endpoints.MapFallbackToPage("/_Host");
            });

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider("brmFiles"),
            //    RequestPath = new PathString(env.ContentRootPath + "\\brmFiles"),
            //    EnableDirectoryBrowsing = false
            //});
            //app.MapRazorComponents();


        }
    }
}
