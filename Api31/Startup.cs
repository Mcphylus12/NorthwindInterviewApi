using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Core;
using Core.Commands;
using Core.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api31
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var startingData = JsonSerializer.Deserialize<List<CompanyModel>>(File.ReadAllText("StartingData.json"), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            services.AddTransient<ICommand<BuyoutRequest>, BuyoutCommand>();
            services.AddSingleton<IStore>(new Store(startingData));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
