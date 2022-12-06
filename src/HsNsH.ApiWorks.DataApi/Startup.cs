using HsNsH.ApiWorks.DataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HsNsH.ApiWorks.DataApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        services.AddDbContext<BookStoresDBContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("BookStoresDB")));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
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