namespace ReportService.Domain.EmployeeReport
{
    public class EmployeeReportBuilder
    {
        private readonly EmployeeReportDataProvider _employeeReportDataProvider;

        public EmployeeReportBuilder(EmployeeReportDataProvider employeeReportDataProvider)
        {
            _employeeReportDataProvider = employeeReportDataProvider;
        }

        public ReportResult Build(int year, int month)
        {
            var reportName = MonthNameResolver.MonthName.GetName(year, month);

            var data = _employeeReportDataProvider.GetReportData();
            var reportContent = new EmployeeTxtReportGenerator().GenerateContent(reportName, data);

            return new ReportResult
            {
                ContentType = "application/octet-stream",
                Name = "report.txt",
                Data = System.Text.Encoding.UTF8.GetBytes(reportContent)
            };
        }
    }
}
