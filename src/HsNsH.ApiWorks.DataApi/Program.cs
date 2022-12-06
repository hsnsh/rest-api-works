using HsNsH.ApiWorks.DataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HsNsH.ApiWorks.DataApi;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BookStoresDBContext>();
            await context.Database.MigrateAsync();

            if (!context.Roles.Any())
            {
                await context.Roles.AddRangeAsync(new[] { new Role() { RoleId = 1, RoleDesc = "Admin" }, new Role() { RoleId = 2, RoleDesc = "Public" } });
                await context.SaveChangesAsync();
            }

            if (!context.Publishers.Any())
            {
                await context.Publishers.AddRangeAsync(new[]
                {
                    new Publisher()
                    {
                        PubId = 1,
                        PublisherName = "Admin",
                        City = "c",
                        State = "s",
                        Country = "c"
                    },
                    new Publisher()
                    {
                        PubId = 2,
                        PublisherName = "Public",
                        City = "c",
                        State = "s",
                        Country = "c"
                    }
                });
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                await context.Users.AddRangeAsync(new[]
                {
                    new User()
                    {
                        FirstName = "Hasan",
                        MiddleName = "Huseyin",
                        LastName = "SAHIN",
                        EmailAddress = "hsnsh@outlook.com",
                        Password = "1q2w3E*",
                        Source = "src",
                        RoleId = 1,
                        PubId = 1
                    }
                });
                await context.SaveChangesAsync();
            }
        }

        await host.RunAsync();

        return 0;
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}