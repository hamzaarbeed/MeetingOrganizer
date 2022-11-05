using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeetingOrganizer
{

    internal class Event
    {
        public static List<Event> eventsList = new List<Event>();


        public TimeSpan duration { get; set; }//length of the event itself

        public DateTimeRange eventRange { get; set; }//time range that the event can occur
        public string name { get; set; }//name of event
        public DateTime chosenTimeSlot { get; set; }//slot chosen by the user for the event to occur on
        public List<Attendee> attendees { get; set; }//list of attendees
        public Event()
        {
            attendees = new List<Attendee>();
        }

        public static void loadFromFile()
        {
            if (File.Exists("Data.dat")) {
                string jsonString = File.ReadAllText("Data.dat");
                if (jsonString.Length > 0)
                    eventsList = JsonSerializer.Deserialize<List<Event>>(jsonString);
            }
        }

        public static void saveToFile()
        {
            string jsonString = JsonSerializer.Serialize(eventsList);
            File.WriteAllText("Data.dat", jsonString);
            
        }

        //create timeslots
        /*void MakeTimeslots()
        {
            timeslots = new Timeslots(availableTimeRange, duration, attendees);
        }
        */

        //public void AddAttendee(Attendee) { }
        //public void DeleteAttendee(Attendee) { }
    }
}
