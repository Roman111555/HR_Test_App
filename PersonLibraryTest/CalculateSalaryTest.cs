using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonLibrary;
using PersonLibrary.Models;
using PersonLibraryTest;
using Xunit;

namespace PersonLibraryTest
{
    public class CalculateSalaryTest
    {
        private CalculateSalary calculateSalary = new CalculateSalary();

        [Fact]
        public void EmployeeSalary()
        {
            EmployeeModel employeeModel = new EmployeeModel()
            {
                id = 1,
                name = "Kastro",
                person_date = "17.11.2014",
                person_group = "employees",
                base_salary = 1000
            };
            double salaryWithPercentage = calculateSalary.getSalaryWithPercentage(employeeModel);
            Assert.True(salaryWithPercentage != null);
        }

        [Fact]
        public void getManagerSalary()
        {
 
          ManagerModel managerModel = new ManagerModel()
          {
              id = 1,
              name = "Bob",
              person_date = "20.05.2018",
              person_group = "managers",
              base_salary = 1000
          };

          double salaryWithPercentage = calculateSalary.getSalaryWithPercentage(managerModel);
          Assert.True(salaryWithPercentage != null);
        }

        [Fact]
        public void SalesManSalary()
        {
        
            SalesManModel model = new SalesManModel()
        {
            id = 1,
            name = "Petro",
            person_date = "15.04.2000",
            person_group = "salesmans",
            base_salary = 1000
        };
            double salaryWithPercentage = calculateSalary.getSalaryWithPercentage(model);
            Assert.True(salaryWithPercentage != null);

        }
    }
}
