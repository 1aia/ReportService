using System.Threading.Tasks;

namespace ReportService.Interfaces
{
    public interface IEmployeeCodeResolver
    {
        Task<string> GetCodeAsync(string inn);
    }
}