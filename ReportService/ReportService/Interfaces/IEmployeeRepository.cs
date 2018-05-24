using System.Collections.Generic;
using ReportService.Domain;

namespace ReportService.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
    }
}