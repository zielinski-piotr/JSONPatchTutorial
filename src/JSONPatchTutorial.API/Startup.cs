using System;
using System.Linq;
using AutoMapper;
using JSONPatchTutorial.Data;
using JSONPatchTutorial.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace JsonPatchTutorial.API
{
    public class Startup
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _keepAliveConnection;

        public Startup()
        {
            _connectionString = $"DataSource={GenerateDbName()};mode=memory;cache=shared";
            _keepAliveConnection = new SqliteConnection(_connectionString);
            _keepAliveConnection.Open();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JSONPatchTutorial.API", Version = "v1" });
            });

            services.AddDbContext<JsonPatchDbContext>(
                options =>
                    options.UseSqlite(_connectionString));

            ConfigureAutomapper(services);

            services.AddTransient<IHouseService, HouseService>();
            services.AddTransient<IRepository, Repository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JSONPatchTutorial.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ConfigureAutomapper(IServiceCollection services)
        {
            services.AddSingleton(_ => new MapperConfiguration(c => { c.AddProfile<MapperProfile>(); }).CreateMapper());
        }

        private string GenerateDbName()
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}