using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO
    {
        private readonly string connectionString;

        private const string SqlSelectAll = "SELECT v.name, v.id, v.city_id, v.description, " +
            "c.name AS cityname, s.abbreviation AS state, ca.name AS categoryname " +
            "FROM venue v INNER JOIN city c ON v.city_id = c.id " +
            "INNER JOIN state s ON s.abbreviation = c.state_abbreviation " +
            "FULL JOIN category_venue cv ON cv.venue_id = v.id " +
            "FULL JOIN category ca ON ca.id = cv.category_id " +
            "ORDER BY v.name";


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
                        bool skip = false;
                        foreach (Venue venue in results)
                        {
                            if (venue.Name == (Convert.ToString(reader["name"])))
                            {
                                venue.Category = venue.Category + ", " + Convert.ToString(reader["categoryname"]);
                                skip = true;
                            }
                            else
                            {
                                skip = false;
                            }
                        }
                        if (!skip)
                        {
                            Venue newVenue = new Venue
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = Convert.ToString(reader["name"]),
                                CityId = Convert.ToInt32(reader["city_id"]),
                                Description = Convert.ToString(reader["description"]),
                                City = Convert.ToString(reader["cityname"]),
                                State = Convert.ToString(reader["state"]),
                                Category = Convert.ToString(reader["categoryname"])
                            };
                            results.Add(newVenue);
                        }

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

            results = (List<Venue>)GetVenue();

            try
            {
                return results[input - 1];
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Please select a listed value: " + ex.Message);
            }
            Venue emptyVenue = new Venue();
            return emptyVenue;
        }
    }
}
