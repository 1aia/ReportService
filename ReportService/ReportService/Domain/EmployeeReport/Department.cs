using System.Collections.Generic;

namespace ReportService.Domain.EmployeeReport
{
    public class Department
    {
        public string Name { get; set; }
        
        public List<Employee> EmployeeList { get; set; }
    }
}