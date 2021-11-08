using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using WorldOfAdventures.BusinessLogic;
using WorldOfAdventures.DAL;

namespace WorldOfAdventures.Api
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
            services.AddSwaggerGen();

            services.AddSingleton(sp =>
            {
                var client = new MongoClient(Configuration.GetConnectionString("MongoDb"));

                return client.GetDatabase("WorldOfAdventures");
            });

            services.AddSingleton<IAdventureRepository, AdventureRepository>();
            services.AddSingleton<IUserAdventureRepository, UserAdventureRepository>();

            services.AddTransient<IValidationService, ValidationService>();
            services.AddTransient<IAdventureService, AdventureService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
