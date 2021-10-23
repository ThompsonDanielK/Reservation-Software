using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO
    {
        private readonly string connectionString;

        private const string SqlMakeReservation = "SELECT TOP 5 s.id, s.name, s.daily_rate, s.max_occupancy, " +
            "s.is_accessible, s.daily_rate * @days AS totalcost " +
            "FROM space s " +
            "WHERE NOT EXISTS (SELECT * FROM reservation " +
            "WHERE start_date BETWEEN @startdate AND DATEADD(day, @days, @startdate) " +
            "AND end_date BETWEEN @startdate AND DATEADD(day, @days, @startdate) " +
            "AND s.venue_id = @venueid)" +
            " AND ((MONTH(@startdate) > s.open_from " +
            "AND MONTH(@startdate) < s.open_to) OR s.open_from IS NULL) " +
            "AND s.venue_id = @venueid " +
            "GROUP BY s.id, s.name, s.daily_rate, s.max_occupancy, s.is_accessible, s.daily_rate * @days";

        private const string SqlInsertReservation = "INSERT INTO reservation " +
            "(space_id, number_of_attendees, start_date, end_date, reserved_for) " +
            "VALUES (@spaceid, @numberofattendees, @startdate, @enddate, @reservedfor); " +
            "SELECT reservation_id " +
            "FROM reservation " +
            "WHERE start_date = @startdate " +
            "AND end_date = @enddate " +
            "AND reserved_for = @reservedfor " +
            "AND space_id = @spaceid " +
            "AND number_of_attendees = @numberofattendees";

        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ICollection<Reservation> ReserveASpace(DateTime date, int days, int attendees, Venue venue)
        {
            List<Reservation> results = new List<Reservation>();

            try
            {

                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlMakeReservation, conn);
                    command.Parameters.AddWithValue("@startdate", Convert.ToDateTime(date));
                    command.Parameters.AddWithValue("@days", days);
                    command.Parameters.AddWithValue("@venueid", venue.Id);

                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation()
                        {                          
                            SpaceName = Convert.ToString(reader["name"]),
                            SpaceId = Convert.ToInt32(reader["id"]),                            
                            NumberOfAttendees = attendees,
                            StartDate = Convert.ToDateTime(date),
                            EndDate = Convert.ToDateTime(date.AddDays(days)),
                            DailyCost = Convert.ToDecimal(reader["daily_rate"]),
                            TotalCost = Convert.ToDecimal(reader["totalcost"]),
                            MaxOccup = Convert.ToInt32(reader["max_occupancy"]),
                            accessible = Convert.ToBoolean(reader["is_accessible"])

                        };

                        results.Add(reservation);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not query the database: " + ex.Message);
            }

            return results;
        }

        public int MakeReservation(Reservation reservation, string name)
        {          

            try
            {
                

                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlInsertReservation, conn);
                    command.Parameters.AddWithValue("@spaceid", reservation.SpaceId);
                    command.Parameters.AddWithValue("@numberofattendees", reservation.NumberOfAttendees);
                    command.Parameters.AddWithValue("@startdate", reservation.StartDate);
                    command.Parameters.AddWithValue("@enddate", reservation.EndDate);
                    command.Parameters.AddWithValue("@reservedfor", name);

                    int confirmationNumber = Convert.ToInt32(command.ExecuteScalar());
                    return confirmationNumber;

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not query the database: " + ex.Message);
            }


            return 0;
        }
    }
}
