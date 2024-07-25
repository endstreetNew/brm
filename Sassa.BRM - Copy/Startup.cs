using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
using razor.Components;
using Sassa.BRM.Models;
using Sassa.BRM.Services;
using Sassa.eDocs;
using Sassa.Socpen.Data;
using System;

namespace Sassa.BRM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string BrmConnectionString = Configuration.GetConnectionString("BrmConnection");

            services.AddHttpContextAccessor();
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddSingleton<StaticD>();
            //Factory pattern
            services.AddDbContextFactory<ModelContext>(options => 
            options.UseOracle(BrmConnectionString));
            //Services 
            services.AddScoped<SessionService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(BrmConnectionString));
            services.AddScoped<BRMDbService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(BrmConnectionString));
            services.AddScoped<MisFileService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(BrmConnectionString));
            services.AddScoped<DestructionService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(BrmConnectionString));
            services.AddScoped<SocpenService>().AddDbContext<SocpenContext>(options =>
            options.UseOracle(BrmConnectionString));
            services.AddScoped<StaticService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(BrmConnectionString)); 
            services.AddScoped<TdwBatchService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(BrmConnectionString)); 
            services.AddSingleton<BarCodeService>();
            services.AddSingleton<RawSqlService>();
            services.AddSingleton<MailMessages>();
            services.AddSingleton<FileService>();

            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<Navigation>();
            services.AddScoped<CSService>();
            services.AddScoped<ReportDataService>();
            services.AddScoped<ProgressService>();

            services.AddScoped<ActiveUser>();
            services.AddSingleton<ActiveUserList>();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });
            //Hosted services on timer
            //services.AddHostedService<TimedService>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
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

            services.AddControllers();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //#if DEBUG
                // For Debug in Kestrel
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BRM API V1");
                //#else
                //               // To deploy on IIS
                //               c.SwaggerEndpoint("/eServicesApi/swagger/v1/swagger.json", "Web API V1");
                //#endif
                //string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "";
                //c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Sassa Services API V1");
                c.RoutePrefix = "api";
            });

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
                KeepAliveInterval = TimeSpan.FromSeconds(120)
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


        }
    }
}