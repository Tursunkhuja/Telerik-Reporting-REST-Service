namespace TelerikReportingRestService.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;
using Microsoft.AspNetCore.Cors;

[Route("api/reports")]
[ApiController]   // IMPORTANT - this is required, please visit https://docs.telerik.com/reporting/embedding-reports/host-the-report-engine-remotely/telerik-reporting-rest-services/asp.net-core-web-api-implementation/host-reports-service-in-.net-6-with-minimal-api
[EnableCors("AllowOrigin")]
public class ReportsController : ReportsControllerBase
{
    public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
        : base(reportServiceConfiguration)
    {
    }

    protected override HttpStatusCode SendMailMessage(MailMessage mailMessage)
    {
        throw new System.NotImplementedException("This method should be implemented in order to send mail messages");

        // using (var smtpClient = new SmtpClient("smtp01.mycompany.com", 25))
        // {
        //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    smtpClient.EnableSsl = false;

        // smtpClient.Send(mailMessage);
        // }
        // return HttpStatusCode.OK;
    }
}
