using System.Net.Http;
using System.Threading.Tasks;
using ReportService.Interfaces;

namespace ReportService.Domain
{
    public class EmployeeCodeResolver : IEmployeeCodeResolver
    {
        protected static readonly HttpClient Client = new HttpClient();
        public async Task<string> GetCodeAsync(string inn)
        {
            return await Client.GetStringAsync("http://buh.local/api/inn/" + inn);
        }
    }
}