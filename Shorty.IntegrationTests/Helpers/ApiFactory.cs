using Microsoft.AspNetCore.Mvc.Testing;

namespace Shorty.IntegrationTests.Helpers;

public class ApiFactory : WebApplicationFactory<Program>
{
    /*protected override IHostBuilder? CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.RemoveAll<ApplicationDbContext>();
                    services.AddDbContext<ApplicationDbContext>(builder =>
                    {
                        //use in memory
                        builder.UseInMemoryDatabase("integration-test");
                    });
                })
                .ConfigureWebHostDefaults(b => b.Configure(app => { }))
            ;
    }*/
}