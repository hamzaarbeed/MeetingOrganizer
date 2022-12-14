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

    public class Event
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


        public Event DeepCopy(){
            Event eventCopy = new Event();
            eventCopy.duration = duration;
            eventCopy.eventRange = eventRange.DeepCopy();
            eventCopy.name = name;
            eventCopy.chosenTimeSlot = chosenTimeSlot;
            foreach (var attendee in attendees)
            {
                eventCopy.attendees.Add(attendee.DeepCopy());
            }
            return eventCopy;
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
    }
}
