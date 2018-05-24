using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportService.Interfaces;

namespace ReportService.Domain.EmployeeReport
{
    public class EmployeeReportDataProvider
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeSalaryService _employeeSalaryService;

        public EmployeeReportDataProvider(IEmployeeRepository employeeRepository, 
            IEmployeeSalaryService employeeSalaryService)
        {
            _employeeRepository = employeeRepository;
            _employeeSalaryService = employeeSalaryService;
        }

        public List<Department> GetReportData()
        {
            var employeeList = _employeeRepository.GetAll().ToList();
            var setSalaryTasks = employeeList.Select(x => _employeeSalaryService.SetSalaryAsync(x)).ToArray();
            Task.WaitAll(setSalaryTasks);
            
            var departments = employeeList
                .GroupBy(x => string.IsNullOrWhiteSpace(x.Department) ? string.Empty : x.Department)
                .Select(x => new Department
                {
                    Name = x.Key,
                    EmployeeList = x.ToList()
                })
                .ToList();

            return departments;
        }
    }
}
