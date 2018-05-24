namespace ReportService.Domain.EmployeeReport
{
    public class ReportResult
    {
        public string ContentType { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }

        public string Error { get; set; }

        public bool Success => string.IsNullOrWhiteSpace(Error);
    }
}