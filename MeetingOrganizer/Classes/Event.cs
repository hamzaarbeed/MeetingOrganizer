using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingOrganizer
{
    internal class Event
    {
        TimeSpan duration { get; set; }//length of the event itself
        DateTimeRange availableTimeRange { get; set; }//time range that the event can occur
        string name { get; set; }//name of event
        DateTime chosenTimeslot { get; set; }//slot chosen by the user for the event to occur on
        public List<Attendee> attendees { get; private set; }//list of attendees
        Timeslots timeslots { get; set; }//stores data on timeslots based on attendees; assumes timeslots will be in increments of 15 minutes

        //create timeslots
        void MakeTimeslots()
        {
            timeslots = new Timeslots(availableTimeRange, duration, attendees);
        }

        
        //public void AddAttendee(Attendee) { }
        //public void DeleteAttendee(Attendee) { }
    }
}
