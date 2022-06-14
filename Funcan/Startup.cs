using Funcan.Controllers.Session;
using Funcan.Domain.Parsers;
using Funcan.Domain.Plotters;
using Funcan.Domain.Repository;
using Funcan.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Funcan;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddSingleton<IPlotterService, PlotterService>();
        services.AddSingleton<IFunctionParser, FunctionParser>();
        services.AddSingleton<DiscontinuitiesPlotter>();
        services.AddSingleton<IPlotter, DiscontinuitiesPlotter>();
        services.AddSingleton<IPlotter, FunctionPlotter>();
        services.AddSingleton<FunctionPlotter>();
        services.AddSingleton<IPlotter, InflectionPointsPlotter>();
        services.AddSingleton<IPlotter, ExtremaPlotter>();
        services.AddSingleton<IPlotter, AsymptotePlotter>();
        services.AddSingleton<ISessionManager, CookieSessionManager>();
        services.AddSingleton<IHistoryRepository, MemoryHistoryRepository>();
        services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Funcan", Version = "v1" }));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Funcan v1"));
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}