using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonLibrary.Models;

namespace PersonLibrary
{
   public class CalculateSalary
    {

        private double getEmployeeSalary(EmployeeModel employee)
        {
            double employeeSalary;
            DateTime dateTimeCurrent = DateTime.Now;
            DateTime personDateTime = DateTime.Parse(employee.person_date);
            int years = new DateTime(dateTimeCurrent.Subtract(personDateTime).Ticks).Year - 1;
            double commonPercent = years * 3;
            if (commonPercent <= 30)
            {
                employeeSalary = employee.base_salary + (employee.base_salary * (commonPercent / 100));
            }
            else
            {
                commonPercent = 30;
                employeeSalary = employee.base_salary + (employee.base_salary * (commonPercent / 100));
            }
            return employeeSalary;
        }     
        
        private double getManagerSalary(ManagerModel manager)
        {
            double managerSalary;
            double subEmployeeSalary = 0;
            List<EmployeeModel> manegerSubEmployees = SqliteDataAccess.GetManegerSubEmployees(manager);

            foreach (var employee in manegerSubEmployees)
            {
                 subEmployeeSalary += getEmployeeSalary(employee);
            }

            DateTime dateTimeCurrent = DateTime.Now;
            DateTime personDateTime = DateTime.Parse(manager.person_date);
            int years = new DateTime(dateTimeCurrent.Subtract(personDateTime).Ticks).Year - 1;
            double commonPercent = years * 5;
            if (commonPercent <= 40)
            {
                managerSalary = manager.base_salary + ((manager.base_salary * (commonPercent / 100)) + (subEmployeeSalary * 0.005));
            }
            else
            {
                commonPercent = 40;
                managerSalary = manager.base_salary + ((manager.base_salary * (commonPercent / 100)) + (subEmployeeSalary * 0.005));
            }

            return managerSalary;

        }      
        
        private double getSalesmanSalary(SalesManModel salesMan)
        {
            double salesmanSalary;
            double subManagerSalary = 0;
            double subEmployeeSalary = 0;
            List<EmployeeModel> salesmanSubEmployees = SqliteDataAccess.GetSalesManSubEmployees(salesMan);
            List<ManagerModel> salesManSubManagers = SqliteDataAccess.GetSalesManSubManagers(salesMan);

            foreach (var employee in salesmanSubEmployees)
            {
                subEmployeeSalary += getEmployeeSalary(employee);
            }          
            
            foreach (var manager in salesManSubManagers)
            {
                subManagerSalary += getManagerSalary(manager);
            }

            DateTime dateTimeCurrent = DateTime.Now;
            DateTime personDateTime = DateTime.Parse(salesMan.person_date);
            int years = new DateTime(dateTimeCurrent.Subtract(personDateTime).Ticks).Year - 1;
            double commonPercent = years;
            if (commonPercent <= 35)
            {
                salesmanSalary = salesMan.base_salary + ((salesMan.base_salary * (commonPercent / 100)) + ((subEmployeeSalary + subManagerSalary) * 0.003));
            }
            else
            {
                commonPercent = 35;
                salesmanSalary = salesMan.base_salary + ((salesMan.base_salary * (commonPercent / 100)) + ((subEmployeeSalary + subManagerSalary) * 0.003));
            }

            return salesmanSalary;
        }

        public double getSalaryWithPercentage(PersonMainModel personMain)
        {
            double result = 0.0;
            if (personMain is EmployeeModel)
            {
                EmployeeModel employee = (EmployeeModel)personMain;
                result = getEmployeeSalary(employee);
            }
            else if (personMain is ManagerModel)
            {
                ManagerModel manager = (ManagerModel) personMain;
                result = getManagerSalary(manager);

            }
            else if(personMain is SalesManModel)
            {
                SalesManModel salesMan = (SalesManModel) personMain;
                result = getSalesmanSalary(salesMan);
            }
            return result;
        }



    }
}
