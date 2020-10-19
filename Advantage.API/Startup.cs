using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Advantage.API
{
    public class Startup
    {
        private string _connectionString = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //cors policy, cross origin resource sharing
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            _connectionString = Configuration["connectionString"]; //coming from user secrets, connectionString is stored as secret
            services.AddControllers();
            services.AddEntityFrameworkNpgsql().AddDbContext<APIContext>(opt => opt.UseNpgsql(_connectionString));
            services.AddTransient<DataSeed>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeed seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CorsPolicy");
            }

            app.UseHttpsRedirection();

            seed.SeedData(20, 500); //20 customers and 500 orders
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action}/{id?}");
            });

        }
    }
}
