using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int OpeningMonth { get; set; }

        public int ClosingMonth { get; set; }

        public int MaxOccupancy { get; set; }

        public bool WheelchairAccessible { get; set; }

        public decimal DailyRate { get; set; }

        public DateTime OpeningDate
        {
            get
            {
                return new DateTime(DateTime.Now.Year, OpeningMonth, 01);
            }
        }

        public DateTime ClosingDate
        {
            get
            {
                return new DateTime(DateTime.Now.Year, ClosingMonth, 1).AddMonths(1).AddDays(-1);
            }
        }
    }
}
