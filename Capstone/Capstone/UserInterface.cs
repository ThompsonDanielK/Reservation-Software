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

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
            spaceDAO = new SpaceDAO(connectionString);
        }

        public void Run()
        {
            bool quit = false;

            while (!quit)
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1) List Venues");
                Console.WriteLine("Q) Quit");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        bool loopOnOff = true;
                        while (loopOnOff)
                        {
                            ICollection<Venue> venue = GetVenueHelper();
                            Venue ven = SelectVenueHelper(venue);

                            if (ven.Name != "" && ven.Name != "A")
                            {
                                while (true)
                                {
                                    VenueDetails(ven);
                                    break;
                                }
                            }
                            else if (ven.Name == "")
                            {
                                loopOnOff = false;
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
        }
        public ICollection<Venue> GetVenueHelper()
        {
            ICollection<Venue> venue = venueDAO.GetVenue();

            if (venue.Count > 0)
            {
                Console.WriteLine();
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

        public Venue SelectVenueHelper(ICollection<Venue> venue)
        {

            string input = Console.ReadLine();

            Venue ven = new Venue();

            if (input.ToUpper() == "R")
            {
                return ven;
            }
            else if (Convert.ToInt32(input) > venue.Count)
            {
                Console.WriteLine("That is not a valid selection");
            }

            ven = venueDAO.SelectVenue(Convert.ToInt32(input));

            if (ven.Name != "")
            {
                Console.WriteLine();
                Console.WriteLine(ven.Name);
                Console.WriteLine("Location: " + ven.City + ", " + ven.State);
                Console.WriteLine("Categories: " + ven.Category);
                Console.WriteLine();
                Console.WriteLine(ven.Description);
                return ven;
            }
            ven.Name = "A";
            return ven;
        }


        public void VenueDetails(Venue venue)
        {
            if (venue.Name != "")
            {
                Console.WriteLine();
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("1) View Spaces");
                Console.WriteLine("2) Search for Reservation");
                Console.WriteLine("R) Return to Previous Screen");

                string input = Console.ReadLine();
                bool loopOnOff = true;

                while (loopOnOff)
                {
                    switch (input)
                    {
                        case "1":
                            //View Spaces
                            ICollection<Space> space = spaceDAO.GetSpaces(venue);
                            break;

                        case "2":
                            //Search for Reservation
                            break;

                        case "r":
                            loopOnOff = false;
                            break;

                        case "R":
                            loopOnOff = false;
                            break;
                    }
                }
            }
            return;
        }
    }
}
