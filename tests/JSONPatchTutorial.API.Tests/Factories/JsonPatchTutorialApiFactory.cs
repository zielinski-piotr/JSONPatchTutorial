using System;
using JsonPatchTutorial.API;
using JSONPatchTutorial.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JSONPatchTutorial.API.Tests.Factories
{
    public class JsonPatchTutorialApiFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices
                    .GetRequiredService<JsonPatchDbContext>();

                try
                {
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = scopedServices.GetRequiredService<ILogger<HouseControllerTests>>();
                    logger.LogError(ex, "An error occurred seeding " +
                                        "the database with test messages. Error: {Message}",
                        ex.Message);
                }
            });
        }
    }
}