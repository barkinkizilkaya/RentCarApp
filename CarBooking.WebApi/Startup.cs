using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;
using CarBookingApp.Persistance;
using Microsoft.EntityFrameworkCore;
using RentCarApp.Core.Services;
using CarBookingApp.Core.DataServices;
using CarBookingApp.Persistance.Repositories;
using Microsoft.OpenApi.Models;

namespace CarBooking.WebApi
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
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("V8", new OpenApiInfo { Title = "TestApi", Version = "V8" });
            });

            var connString = "Filename=:memory:";
            var conn = new SqliteConnection(connString);
            conn.Open();

          

            //services.AddDbContext<CarBookingAppDbContext>(opt => opt.UseSqlite(conn));
            services.AddDbContext<CarBookingAppDbContext>(opt => opt.UseSqlite(conn));

            ensureDatabaseCreated(conn);
            services.AddTransient<ICarBookingRequestProcessor, CarBookingRequestProcessor>();
            services.AddTransient<ICarBookingService, CarBookingService>();

        }

        private static void ensureDatabaseCreated(SqliteConnection coon)
        {
            var builder = new DbContextOptionsBuilder<CarBookingAppDbContext>();
            builder.UseSqlite(coon);

            using var context = new CarBookingAppDbContext(builder.Options);
            context.Database.EnsureCreated();
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

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/V8/swagger.json", "TestApi V8"));
        }
    }
}
