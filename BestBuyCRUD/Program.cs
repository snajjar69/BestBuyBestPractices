using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestBuyCRUD
{
    class Program
    {
        static void Main(string[] args)
        {
            var departments = GetAllDepartments();

            foreach (var dept in departments)
            {
                Console.WriteLine($"{dept.DepartmentID} {dept.Name}");
            }

            var response = "";

            do
            {
                Console.WriteLine("Would you like to update a  Department?");
                Console.WriteLine("\n     Type Y for yes \n     or type EXIT to exit the program");
                response = Console.ReadLine().ToUpper();

                if (response == "Y")
                {
                    Console.WriteLine("What is the name of the department you want to update?");
                    var departmentName = Console.ReadLine();

                    Console.WriteLine("What is the new name of the department?");

                    var newName = Console.ReadLine();
                    UpdateDepartment(departmentName, newName);
                }

                var depts = GetAllDepartments();

                Console.WriteLine($"----------------------"); 
                Console.WriteLine($"Here is the updated list of Departments");
                foreach (var item in depts)
                {
                    Console.WriteLine($"{item}"); ;
                }

            } while (response != "EXIT");

        }

        static IEnumerable<Department> GetAllDepartments()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("ConnectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Departments;";

            using (conn)
            {
                conn.Open();
                List<Department> allDepartments = new List<Department>();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    var currentDepartment = new Department();

                    currentDepartment.DepartmentID = reader.GetInt32("DepartmentID");

                    currentDepartment.Name = reader.GetString("Name");

                    allDepartments.Add(currentDepartment);
                }

                return allDepartments;
            }
        }

        static void CreateDepartment(string departmentName)
        {
            var connStr = System.IO.File.ReadAllText("ConnectionString.txt");

            //If you adopt initializing the connection inside the using statement then you can't make a mistake
            //later when reorganizing or refactoring code and accidentally doing something that implicitly
            //opens a connection that isn't closed
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                // parameterized query to prevent SQL Injection
                cmd.CommandText = "INSERT INTO departments (Name) VALUES (@departmentName);";
                cmd.Parameters.AddWithValue("departmentName", departmentName);

                cmd.ExecuteNonQuery();
            }
        }

        static void UpdateDepartment(string currentName, string newDepartmentName)
        {
            var connStr = System.IO.File.ReadAllText("ConnectionString.txt");

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                // parameterized query to prevent SQL Injection
                cmd.CommandText = "Update departments SET Name = @newDepartmentName WHERE Name = @currentName;";

                cmd.Parameters.AddWithValue("newDepartmentName", newDepartmentName);
                cmd.Parameters.AddWithValue("currentName", currentName);
                

                cmd.ExecuteNonQuery();
            }
        }

    }
}
