using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingOrganizer
{
    internal class Attendee
    {
        public Attendee() {
            availabilities = new List<DateTimeRange>();
        }
        public string name { get; set; }//name of this attendee
        public List<DateTimeRange> availabilities { get; set; }//time ranges that this attendee is available in
        public string email { get; set; }//email of this attendee

        public Attendee deepCopy(){
            Attendee newAttendee = new Attendee();
            newAttendee.name = name;
            newAttendee.email = email;
            foreach (DateTimeRange range in availabilities) {
                newAttendee.availabilities.Add(range.deepCopy());
            }
            return newAttendee;
        }
    }
}
