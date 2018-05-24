using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReportService.Interfaces;

namespace ReportService.Domain
{
    public class EmployeeSalaryService : IEmployeeSalaryService
    {
        private readonly IEmployeeCodeResolver _employeeCodeResolver;
        protected static readonly HttpClient Client = new HttpClient();

        public EmployeeSalaryService(IEmployeeCodeResolver employeeCodeResolver)
        {
            _employeeCodeResolver = employeeCodeResolver;
        }

        public async Task SetSalaryAsync(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.BuhCode))
            {
                employee.BuhCode = await _employeeCodeResolver.GetCodeAsync(employee.Inn);
            }

            var salaryText = await GetSalary(employee);

            decimal salary;
            if (decimal.TryParse(salaryText, out salary))
            {
                employee.Salary = (int)salary;
            };
        }

        protected async Task<string> GetSalary(Employee employee)
        {
            var url = "http://salary.local/api/empcode/" + employee.Inn;

            var json = JsonConvert.SerializeObject(new { employee.BuhCode });
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var result = await Client.PostAsync(url, content);

            return await result.Content.ReadAsStringAsync();
        }
    }

}