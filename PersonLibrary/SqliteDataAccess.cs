using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonLibrary.Models;

namespace PersonLibrary
{
    public class SqliteDataAccess
    {

        public static List<PersonMainModel> LoadAllPersons()
        {
            string getAllPersons = @"SELECT * FROM managers mg
             UNION 
             SELECT * FROM salesmans sm
             UNION
             SELECT em.id, em.name, em.person_date, em.person_group, em.base_salary 
             FROM employees em
             ORDER by person_group DESC";

            List<PersonMainModel> list = new List<PersonMainModel>();
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<PersonMainModel>(getAllPersons, new DynamicParameters());
                    foreach (var model in output) list.Add(model);
                }
            }
            catch (Exception e)
            {
                list = null;
            }

            return list;
        }

        public static List<PersonMainModel> GetPersonsByGroup(string groupID)
        {
            string getPersonByGroupRequest = $"SELECT * FROM {groupID} tb where tb.person_group = '{groupID}'";
            List<PersonMainModel> list = new List<PersonMainModel>();

            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<PersonMainModel>(getPersonByGroupRequest, new DynamicParameters());
                    foreach (var model in output) list.Add(model);
                }
            }
            catch (Exception e)
            {
                list = null;
            }

            return list;
        }

        public static PersonMainModel GetPersonById(int personID, string groupID)
        {
            string getPersonByIDRequest = $"SELECT * FROM {groupID} tb where tb.id = {personID}";

            PersonMainModel output = null;
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    if (groupID.ToLower().Equals("employees"))
                    {
                        output = cnn.QuerySingle<EmployeeModel>(getPersonByIDRequest);
                    }
                    else if (groupID.ToLower().Equals("managers"))
                    {
                        output = cnn.QuerySingle<ManagerModel>(getPersonByIDRequest);
                    }
                    else if (groupID.ToLower().Equals("salesman"))
                    {
                        output = cnn.QuerySingle<SalesManModel>(getPersonByIDRequest);
                    }
                }
            }
            catch (Exception e)
            {
                output = null;
            }

            return output;
        }

        public static List<SalesManModel> GetManegerSubSelesmans(ManagerModel manager)
        {

            string getSubSalesManRequest = "SELECT sl.* from managers mg " +
                                           "INNER JOIN sub_table sb " +
                                           "ON mg.id = sb.manager_id " +
                                           "INNER JOIN salesmans sl " +
                                           "ON sb.salesman_id = sl.id " +
                                           $"WHERE mg.id = {manager.id} " +
                                           "ORDER by sl.name DESC ";

            List<SalesManModel> sub_salesMan = new List<SalesManModel>();
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<SalesManModel>(getSubSalesManRequest, new DynamicParameters());
                    foreach (var model in output) sub_salesMan.Add(model);
                }
            }
            catch (Exception e)
            {
                sub_salesMan = null;
            }

            return sub_salesMan;
        }

        public static List<EmployeeModel> GetManegerSubEmployees(ManagerModel manager)
        {
            string getSubEmployeeRequest = "SELECT em.* from managers mg " +
                                           "INNER JOIN employees em " +
                                           "ON mg.id = em.chef_id_manager " +
                                           $"WHERE mg.id = {manager.id} " +
                                           "ORDER by em.name DESC;";

            List<EmployeeModel> sub_employees = new List<EmployeeModel>();
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output1 = cnn.Query<EmployeeModel>(getSubEmployeeRequest, new DynamicParameters());
                    foreach (var model1 in output1) sub_employees.Add(model1);
                }
            }
            catch (Exception e)
            {
                sub_employees = null;
            }

            return sub_employees;
        }

        public static List<ManagerModel> GetSalesManSubManagers(SalesManModel salesMan)
        {
            string getSubManagerRequest = @"SELECT mg.* from salesmans sl
                                            INNER JOIN sub_table sb
                                            ON sl.id = sb.salesman_id
                                            INNER JOIN managers mg
                                            ON sb.manager_id = mg.id " +
                                          $"WHERE sl.id = {salesMan.id} " +
                                          "ORDER by mg.name DESC;";

            List<ManagerModel> sub_managers = new List<ManagerModel>();
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<ManagerModel>(getSubManagerRequest, new DynamicParameters());
                    foreach (var model in output) sub_managers.Add(model);
                }
            }
            catch (Exception e)
            {
                sub_managers = null;
            }

            return sub_managers;
        }

        public static List<EmployeeModel> GetSalesManSubEmployees(SalesManModel salesMan)
        {
            string getSubEmployeesRequest = @"SELECT em.* from salesmans sl
                                              INNER JOIN employees em
                                              ON sl.id = em.chef_id_salesman " +
                                            $"WHERE sl.id = {salesMan.id} " +
                                            "ORDER by em.name DESC;";

            List<EmployeeModel> sub_employees = new List<EmployeeModel>();
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output1 = cnn.Query<EmployeeModel>(getSubEmployeesRequest, new DynamicParameters());
                    foreach (var model1 in output1) sub_employees.Add(model1);
                }
            }
            catch (Exception e)
            {
                sub_employees = null;
            }

            return sub_employees;
        }

        public static void AddEmployeeSubToManager(EmployeeModel empl, ManagerModel manager)
        {
            //TODO will add check into db for existing users and will add sub to manager
        }

        public static void AddEmployeeSubToSalesMan(EmployeeModel empl, SalesManModel salesMan)
        {
            //TODO will add check into db for existing users and will add sub to salesMan
        }

        public static void AddManagerSubToSalesman(ManagerModel manager, SalesManModel salesManModel)
        {
            //TODO added check into db for existing  users and will add sub manager to salesMan
        }

        public static int AddPerson(PersonMainModel person)
        {
            string addPersonRequest =
                $"INSERT INTO {getTableName(person)} " +
                "(name, person_date, person_group, base_salary ) " +
                "values(@name, @person_date, @person_group, @base_salary)";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                int result = cnn.Execute(addPersonRequest, person);
                return result;
            }
        }

        public static int UpdatePerson(PersonMainModel person)
        {
            string updetePersonRequest = $"update {getTableName(person)} " +
                                         "set name = @name, person_date = @person_date, person_group = @person_group, base_salary = @base_salary " +
                                         "where id = @id";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var result = cnn.Execute(updetePersonRequest,
                    new {person.name, person.person_date, person.person_group, person.base_salary, person.id});
                return result;
            }
        }

        public static int DeletePerson(PersonMainModel person)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var result =
                    cnn.Execute(
                        $"delete from {getTableName(person)} where id = @Id and name = @name and person_group = @person_group",
                        new {person.id, person.name, person.person_group});
                return result;
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        private static string getTableName(PersonMainModel person)
        {
            string tableName = null;

            if (person.person_group.ToLower().Equals("employees"))
                tableName = "employees";

            else if (person.person_group.ToLower().Equals("managers"))
                tableName = "managers";
            else
                tableName = "salesmans";

            return tableName;
        }

    }
}
