using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Runtime.InteropServices;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.WebReportDesigner.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

var reportsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

// CORS policy that will allow any origin. We use this for the ReportsController (might not be appropriate for other controllers)
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Configure dependencies for ReportsController.
builder.Services.TryAddSingleton<IReportServiceConfiguration>(sp =>
    new ReportServiceConfiguration
    {
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(), //uses default configuration
        HostAppId = "TelerikReportingRestService", // host app ID for this app
        Storage = new FileStorage(),
        ReportSourceResolver = new TypeReportSourceResolver().AddFallbackResolver(new UriReportSourceResolver(reportsPath))
    });

#region Web Designer Services
builder.Services.TryAddSingleton<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
{
    DefinitionStorage = new FileDefinitionStorage(reportsPath),
    ResourceStorage = new ResourceStorage(Path.Combine(reportsPath, "Resources")),
    SettingsStorage = new FileSettingsStorage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting"))
});
#endregion

#region OpenTelemetry
builder.Services.AddOpenTelemetryTracing(b =>
{
    b.SetResourceBuilder(
        ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
    .AddAspNetCoreInstrumentation()
    .AddOtlpExporter(opts => { opts.Endpoint = new Uri("http://localhost:4317/"); });
});
#endregion

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


//EnableTracing();
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    CopyFontsInLinux();
}

app.Run();


static void EnableTracing()
{
    System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(File.CreateText("log.txt")));
    System.Diagnostics.Trace.AutoFlush = true;
}

static void CopyFontsInLinux()
{
    var fonts = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Fonts"), "*.ttf");
    string fontFolder = @"/usr/share/fonts";
    foreach (var font in fonts)
    {
        var destFileName = Path.Combine(fontFolder, Path.GetFileName(font));
        File.Copy(font, destFileName, true);
    }
}