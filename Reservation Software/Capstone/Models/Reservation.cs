using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public bool accessible;

        public int SpaceId { get; set; }

        public string SpaceName { get; set; }

        public int NumberOfAttendees { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    
        public decimal DailyCost { get; set; }

        public decimal TotalCost { get; set; }

        public int MaxOccup { get; set; }

        public string Accessible
        {
            get
            {
                if (accessible)
                {
                    return "Yes";
                }

                return "No";
            }
        }



    }
}
