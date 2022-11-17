namespace HsNsH.ApiWorks.JsonTokenAuthApi;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

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