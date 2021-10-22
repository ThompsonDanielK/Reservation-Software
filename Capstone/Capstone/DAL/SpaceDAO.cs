using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SpaceDAO
    {
        private readonly string connectionString;

        private const string SqlGetSpace = "";

        public SpaceDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ICollection<Space> GetSpaces(Venue ven)
        {
            List<Space> results = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetSpace, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        

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
