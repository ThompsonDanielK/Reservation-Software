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

        public decimal DailyRate { get; set; }

       
    }
}
