using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeoplesDb.Api.People.Models;
using PeoplesDb.Api.People.Repositories;

namespace PeoplesDb.Api.People
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddDbContext<PersonContext>(opt =>
            {
                string connectionString = Configuration.GetConnectionString("PeopleContext");
                if(!string.IsNullOrEmpty(connectionString))
                {
                    opt.UseSqlServer(connectionString);
                }
                else
                {
                    opt.UseInMemoryDatabase("PeopleContext");
                }
            });
        }

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
