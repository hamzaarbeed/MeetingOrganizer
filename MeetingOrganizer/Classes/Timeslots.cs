using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingOrganizer
{
    //call CalculateConflicts() and BestTimeslots() and the lists will be stored in the static variables
    public class Timeslots
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
        public static (HashSet<int> canAttend, HashSet<int> cantAttend) WhoCanAndCannotAttend(DateTime start, DateTime timePicked, int numAttendees)
        {
            HashSet<int> cantAttend = ConflictsForTimeslot(start, timePicked);
            HashSet<int> canAttend=new();
            for (int i=0; i<numAttendees; i++)
            {
                canAttend.Add(i);
            }
            return (canAttend.Except<int>(cantAttend).ToHashSet<int>(), cantAttend);
        }

        /// <summary>
        /// Creates a list of time ranges as strings for each set of consecutive timeslots in bestTimeslots.
        /// Assumptions: slot length is 15 minutes, bestTimeslots contains values and they are in ascending order by time
        /// </summary>
        /// <returns></returns>
        public static List<string> TimeslotsToRangeStrings()
        {
            //if there is nothing in bestTimeslots; What should we return?
            if (bestTimeslots.Count == 0)
            {
                return null;
            }


            List<string> timeslotRanges = new List<string>();//what we return
            DateTime start= bestTimeslots[0].dateAndTime;//holds start of the time range
            DateTime end;//holds end of the time range
            TimeSpan slotLength=TimeSpan.FromMinutes(15);
            DateTime previousSlot = bestTimeslots[0].dateAndTime-slotLength;//holds the previous timeslot. Begins 15 minutes before the first slot

            //check all best timeslots and add ranges as strings for consecutive times. Timeslots are assumed to be in order.
            foreach(var slot in bestTimeslots)
            {
                if (!(previousSlot+slotLength==slot.dateAndTime))
                {
                    end=previousSlot;
                    if (start==end)
                        timeslotRanges.Add(start.ToString());
                    else
                        timeslotRanges.Add(start.ToString() + " - "+ end.ToString());
                    start= slot.dateAndTime;
                }
                previousSlot = slot.dateAndTime;
            }
            
            //add a string for the last item
            end=previousSlot;
            if (start==end)
                timeslotRanges.Add(start.ToString());
            else
                timeslotRanges.Add(start.ToString() + " - "+ end.ToString());

            return timeslotRanges;
        }

        /// <summary>
        /// Converts a time range (as a string) to convert to a list of timeslots
        /// Assumptions: string is in the format "DateTime - DateTime", Each timeslot is 15 minutes long
        /// </summary>
        /// <param name="rangeString">The string of the time range to convert to a list of timeslots</param>
        /// <returns>List of timeslots (DateTime)</returns>
        /// <exception cref="ArgumentException">throws exception if the string is in the wrong format</exception>
        public static List<DateTime> RangeStringToTimeslots(string rangeString)
        {

            string[] timeInterval = rangeString.Split('-');//should contain [start] or [start, end]
            if (timeInterval.Length == 0)
            {
                return new List<DateTime>();
            }
            if (!(timeInterval.Length == 1 || timeInterval.Length==2))
            {
                throw new ArgumentException();
            }

            TimeSpan slotLength = TimeSpan.FromMinutes(15);//there are 15 minutes in each slot
            DateTime start = DateTime.Parse(timeInterval[0]);//beginning of time range
            DateTime end;//contains end of time range
            List<DateTime> timeslots = new List<DateTime>();//what we return

            //if array is in the format [start, end], end  is the second value
            if (timeInterval.Length==2)
            {
                end = DateTime.Parse(timeInterval[1]);
            }
            //if array is in the format [start], end is the first (only) value
            else
                end = DateTime.Parse(timeInterval[0]);

            //add all timeslots in the range to the list
            while (start<=end)
            {
                timeslots.Add(start);
                start+=slotLength;
            }

            return timeslots;
        }
    }
}
