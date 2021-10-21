using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public string Description { get; set; }
        
        public string City { get; set; }

        public string State { get; set; }

        public string Category { get; set; }

        public int GetId()
        {
            return Id;
        }
    }
}
