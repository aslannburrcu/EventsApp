using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BurcuAslan_Events.Models
{
    public class ViewModel
    {
        public IEnumerable<Tickets> ticket { get; set; }
        public IEnumerable<Events> events { get; set; }
        public IEnumerable<Event_categories> event_categories { get; set; }
        public IEnumerable<Cities> cities { get; set; }

    }
}