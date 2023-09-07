using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Sassa.BRM.Models;
using Sassa.BRM.Services;

namespace Sassa.Brm.Api
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

            // Add services to the container.
            services.AddScoped<BRMDbService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(Configuration.GetConnectionString("BrmConnection")));
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //app.MapControllers();

            //    app.Run();
            //------------------------------------------------------------------
            //app.UseSwagger();

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            //// specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    //#if DEBUG
            //    // For Debug in Kestrel
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BRM API V1");
            //    //#else
            //    //               // To deploy on IIS
            //    //               c.SwaggerEndpoint("/eServicesApi/swagger/v1/swagger.json", "Web API V1");
            //    //#endif
            //    //string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "";
            //    //c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Sassa Services API V1");
            //    c.RoutePrefix = "api";
            //});

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    //app.UseDeveloperExceptionPage();
            //    app.UseExceptionHandler("/Error");
            //}
            ////app.ApplicationServices.GetService<Navigation>();
            //app.UseStaticFiles();
            //app.UseRouting();
            //var wsOptions = new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(120)
            //};
            //app.UseWebSockets(wsOptions);
            //app.UseHttpsRedirection();
            //app.UseExceptionHandler(c => c.Run(async context =>
            //{
            //    var exception = context.Features
            //        .Get<IExceptionHandlerPathFeature>()
            //        .Error;
            //    var response = new { error = exception.Message };
            //    await context.Response.WriteAsJsonAsync(response);
            //}));
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //    //endpoints.MapBlazorHub();
            //    //endpoints.MapBlazorHub(options => options.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1));
            //   // endpoints.MapFallbackToPage("/_Host");
            //});

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider("brmFiles"),
            //    RequestPath = new PathString(env.ContentRootPath + "\\brmFiles"),
            //    EnableDirectoryBrowsing = false
            //});


        //}
        //    // Configure the HTTP request pipeline.
        //    if (app.Environment.IsDevelopment())
        //    {
        //        app.UseSwagger();
        //        app.UseSwaggerUI();
        //    }

        //    app.UseHttpsRedirection();

        //    app.UseAuthorization();

        //    app.MapControllers();

        //    app.Run();
        }
    }
}
