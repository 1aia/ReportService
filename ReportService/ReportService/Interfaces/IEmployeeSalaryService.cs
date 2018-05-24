using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Interfaces
{
    public interface IEmployeeSalaryService
    {
        Task SetSalaryAsync(Employee employee);
    }
}