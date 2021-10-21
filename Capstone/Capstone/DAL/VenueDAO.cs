using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO
    {
        private readonly string connectionString;

        private const string SqlSelectAll = "SELECT * FROM dbo.venue ORDER BY name";

        private const string SqlGetLocation = "Select c.name + ', ' + s.abbreviation AS citystate FROM city c " +
            "INNER JOIN venue v ON v.id = c.id INNER JOIN state s ON s.abbreviation = c.state_abbreviation " +
            "WHERE v.city_id = @cityid AND v.id = @venueId";

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ICollection<Venue> GetVenue()
        {
            List<Venue> results = new List<Venue>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = new Venue
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            CityId = Convert.ToInt32(reader["city_id"]),
                            Description = Convert.ToString(reader["description"])
                        };

                        results.Add(venue);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not query the database: " + ex.Message);
            }

            return results;
        }

        public Venue SelectVenue(int input)
        {
            List<Venue> results = new List<Venue>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAll, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = new Venue
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = Convert.ToString(reader["name"]),
                            CityId = Convert.ToInt32(reader["city_id"]),
                            Description = Convert.ToString(reader["description"])
                        };

                        results.Add(venue);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not query the database: " + ex.Message);
            }

            return results[input - 1];
        }

        public string GetLocation(Venue ven)
        {
            string result = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetLocation, conn);
                    command.Parameters.AddWithValue("@cityId", ven.CityId);
                    command.Parameters.AddWithValue("@venueId", ven.Id);

                    result = (string)command.ExecuteScalar();
                    return result;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not query the database: " + ex.Message);
            }
            return result;
        }
    }
}
