using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string OpeningMonth { get; set; }

        public string ClosingMonth { get; set; }

        public int MaxOccupancy { get; set; }

        public bool WheelchairAccessible { get; set; }

        public decimal DailyRate { get; set; }
    }
}
