using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// This class is responsible for representing the main user interface to the user.
    /// </summary>
    /// <remarks>
    /// ALL Console.ReadLine and WriteLine in this class
    /// NONE in any other class. 
    ///  
    /// The only exceptions to this are:
    /// 1. Error handling in catch blocks
    /// 2. Input helper methods in the CLIHelper.cs file
    /// 3. Things your instructor explicitly says are fine
    /// 
    /// No database calls should exist in classes outside of DAO objects
    /// </remarks>
    public class UserInterface
    {
        private readonly string connectionString;

        private readonly VenueDAO venueDAO;

        private readonly SpaceDAO spaceDAO;

        private readonly ReservationDAO reservationDAO;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
            spaceDAO = new SpaceDAO(connectionString);
            reservationDAO = new ReservationDAO(connectionString);
        }

        public void Run()
        {
            bool quit = false;

            while (!quit)
            {
                Console.Clear();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1) List Venues");
                Console.WriteLine("Q) Quit");

                try
                {
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            bool loopOnOff = true;
                            while (loopOnOff)
                            {

                                List<Venue> venue = GetVenueHelper();
                                string input2 = Console.ReadLine();


                                bool loopOnOff2 = true;

                                while (loopOnOff2)
                                {
                                    Venue ven = SelectVenueHelper(venue, input2);

                                    if (ven.Id != -1 && ven.Id != -2)
                                    {


                                        loopOnOff2 = VenueDetails(ven);


                                    }
                                    else if (ven.Id == -1)
                                    {
                                        loopOnOff2 = false;

                                        loopOnOff = false;

                                    }
                                }


                            }
                            break;

                        case ("Q"):
                            quit = true;
                            break;

                        case ("q"):
                            quit = true;
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid selection: " + ex.Message);
                }


            }
        }
        public List<Venue> GetVenueHelper()
        {
            List<Venue> venue = venueDAO.GetVenue();

            if (venue.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("Which Venue would you like to view?");

                int indexPlusOne = 1;
                foreach (Venue ven in venue)
                {
                    Console.WriteLine($"{indexPlusOne}) {ven.Name}");
                    indexPlusOne++;
                }

                Console.WriteLine("R) Return to Previous Screen");
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }

            return venue;
        }

        public Venue SelectVenueHelper(List<Venue> venue, string input)
        {
            Venue ven = new Venue();
            try
            {
                if (input.ToUpper() == "R")
                {
                    return ven;
                }
                else if (Convert.ToInt32(input) > venue.Count)
                {
                    Console.WriteLine("That is not a valid selection");
                }

                ven = venueDAO.SelectVenue(Convert.ToInt32(input), venue);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid selection: " + ex.Message);
            }
            if (ven.Id != -1)
            {
                Console.Clear();
                Console.WriteLine(ven.Name);
                Console.WriteLine("Location: " + ven.City + ", " + ven.State);
                Console.WriteLine("Categories: " + ven.Category);
                Console.WriteLine();
                Console.WriteLine(ven.Description);
                return ven;
            }
            ven.Id = -2;
            return ven;
        }


        public bool VenueDetails(Venue venue)
        {
            if (venue.Id != -1)
            {
                Console.WriteLine();
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("1) View Spaces");
                Console.WriteLine("R) Return to Previous Screen");

                string input = Console.ReadLine();
                bool loopOnOff = true;

                while (loopOnOff)
                {
                    switch (input)
                    {
                        case "1":
                            //View Spaces
                            ICollection<Space> spaceCollection = spaceDAO.GetSpaces(venue);
                            GetSpaceHelper(spaceCollection, venue);
                            loopOnOff = ListVenueSpaceMenu(venue);
                            break;

                        case "r":
                            loopOnOff = false;
                            return false;

                        case "R":
                            loopOnOff = false;
                            return false;
                    }
                }
            }
            return false;
        }

        public void GetSpaceHelper(ICollection<Space> spaceCollection, Venue venue)
        {

            if (spaceCollection.Count > 0)
            {
                Console.Clear();
                Console.WriteLine(venue.Name);
                Console.WriteLine();
                Console.WriteLine($"{" ",-6}{"Name",-25}{"Open",-8}{"Close",-8}{"Daily Rate",-15}{"Max. Occupancy",-15}");

                foreach (Space space in spaceCollection)
                {
                    Console.WriteLine($"#{space.Id,-5}{space.Name,-25}{space.OpeningMonth,-8}{space.ClosingMonth,-8}{space.DailyRate.ToString("C"),-15}{space.MaxOccupancy,-15}");
                }

            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        public bool ListVenueSpaceMenu(Venue venue)
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) Reserve a Space");
            Console.WriteLine("R) Return to Previous Screen");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    //Reserve a space
                    ReserveASpaceMenu(venue);
                    break;
                case "r":
                    return false;
                case "R":
                    return false;
            }
            return true;
        }

        public void ReserveASpaceMenu(Venue venue)
        {
            bool loopOnOff = true;

            while (loopOnOff)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("When do you need the space? ");
                    DateTime date = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("How many days will you need the space? ");
                    int howManyDays = Convert.ToInt32(Console.ReadLine());
                    Console.Write("How many people will be in attendance? ");
                    int attendees = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    ICollection<Reservation> reservationCollection = reservationDAO.ReserveASpace(date, howManyDays, attendees, venue);

                    if (reservationCollection.Count < 1)
                    {
                        Console.WriteLine("No reservations available, would you like to try again? (Y/N)");
                        string input = Console.ReadLine().ToUpper();

                        if (input == "N")
                        {
                            return;
                        }

                    }
                    else
                    {
                        loopOnOff = false;
                        Console.Clear();
                        Console.WriteLine("The following spaces are available based on your needs:");
                        Console.WriteLine(String.Format($"{"Space #",-10}{"Name",-25}{"Daily Rate",-15}{"Max Occup.",-15}{"Accessible?",-15}{"Total Cost",-15}"));
                        foreach (Reservation reservation in reservationCollection)
                        {
                            Console.WriteLine($"{reservation.SpaceId,-10}{reservation.SpaceName,-25}{reservation.DailyCost.ToString("C"),-15}{reservation.MaxOccup,-15}{reservation.Accessible,-15}{reservation.TotalCost.ToString("C"),-15}");

                        }

                        Console.WriteLine();
                        Console.Write("Which space would you like to reserve? (enter 0 to cancel)? ");
                        int spaceNumber = Convert.ToInt32(Console.ReadLine());
                        if (spaceNumber == 0)
                        {
                            return;
                        }
                        else
                        {
                            MakeReservationHelper(spaceNumber, reservationCollection, venue);
                        }
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid input: " + ex.Message);
                }
            }

        }


        public void MakeReservationHelper(int spaceNumber, ICollection<Reservation> reservationCollection, Venue venue)
        {
            Reservation reservation = new Reservation();

            foreach (Reservation reservation1 in reservationCollection)
            {
                if (reservation1.SpaceId == spaceNumber)
                {
                    reservation = reservation1;
                }

            }

            Console.Write("Who is this reservation for? ");
            string name = Console.ReadLine();
            Console.WriteLine();

            int confirmationNumber = reservationDAO.MakeReservation(reservation, name);

            Console.WriteLine("Thanks for submitting your reservation! The details for your event are listed below:");
            Console.WriteLine();
            Console.WriteLine("Confirmation #: " + confirmationNumber);
            Console.WriteLine("Venue: " + venue.Name);
            Console.WriteLine("Space: " + reservation.SpaceName);
            Console.WriteLine("Reserved For: " + name);
            Console.WriteLine("Attendees: " + reservation.NumberOfAttendees);
            Console.WriteLine("Arrival Date: " + reservation.StartDate.ToString("MM/dd/yyyy"));
            Console.WriteLine("Depart Date: " + reservation.EndDate.ToString("MM/dd/yyyy"));
            Console.WriteLine("Total Cost: " + reservation.TotalCost.ToString("C"));
        }
    }
}
