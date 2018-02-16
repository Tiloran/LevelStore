using LevelStore.Models;
using LevelStore.Models.EF;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace LevelStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            
            
                var context = services.GetRequiredService<ApplicationDbContext>();
                SeedingDB.EnsurePopulated(context);
            
            
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
