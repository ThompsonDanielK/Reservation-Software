using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class is responsible for querying the database and working with Venues
    /// </summary>
    public class SpaceDAO
    {
        private readonly string connectionString;

        private const string SqlGetSpace = "SELECT * FROM space s INNER JOIN venue v ON v.id = s.venue_id WHERE v.id = @venueid";

        public SpaceDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Queries the database and builds a list of spaces.
        /// </summary>
        /// <param name="ven"></param>
        /// <returns>Returns a collection of spaces.</returns>
        public ICollection<Space> GetSpaces(Venue ven)
        {
            List<Space> results = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetSpace, conn);
                    command.Parameters.AddWithValue("@venueid", ven.Id);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = new Space()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
                            DailyRate = Convert.ToDecimal(reader["daily_rate"])
                        };
                        DateNullCheck(reader, space);

                        results.Add(space);
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
        /// Sets openingMonth and closingMonth to an empty string if null
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="space"></param>
        private void DateNullCheck(SqlDataReader reader, Space space)
        {
            if (!DBNull.Value.Equals(reader["open_from"]))
            {
                space.openingMonth = Convert.ToInt32(reader["open_from"]);
            }
            else
            {
                space.openingMonth = 0;
            }

            if (!DBNull.Value.Equals(reader["open_to"]))
            {
                space.closingMonth = Convert.ToInt32(reader["open_to"]);
            }
            else
            {
                space.closingMonth = 0;
            }
        }
    }
}
