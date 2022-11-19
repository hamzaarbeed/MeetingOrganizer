using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingOrganizer
{
    public class DateTimeRange
    {
        public DateTime start { get; private set; }
        public DateTime end { get; private set; }
        public DateTimeRange(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
        }

        override
        public string ToString()
        {
            return start.ToString("MM/dd/yyyy HH:mm") + " - " + end.ToString("MM/dd/yyyy HH:mm");
        } 
        /// <summary>
        /// Checks if this DateTimeRange overlaps with another DateTimeRange. 
        /// Overlap means that the ranges intersect (contain the same DateTime) at some point (including endpoints)
        /// </summary>
        /// <param name="otherDTR">The other DateTimeRange</param>
        /// <returns>true if it overlaps, false if not</returns>
        /*
        public bool Overlaps(DateTimeRange otherDTR)
        {
            DateTime otherStart = dtr.start;
            DateTime otherEnd = dtr.end;
            //if one end of one DateTimeRange is between both ends of the other, they overlap
            if (start<=otherStart && otherStart<=end || 
                start<=otherEnd && otherEnd<=end ||
                otherStart<=start && start<=otherEnd ||
                otherStart<=end && end<=otherEnd)
                return true;
            return false;
        }
        */
        /// <summary>
        /// Merges this and another DateTimeRange if they overlap. The merged result will be a new DateTimeRange. Throws an exception if there is no overlap.
        /// </summary>
        /// <param name="otherDTR">The DateTimeRange to merge with this one</param>
        /// <returns>A new DateTimeRange which merges the two DateTimeRanges</returns>
        /*
        public DateTimeRange Merge(DateTimeRange otherDTR)
        {
            DateTime otherStart = dtr.start;
            DateTime otherEnd = dtr.end;
            DateTime newStart = dtr.start;//start time of the new range
            DateTime newEnd = dtr.end;//end time of the new range

            if (!(this.Overlaps(otherDTR)))
                throw InvalidOperationException;


            //check the 4 overlap conditions, if the range's end is within the other's range then we don't need that end
            if (start<=otherStart && otherStart<=end)
                newStart = start;
            else
                newStart = otherStart;
            if (start<=otherEnd && otherEnd<=end)
                newEnd = end;
            else
                newEnd = otherEnd;
            return new DateTimeRange(newStart, newEnd);
        }
        */
    }
}
