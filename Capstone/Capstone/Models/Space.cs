using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int openingMonth;

        public int closingMonth; 

        public int Id { get; set; }

        public string Name { get; set; }

        public int MaxOccupancy { get; set; }

        public decimal DailyRate { get; set; }

        public string OpeningMonth
        {
            get
            {
                if (openingMonth == 0)
                {
                    return "";
                }

                return Convert.ToDateTime(openingMonth.ToString() + "/01/2001").ToString("MMM") + ".";
            }
        }

        public string ClosingMonth
        {
            get
            {
                if (openingMonth == 0)
                {
                    return "";
                }

                return Convert.ToDateTime(closingMonth.ToString() + "/01/2001").ToString("MMM") + ".";
            }
        }


    }
}
