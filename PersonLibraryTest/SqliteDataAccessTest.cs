using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonLibrary;
using PersonLibrary.Models;
using Xunit;
using PersonLibraryTest;

namespace PersonLibraryTest
{
    public class SqliteDataAccessTest
    {

        [Fact]
        public void LoadAllPersons_shouldGetList()
        {
            List<PersonMainModel> loadAllPersons = SqliteDataAccess.LoadAllPersons();
            Assert.True(loadAllPersons != null);
        }    
        
        [Fact]
        public void GetPersonsByGroup_shouldGetList()
        {
            List<PersonMainModel> byGroup = SqliteDataAccess.GetPersonsByGroup("employees");
            Assert.True(byGroup != null);
        }       
        
        [Fact]
        public void GetPersonById_returnSingleObject()
        {
            var personMainModel = SqliteDataAccess.GetPersonById(1,"managers");


                double result = 0.0;
                if (personMainModel is EmployeeModel)
                {
                    Assert.True(personMainModel is EmployeeModel);
                }
                else if (personMainModel is ManagerModel)
                {
                    Assert.True(personMainModel is ManagerModel);

                }
                else if (personMainModel is SalesManModel)
                {
                    Assert.True(personMainModel is SalesManModel);

                }
                else if(personMainModel is PersonMainModel)
                { 
                    Assert.True(personMainModel is PersonMainModel);
                }

                Assert.True(personMainModel != null);
        }

        private ManagerModel managerModel1 = new ManagerModel()
        {
            id = 1,
            name = "Bob",
            person_date = "20.05.2018",
            person_group = "managers",
            base_salary = 1000
        };

        [Fact]
        public void GetManegerSubSelesmans_Test()
        {
            List<SalesManModel> manegerSubSelesmans = SqliteDataAccess.GetManegerSubSelesmans(managerModel1);
            Assert.True(manegerSubSelesmans != null);
        }     
        
        [Fact]
        public void GetManegerSubEmployees_Test()
        {
            List<EmployeeModel> manegerSubEmployees = SqliteDataAccess.GetManegerSubEmployees(managerModel1);
            Assert.True(manegerSubEmployees != null);
        }

        private SalesManModel model = new SalesManModel()
        {
            id = 1,
            name = "Petro",
            person_date = "15.04.2000",
            person_group = "salesmans",
            base_salary = 1000
        };

        [Fact]
        public void GetSalesManSubManagers()
        {
            var salesManSubManagers = SqliteDataAccess.GetSalesManSubManagers(model);
            Assert.True(salesManSubManagers != null);
        }

        [Fact]
        public void GetSalesManSubEmployees()
        {
            List<EmployeeModel> salesManSubEmployees = SqliteDataAccess.GetSalesManSubEmployees(model);
            Assert.True(salesManSubEmployees != null);
        }



        SalesManModel salesManModel = new SalesManModel()
        {
            id = 4,
            name = "Ciri",
            person_date = "25.12.2020",
            person_group = "salesmans",
            base_salary = 1200
        };

        EmployeeModel employeeModel = new EmployeeModel()
        {
            id = 5,
            name = "Triss",
            person_date = "14.11.2020",
            person_group = "employees",
            base_salary = 1300
        };

        ManagerModel managerModel = new ManagerModel()
        {
            id = 5,
            name = "Geralt",
            person_date = "15.10.2020",
            person_group = "managers",
            base_salary = 1400
        };



        [Fact]
        public void AddPerson()
        {
            int addSalesman = SqliteDataAccess.AddPerson(salesManModel);
            Assert.True(addSalesman != 0);          
            
            int addEmployee = SqliteDataAccess.AddPerson(employeeModel);
            Assert.True(addEmployee != 0);         
            
            int addManager = SqliteDataAccess.AddPerson(managerModel);
            Assert.True(addManager != 0);
        }

        [Fact]
        public void UpdatePerson()
        {
            int addSalesman = SqliteDataAccess.UpdatePerson(salesManModel);
            Assert.True(addSalesman != 0);

            int addEmployee = SqliteDataAccess.UpdatePerson(employeeModel);
            Assert.True(addEmployee != 0);

            int addManager = SqliteDataAccess.UpdatePerson(managerModel);
            Assert.True(addManager != 0);

        }       
        
        [Fact]
        public void DeletePerson()
        {
            int addSalesman = SqliteDataAccess.DeletePerson(salesManModel);
            Assert.True(addSalesman != 0);

            int addEmployee = SqliteDataAccess.DeletePerson(employeeModel);
            Assert.True(addEmployee != 0);

            int addManager = SqliteDataAccess.DeletePerson(managerModel);
            Assert.True(addManager != 0);

        }       

    }
}
