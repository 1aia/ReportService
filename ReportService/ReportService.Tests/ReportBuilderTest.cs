using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ReportService.Domain;
using ReportService.Domain.EmployeeReport;
using ReportService.Interfaces;

namespace ReportService.Tests
{
    public class ReportBuilderBase
    {
        protected IEmployeeRepository repository;
        protected IEmployeeSalaryService salaryService;
        protected EmployeeReportDataProvider employeeReportDataProvider;
        protected List<Department> Departments;

        [SetUp]
        public void Setup()
        {
            var salaryServiceMock = new Mock<IEmployeeSalaryService>();
            salaryServiceMock.Setup(x => x.SetSalaryAsync(It.IsAny<Employee>()))
                .Callback<Employee>(x => x.Salary = 10000)
                .Returns(Task.CompletedTask);

            salaryService = salaryServiceMock.Object;

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock.Setup(x => x.GetAll())
                .Returns(new List<Employee> {
                    new Employee { Name = "Василий Васильевич Кузнецов", Inn = "1650121210" },
                    new Employee { Name = "Демьян Сергеевич Коротченко", Inn = "1650121211" },
                    new Employee { Name = "Михаил Андреевич Суслов", Inn = "1650121212" },
                    new Employee { Department = "HR", Name = "Фрол Романович Козлов", Inn = "1650121213" },
                    new Employee { Department = "HR", Name = "Дмитрий Степанович Полянски", Inn = "1650121214" },
                    new Employee { Department = "HR", Name = "Андрей Павлович Кириленко", Inn = "1650121215" },
                    new Employee { Department = "PR", Name = "Арвид Янович Пельше", Inn = "1650121216" },
                    new Employee { Department = "PR", Name = "Алексей Иванович Рыков", Inn = "1650121217" },
                });
            repository = repositoryMock.Object;


            employeeReportDataProvider = new EmployeeReportDataProvider(repository, salaryService);

            Departments = new List<Department> {
                new Department
                {
                    Name = "DEP1",
                    EmployeeList = new List<Employee>
                    {
                        new Employee { Salary = 1 },
                        new Employee { Salary = 2 },
                        new Employee { Salary = 3 }
                    }
                },

                new Department
                {
                    Name = "DEP2",
                    EmployeeList = new List<Employee>
                    {
                        new Employee { Salary = 4 },
                        new Employee { Salary = 5 },
                    }
                }
            };
        }

        [Test]
        public void ReportHasSuccessResult()
        {
            var report = new EmployeeReportBuilder(employeeReportDataProvider);

            var result = report.Build(2018, 5);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Name, "report.txt");
        }

        [Test]
        public void EmployeesPackedToDepartments()
        {
            var departments = employeeReportDataProvider.GetReportData();

            Assert.AreEqual(departments.Count, 3);
        }

        [Test]
        public void EmployeesSalariesRetrived()
        {
            var departments = employeeReportDataProvider.GetReportData();

            Assert.IsTrue(departments.All(x => x.EmployeeList.All(y => y.Salary > 0)));
        }

        [Test]
        public void ReportContentIsCorrect()
        {
            var department = new Department
            {
                Name = "DPT",
                EmployeeList = new List<Employee>
                {
                    new Employee { Salary = 1, Name = "TEST"}
                }
            };

            var content = new EmployeeTxtReportGenerator().GenerateContent("Тест", new List<Department> { department });

            var expected = $"Тест{Environment.NewLine}" +
                           $"--------------------------------------------{Environment.NewLine}" +
                           $"DPT{Environment.NewLine}" +
                           $"TEST 1р{Environment.NewLine}" +
                           $"Всего по отделу 1р{Environment.NewLine}" +
                           $"--------------------------------------------{Environment.NewLine}" +
                           "Всего по предприятию 1р";
            Assert.AreEqual(content, expected);
        }

        [Test]
        public void OverallSummaryExists()
        {
            var content = new EmployeeTxtReportGenerator().GenerateContent(string.Empty, employeeReportDataProvider.GetReportData());

            Assert.IsTrue(content.Contains("Всего по предприятию 80000р"));
        }

        [Test]
        public void DepartmentSummariesAreCorrect()
        {
            var content = new EmployeeTxtReportGenerator().GenerateContent(string.Empty, Departments);

            Assert.IsTrue(content.Contains("Всего по отделу 6р"));
            Assert.IsTrue(content.Contains("Всего по отделу 9р"));
        }

        [Test]
        public void OverallSummaryIsCorrect()
        {
            var content = new EmployeeTxtReportGenerator().GenerateContent(string.Empty, Departments);

            Assert.AreEqual(content.Split(Environment.NewLine).Last(), "Всего по предприятию 15р");
        }
    }
}