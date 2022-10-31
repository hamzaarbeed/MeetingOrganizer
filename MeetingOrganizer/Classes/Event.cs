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
        Timeslots timeslots { get; set; }//stores data on timeslots based on attendees

        //create timeslots
        void MakeTimeslots()
        {
            timeslots = new Timeslots(availableTimeRange, duration, attendees);
        }

        /// <summary>
        /// Shows a list of timeslots with the least conflicts along with the attendees' Guid that can't attend
        /// </summary>
        /// <returns>list of pairs of timeslot (as DateTime) and attendee IDs (HashSet<Guid>) who can't make it</returns>
        List<Tuple<DateTime,HashSet<Guid>>> BestTimeslots()
        {
            DateTime start = availableTimeRange.start;
            TimeSpan slotLength = TimeSpan.FromMinutes(15);
            List<Tuple<DateTime, HashSet<Guid>>> bestTimeslots=new();
            DateTime slot;
            HashSet<Guid> attendeesUnableToAttend;
            
            //*optimization* might be faster to just iterate through timeslots and get the minimum number of conflicts
            for (int numConflicts = 0; numConflicts < attendees.Count; numConflicts++)//iterate numConflicts
            {
                for (int slotIndex = 0; slotIndex < timeslots.slotConflictsList.Count; slotIndex++)//iterate timeslots
                {
                    //if the number of conflicts in the timeslot is lower than the current number of conflicts,
                    //add that timeslot to the result
                    if (timeslots.slotConflictsList[slotIndex].Count<=numConflicts)
                    {
                        slot=start+(slotIndex*slotLength);
                        attendeesUnableToAttend=timeslots.slotConflictsList[slotIndex];
                        bestTimeslots.Add(Tuple.Create(slot, attendeesUnableToAttend));

                    }
                }
                if (bestTimeslots.Count>0)
                    return bestTimeslots;
            }
            return bestTimeslots;
        }
        //public void AddAttendee(Attendee) { }
        //public void DeleteAttendee(Attendee) { }
    }
}
