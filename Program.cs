using Microsoft.Extensions.DependencyInjection.Extensions;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

var reportsPath = Path.Combine(builder.Environment.WebRootPath);

// Configure dependencies for ReportsController.
builder.Services.TryAddSingleton<IReportServiceConfiguration>(sp =>
    new ReportServiceConfiguration
    {
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(), //uses default configuration
        HostAppId = "TelerikReportingRestService", // host app ID for this app
        Storage = new FileStorage(),
        ReportSourceResolver = new TypeReportSourceResolver().AddFallbackResolver(new UriReportSourceResolver(reportsPath))
    });

// CORS policy that will allow any origin. We use this for the ReportsController (might not be appropriate for other controllers)
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


var app = builder.Build();

// Allows the use of static files
app.UseStaticFiles();

app.UseRouting();

// Enable the CORS policy
app.UseCors("AllowOrigin");

// enable use of the API controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();