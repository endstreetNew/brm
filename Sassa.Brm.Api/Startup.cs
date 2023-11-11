using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Sassa.BRM.Models;
using Sassa.BRM.Services;

namespace Sassa.Brm.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Add services to the container.
            services.AddScoped<BRMDbService>().AddDbContext<ModelContext>(options =>
            options.UseOracle(_configuration.GetConnectionString("BrmConnection")!));
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

        }
    }
}
