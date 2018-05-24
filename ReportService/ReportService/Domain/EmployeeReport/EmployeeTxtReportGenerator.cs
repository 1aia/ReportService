using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportService.Domain.EmployeeReport
{
    public class EmployeeTxtReportGenerator
    {
        public string GenerateContent(string reportName, List<Department> departmentList)
        {
            var reportLines = new List<string> {reportName, Line};

            foreach (var department in departmentList)
            {
                var departmentLines = department.EmployeeList
                    .Select(x => BuildDataLine(x.Name, x.Salary))
                    .ToList();

                if (!string.IsNullOrWhiteSpace(department.Name))
                {
                    departmentLines.Insert(0, department.Name);
                    departmentLines.Add(BuildDataLine("Всего по отделу", department.EmployeeList.Sum(y => y.Salary)));
                }

                reportLines.AddRange(departmentLines);
                reportLines.Add(Line);
            }

            reportLines.Add(BuildDataLine("Всего по предприятию", departmentList.Sum(x => x.EmployeeList.Sum(y => y.Salary))));

            return string.Join(Environment.NewLine, reportLines);
        }

        protected string BuildDataLine(string name, int salary) => $"{name} {salary}р";
        protected string Line => "--------------------------------------------";
    }
}