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
            //*Possible optimization* The end of the event range should have every attendee in conflict. Maybe we should cut it off?
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
        /// <summary>
        /// Returns a list of timeslots with the least conflicts along with the attendees' Guid that can't attend
        /// </summary>
        /// <returns>list of pairs containing timeslot (as DateTime) and attendee IDs (HashSet<Guid>) who can't make it</returns>
        List<Tuple<DateTime, HashSet<Guid>>> BestTimeslots(DateTime start)
        {
            TimeSpan slotLength = TimeSpan.FromMinutes(15);
            int numConflicts = int.MaxValue;//stores the minimum number of conflicts found so far when iterating; make this as high as possible so almost everything is lower
            DateTime slot;//store the time of the slot
            HashSet<Guid> attendeesUnableToAttend;//store the attendees that can't attend
            List<Tuple<DateTime, HashSet<Guid>>> bestTimeslots = new();//what we will return

            //go through conflicts, keep track of minimum conflicts and a list that has that number of conflicts, reset list if a lower # conflicts encountered
            for (int slotIndex = 0; slotIndex < slotConflictsList.Count; slotIndex++)//iterate timeslots
            {
                if (slotConflictsList[slotIndex].Count;<numConflicts)
                {
                    numConflicts=slotConflictsList[slotIndex].Count;
                    bestTimeslots=new();
                }
                //if the number of conflicts in the timeslot is lower than the current number of conflicts,
                //add that timeslot to the result
                if (slotConflictsList[slotIndex].Count==numConflicts)
                {
                    slot=start+(slotIndex*slotLength);
                    attendeesUnableToAttend=slotConflictsList[slotIndex];
                    bestTimeslots.Add(Tuple.Create(slot, attendeesUnableToAttend));

                }
            }
            return bestTimeslots;
        }
    }
}
