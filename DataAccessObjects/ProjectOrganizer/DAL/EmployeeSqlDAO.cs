using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private readonly string connectionString;

        private const string SqlSelectAll = "SELECT * FROM dbo.employee";

        private const string SqlSearchByName = "SELECT * FROM dbo.employee WHERE first_name LIKE @firstname AND last_name LIKE @lastname";

        private const string SqlSearchForNull = "SELECT * FROM dbo.employee e LEFT JOIN dbo.project_employee pe ON pe.employee_id = e.employee_id WHERE pe.project_id IS NULL";

        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public ICollection<Employee> GetAllEmployees()
        {
            List<Employee> results = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            DepartmentId = Convert.ToInt32(reader["employee_id"]),
                            FirstName = Convert.ToString(reader["first_name"]),
                            LastName = Convert.ToString(reader["last_name"]),
                            JobTitle = Convert.ToString(reader["job_title"]),
                            BirthDate = Convert.ToDateTime(reader["birth_date"]),
                            HireDate = Convert.ToDateTime(reader["hire_date"])
                        };

                        results.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not query the database: " + ex.Message);
            }

            return results;
        }

        /// <summary>
        /// Find all employees whose names contain the search strings.
        /// Returned employees names must contain *both* first and last names.
        /// </summary>
        /// <remarks>Be sure to use LIKE for proper search matching.</remarks>
        /// <param name="firstname">The string to search for in the first_name field</param>
        /// <param name="lastname">The string to search for in the last_name field</param>
        /// <returns>A list of employees that matches the search.</returns>
        public ICollection<Employee> Search(string firstname, string lastname)
        {
            List<Employee> results = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSearchByName, conn);
                    command.Parameters.AddWithValue("@firstname", firstname + "%");
                    command.Parameters.AddWithValue("@lastname", lastname + "%");

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            DepartmentId = Convert.ToInt32(reader["employee_id"]),
                            FirstName = Convert.ToString(reader["first_name"]),
                            LastName = Convert.ToString(reader["last_name"]),
                            JobTitle = Convert.ToString(reader["job_title"]),
                            BirthDate = Convert.ToDateTime(reader["birth_date"]),
                            HireDate = Convert.ToDateTime(reader["hire_date"])
                        };

                        results.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not search for employee: " + ex.Message);
            }

            return results;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> results = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSearchForNull, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            EmployeeId = Convert.ToInt32(reader["employee_id"]),
                            DepartmentId = Convert.ToInt32(reader["employee_id"]),
                            FirstName = Convert.ToString(reader["first_name"]),
                            LastName = Convert.ToString(reader["last_name"]),
                            JobTitle = Convert.ToString(reader["job_title"]),
                            BirthDate = Convert.ToDateTime(reader["birth_date"]),
                            HireDate = Convert.ToDateTime(reader["hire_date"])
                        };

                        results.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not query the database: " + ex.Message);
            }

            return results;
        }

    }
}
