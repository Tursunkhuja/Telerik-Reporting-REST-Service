using Microsoft.Extensions.DependencyInjection.Extensions;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers().AddNewtonsoftJson();

var reportsPath = Path.Combine(builder.Environment.WebRootPath);

// Configure dependencies for ReportsController.
builder.Services.TryAddSingleton<IReportServiceConfiguration>(sp =>
    new ReportServiceConfiguration
    {
        // The default ReportingEngineConfiguration will be initialized from appsettings.json or appsettings.{EnvironmentName}.json:
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),

        // In case the ReportingEngineConfiguration needs to be loaded from a specific configuration file, use the approach below:
        //ReportingEngineConfiguration = ResolveSpecificReportingConfiguration(sp.GetService<IWebHostEnvironment>()),
        HostAppId = "Net6RestServiceWithCors",
        Storage = new FileStorage(),
        ReportSourceResolver = new TypeReportSourceResolver()
            .AddFallbackResolver(
                new UriReportSourceResolver(reportsPath))
    });

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowOrigin");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();