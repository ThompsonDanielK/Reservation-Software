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
                            ListVenuesMethods();
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

        /// <summary>
        /// Contains methods that list venues and displays a menu.
        /// </summary>
        private void ListVenuesMethods()
        {
            bool loopOnOff = true;
            while (loopOnOff)
            {

                List<Venue> venue = ListVenues();
                string input2 = Console.ReadLine();
                bool loopOnOff2 = true;

                while (loopOnOff2)
                {
                    Venue ven = VenueDetails(venue, input2);

                    if (ven.Id != -1 && ven.Id != -2)
                    {

                        loopOnOff2 = VenueDetailsMenu(ven);
                    }
                    else if (ven.Id == -1)
                    {

                        loopOnOff2 = false;
                        loopOnOff = false;
                    }
                }
            }
        }

        /// <summary>
        /// Loops through venues and display them to Console.
        /// </summary>
        /// <returns>A list of venues</returns>
        public List<Venue> ListVenues()
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

        /// <summary>
        /// Displays details about selected venue.
        /// </summary>
        /// <param name="venue"></param>
        /// <param name="input"></param>
        /// <returns>The selected venue</returns>
        public Venue VenueDetails(List<Venue> venue, string input)
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

        /// <summary>
        /// Displays a menu and contains a method that allows uses to view spaces within a venue.
        /// </summary>
        /// <param name="venue"></param>
        /// <returns>A bool that turns off ListVenuesMethods while loop if user wants to return to previous screen.</returns>
        public bool VenueDetailsMenu(Venue venue)
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
                            loopOnOff = ViewSpaces(venue);
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
            return true;
        }

        /// <summary>
        /// Contains methods the get spaces and list a menu for the space.
        /// </summary>
        /// <param name="venue"></param>
        /// <returns>A bool that if false, will turn off the loop in VenueDetailsMenu.</returns>
        private bool ViewSpaces(Venue venue)
        {
            bool loopOnOff;
            ICollection<Space> spaceCollection = spaceDAO.GetSpaces(venue);
            GetSpaceHelper(spaceCollection, venue);
            loopOnOff = ListVenueSpaceMenu(venue);
            return loopOnOff;
        }

        /// <summary>
        /// Loops through spaces in a collection and prints them to console.
        /// </summary>
        /// <param name="spaceCollection"></param>
        /// <param name="venue"></param>
        public void GetSpaceHelper(ICollection<Space> spaceCollection, Venue venue)
        {

            if (spaceCollection.Count > 0)
            {
                Console.Clear();
                Console.WriteLine(venue.Name + " Spaces");
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

        /// <summary>
        /// Space Menu, allows for user input and contains a method to reserve a space.
        /// </summary>
        /// <param name="venue"></param>
        /// <returns>A bool that if false, will turn off the loop in the ViewSpaces method.</returns>
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
                    ReserveASpace(venue);
                    break;
                case "r":
                    return false;
                case "R":
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Contains methods that take in user input and lists spaces avaible to be reserved.
        /// </summary>
        /// <param name="venue"></param>
        public void ReserveASpace(Venue venue)
        {
            bool loopOnOff = true;

            while (loopOnOff)
            {
                try
                {
                    Console.WriteLine();                    

                    DateTime date = GetDate();
                    int howManyDays = GetHowManyDays();
                    int attendees = GetHowManyAttendees();

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

                        loopOnOff = GetSpaceNumber(venue, loopOnOff, reservationCollection);
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid input: " + ex.Message);
                }
            }

        }

        /// <summary>
        /// Gets a space number from user and contains a method to make a reservation.
        /// </summary>
        /// <param name="venue"></param>
        /// <param name="loopOnOff"></param>
        /// <param name="reservationCollection"></param>
        /// <returns>A bool that if false, will turn off the loop in the ReserveASpace method.</returns>
        private bool GetSpaceNumber(Venue venue, bool loopOnOff, ICollection<Reservation> reservationCollection)
        {
            bool loopOnOff5 = true;
            while (loopOnOff5)
            {
                Console.WriteLine();
                Console.Write("Which space would you like to reserve (enter 0 to cancel)? ");
                int spaceNumber = Convert.ToInt32(Console.ReadLine());
                if (spaceNumber == 0)
                {
                    loopOnOff5 = false;

                }
                else
                {
                    int count = 0;
                    foreach (Reservation reservation in reservationCollection)
                    {
                        if (reservation.SpaceId == spaceNumber)
                        {
                            count++;
                        }
                    }
                    if (count < 1)
                    {
                        Console.WriteLine("Invalid Selection");
                    }
                    else
                    {
                        loopOnOff = MakeReservationHelper(spaceNumber, reservationCollection, venue);
                        loopOnOff5 = false;

                    }
                }
            }

            return loopOnOff;
        }

        /// <summary>
        /// Gets amount of attendees from user and checks to see if its valid.
        /// </summary>
        /// <returns>The amount of attendees</returns>
        public int GetHowManyAttendees()
        {
            int attendees = -1;
            bool loopOnOff4 = true;
            while (loopOnOff4)
            {
                Console.Write("How many people will be in attendance? ");
                attendees = Convert.ToInt32(Console.ReadLine());

                if (attendees < 1)
                {
                    Console.WriteLine("You cannot make reservations for less than 1 person. Please input a vaild amount of people attending.");
                    Console.WriteLine();
                }
                else
                {
                    loopOnOff4 = false;
                }
            }

            return attendees;
        }

        /// <summary>
        /// Gets amount of days from user and checks if its valid.
        /// </summary>
        /// <returns>The amount of days</returns>
        public int GetHowManyDays()
        {
            int howManyDays = -1;
            bool loopOnOff3 = true;
            while (loopOnOff3)
            {
                Console.Write("How many days will you need the space? ");
                howManyDays = Convert.ToInt32(Console.ReadLine());

                if (howManyDays < 1)
                {
                    Console.WriteLine("You cannot make reservations for less than 1 day. Please input a vaild amount of days.");
                    Console.WriteLine();
                }
                else
                {
                    loopOnOff3 = false;
                }
            }

            return howManyDays;
        }

        /// <summary>
        /// Gets the date from user and checks if its valid.
        /// </summary>
        /// <returns>Date of potential reservation</returns>
        public DateTime GetDate()
        {
            DateTime date = DateTime.Now;
            bool loopOnOff2 = true;
            while (loopOnOff2)
            {
                Console.Write("When do you need the space? ");
                date = Convert.ToDateTime(Console.ReadLine());

                if (date < DateTime.Now)
                {
                    Console.WriteLine("You cannot make reservations on dates in the past. Please input a valid date.");
                    Console.WriteLine();
                }
                else
                {
                    loopOnOff2 = false;
                }
            }

            return date;
        }

        /// <summary>
        /// Loops through reservations in a collection to see if any reservation match the space number passed through in the parameters
        /// </summary>
        /// <param name="spaceNumber"></param>
        /// <param name="reservationCollection"></param>
        /// <param name="venue"></param>
        /// <returns>A bool that if false, will end the first loop in GetSpaceNumber method</returns>
        public bool MakeReservationHelper(int spaceNumber, ICollection<Reservation> reservationCollection, Venue venue)
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
            return ReservationConfirmationDetails(venue, reservation, name, confirmationNumber);
        }

        /// <summary>
        /// Confirms submission of reservation and lists reservation details
        /// </summary>
        /// <param name="venue"></param>
        /// <param name="reservation"></param>
        /// <param name="name"></param>
        /// <param name="confirmationNumber"></param>
        /// <returns>A bool that is passed returned to the MakeReservationHelper method. If false, will end the first loop in GetSpaceNumber method</returns>
        public bool ReservationConfirmationDetails(Venue venue, Reservation reservation, string name, int confirmationNumber)
        {
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
            Console.WriteLine();
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            return false;
        }
    }
}
