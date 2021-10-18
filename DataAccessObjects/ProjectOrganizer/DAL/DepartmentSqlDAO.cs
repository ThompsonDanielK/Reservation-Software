using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private readonly string connectionString;

        private const string SqlSelectAll = "SELECT * FROM dbo.department";

        private const string SqlCreateDepartment = "INSERT INTO dbo.department (name) VALUES (@name); SELECT @@IDENTITY";

        private const string SqlUpdateDepartment = "UPDATE dbo.department SET name = @newName WHERE department_id = @department_id";

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public ICollection<Department> GetDepartments()
        {
            List<Department> results = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Department department = new Department
                        {
                            Id = Convert.ToInt32(reader["department_id"]),
                            Name = Convert.ToString(reader["name"])
                        };

                        results.Add(department);
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
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlCreateDepartment, conn);
                    command.Parameters.AddWithValue("@name", newDepartment.Name);

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    return id;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not create department: " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlUpdateDepartment, conn);
                    command.Parameters.AddWithValue("@newName", updatedDepartment.Name);
                    command.Parameters.AddWithValue("@department_id", updatedDepartment.Id);

                    command.ExecuteNonQuery();

                    return true;                   
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not update department: " + ex.Message);
                return false;
            }
        }

    }
}
