using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingOrganizer
{
    internal class Timeslots
    {
        public List<HashSet<Guid>> slotConflictsList { get; private set; }

        /// <summary>
        /// Constructor that creates a list of attendee time conflicts for a Timeslots object. 
        /// </summary>
        /// <param name="eventRange">time range the event can occur</param>
        /// <param name="duration">length of event</param>
        /// <param name="attendees">list of people who will attend</param>
        public Timeslots(DateTimeRange eventRange, TimeSpan duration, List<Attendee> attendees)
        {
            int slots=(int)((eventRange.end-eventRange.start).TotalMinutes)/15;
            bool conflict;
            DateTime currentTime = eventRange.start;
            TimeSpan slotLength = TimeSpan.FromMinutes(15);
            slotConflictsList = new List<HashSet<Guid>>();

            //initialize the conflict list
            for (int i=0; i<slots; i++)
            {
                slotConflictsList.Add(new HashSet<Guid>());
            }

            //for each slot, check if each attendee has a conflict. If an attendee has a conflict, add their Guid to that timeslot
            for (int slotNumber=0;slotNumber<slots; slotNumber++)
            {
                foreach (Attendee at in attendees)
                {
                    conflict = true;
                    foreach (DateTimeRange attendeeRange in at.availability.timesAvailable)
                    {
                        //if attendee can attend, they have no conflict
                        if (attendeeRange.start<=currentTime && currentTime+duration<=attendeeRange.end)
                            conflict = false;
                    }
                    //add Guid if attendee cannot attend
                    if (!conflict)
                        slotConflictsList[slotNumber].Add(at.id);
                }
                currentTime+=slotLength;//add 15 minutes to the time to match the timeslot's time
            }
            
        }
    }
}
