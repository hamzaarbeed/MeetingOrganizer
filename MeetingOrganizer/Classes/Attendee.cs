using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingOrganizer
{
    internal class Attendee
    {
        public string name { get; private set; }//name of this attendee
        public Availability availability { get; private set; }//time ranges that this attendee is available in
        public string email { get; private set; }//email of this attendee
        public Guid id { get; private set; }//unique id for this attendee
    }
}
