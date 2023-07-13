using Telerik.Reporting;
using Telerik.Reporting.Services;

internal class CustomReportResolver : IReportSourceResolver
{
    static UriReportSource reportSource = new UriReportSource();    
    public ReportSource Resolve(string report, OperationOrigin operationOrigin, IDictionary<string, object> currentParameterValues)
    {
        var reportsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

        if (currentParameterValues.ContainsKey("TestParam"))
        {
            int r = (new Random()).Next(100, 1000);
            currentParameterValues["TestParam"] = r.ToString();
        }
        reportSource.Uri = Path.Combine(reportsPath, report);        
        return reportSource;
    }
}