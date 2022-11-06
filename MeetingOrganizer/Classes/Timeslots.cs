using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingOrganizer
{
    //call CalculatConflicts() and BestTimeslots() and the lists will be stored in the static variables
    internal class Timeslots
    {
        public static List<HashSet<int>> slotConflictsList { get; private set; }
        public static List<(DateTime dateAndTime, HashSet<int> indexAbsent)> bestTimeslots { get; private set; }

        /// <summary>
        /// Constructor that creates a list of attendee time conflicts for a Timeslots object. 
        /// </summary>
        /// <param name="eventRange">time range the event can occur</param>
        /// <param name="duration">length of event</param>
        /// <param name="attendees">list of people who will attend</param>
        public static void CalculateConflicts(DateTimeRange eventRange, TimeSpan duration, List<Attendee> attendees)
        {
            int slots = (int)((eventRange.end-eventRange.start).TotalMinutes)/15;
            bool conflict;
            DateTime currentTime = eventRange.start;
            TimeSpan slotLength = TimeSpan.FromMinutes(15);
            slotConflictsList = new List<HashSet<int>>();

            //initialize the conflict list
            for (int i = 0; i<slots; i++)
            {
                slotConflictsList.Add(new HashSet<int>());
            }

            //for each slot, check if each attendee has a conflict. If an attendee has a conflict, add their index to that timeslot
            //*Possible optimization* The end of the event range should have every attendee in conflict. Maybe we should cut it off?
            for (int slotNumber = 0; slotNumber<slots; slotNumber++)
            {
                for (int attendeeIndex=0; attendeeIndex<attendees.Count; attendeeIndex++)
                {
                    conflict = true;
                    foreach (DateTimeRange attendeeRange in attendees[attendeeIndex].availabilities)
                    {
                        //if attendee can attend, they have no conflict
                        if (attendeeRange.start<=currentTime && currentTime+duration<=attendeeRange.end)
                            conflict = false;
                    }
                    //add attendee's index in the list if attendee cannot attend
                    if (conflict)
                        slotConflictsList[slotNumber].Add(attendeeIndex);
                }
                currentTime+=slotLength;//add 15 minutes to the time to match the timeslot's time
            }

        }
        /// <summary>
        /// Returns a list of timeslots with the least conflicts along with the attendees' Guid that can't attend
        /// </summary>
        /// <returns>list of pairs containing timeslot (as DateTime) and attendee indices (HashSet<int>) who can't make it</returns>
        public static List<(DateTime dateAndTime, HashSet<int> indexAbsent)> BestTimeslots(DateTime start)
        {
            TimeSpan slotLength = TimeSpan.FromMinutes(15);
            int numConflicts = int.MaxValue;//stores the minimum number of conflicts found so far when iterating; make this as high as possible so almost everything is lower
            DateTime slot;//store the time of the slot
            HashSet<int> attendeesUnableToAttend;//store the attendees that can't attend
            //List<(DateTime dateAndTime, HashSet<int> indexAbsent)> bestTimeslots = new();//what we will return

            //go through conflicts, keep track of minimum conflicts and a list that has that number of conflicts, reset list if a lower # conflicts encountered
            for (int slotIndex = 0; slotIndex < slotConflictsList.Count; slotIndex++)//iterate timeslots
            {
                if (slotConflictsList[slotIndex].Count<numConflicts)
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
                    bestTimeslots.Add((slot, attendeesUnableToAttend));

                }
            }
            
            return bestTimeslots;//we can change it to only return a list<DateTime> later
        }
        /// <summary>
        /// Takes a timeslot and returns the indices of attendees who cannot attend that timeslot
        /// </summary>
        /// <param name="start">start of the event</param>
        /// <param name="timePicked">the timeslot chosen</param>
        /// <returns>HashSet containing indices of absent attendees</returns>
        public static HashSet<int> ConflictsForTimeslot(DateTime start, DateTime timePicked)
        {
            //*Possible Issue* We may need to check if the timeslot exists in the slotConflictsList
            TimeSpan slotLength = TimeSpan.FromMinutes(15);
            return slotConflictsList[(int)((timePicked-start)/slotLength)];
        }
    }
}
