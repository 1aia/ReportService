using Microsoft.AspNetCore.Mvc;
using ReportService.Domain.EmployeeReport;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly EmployeeReportBuilder _reportBuilder;

        public ReportController(EmployeeReportBuilder reportBuilder)
        {
            _reportBuilder = reportBuilder;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public IActionResult Download(int year, int month)
        {
            var reportResult = _reportBuilder.Build(year, month);

            if (reportResult.Success)
            {
                return File(reportResult.Data, reportResult.ContentType, reportResult.Name);
            }

            return Content(reportResult.Error);
        }
    }
}
