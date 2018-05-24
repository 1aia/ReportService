using System.Collections.Generic;
using Npgsql;
using ReportService.Interfaces;

namespace ReportService.Domain
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository()
        {
            _connectionString = "";
        }

        public IEnumerable<Employee> GetAll()
        {
            var employeeList = new List<Employee>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand(@"SELECT e.name, e.inn, d.name 
                                               FROM emps e 
                                               LEFT JOIN deps d on e.departmentid = d.id AND d.active = true", connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var emp = new Employee
                    {
                        Name = reader.GetString(0),
                        Inn = reader.GetString(1),
                        Department = reader.GetString(2)
                    };

                    employeeList.Add(emp);
                }

                reader.Close();
                connection.Close();
            };

            return employeeList;
        }
    }
}