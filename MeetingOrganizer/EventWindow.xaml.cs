using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeetingOrganizer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    
    public partial class EventWindow : Window
    {
        public int eventIndex { get; set; }
        public EventWindow()
        {
            eventIndex = -1;
            InitializeComponent();
            
        }

        private void BtnAddanAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (timesExist())
            {
                saveCreateEvent();
                AttendeeWindow window = new AttendeeWindow();
                window.eventIndex = eventIndex;
                window.Show();
            }

        }


        private void BtnPickEventTime_Click(object sender, RoutedEventArgs e)
        {
            Timeslots.CalculateConflicts(Event.eventsList[eventIndex].eventRange, Event.eventsList[eventIndex].duration, Event.eventsList[eventIndex].attendees);
            Timeslots.BestTimeslots(Event.eventsList[eventIndex].eventRange.start);
            PickEventTimeWindow window = new PickEventTimeWindow();
            window.eventIndex=eventIndex;
            window.Show();
        }

        private void BtnDeleteAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (eventExists() && attendeeSelected())
            {
                Event.eventsList[eventIndex].attendees.RemoveAt(LstBxAttendeesList.SelectedIndex);
                LstBxAttendeesList.Items.RemoveAt(LstBxAttendeesList.SelectedIndex);
            }
        }

        private void BtnEmailingAttendeeWindow_Click(object sender, RoutedEventArgs e)
        {
            EmailingWindow window= new EmailingWindow();
            window.eventIndex = eventIndex;
            window.Show();
        }


        private void EventWindow_Activated(object sender, EventArgs e)
        {
            if (eventExists())
            {
                TxtbxEventName.Text = Event.eventsList[eventIndex].name;
                CmbBxEventLength.Text = Event.eventsList[eventIndex].duration.ToString(@"hh\:mm");
                DtPkrEventRangeFrom.Text = Event.eventsList[eventIndex].eventRange.start.ToString();
                DtPkrEventRangeTo.Text = Event.eventsList[eventIndex].eventRange.end.ToString();
                List<Attendee> list = Event.eventsList[eventIndex].attendees;
                TxtblkChosenTimeslot.Text = Event.eventsList[eventIndex].chosenTimeSlot.ToString();
                if (list != null)
                {
                    LstBxAttendeesList.Items.Clear();
                    for (int i = 0; i < list.Count; i++)
                    {
                        LstBxAttendeesList.Items.Add(list[i].name);
                    }
                }
            }
        }
        private bool eventExists()
        {
            return eventIndex != -1;
        }
        private bool timesExist()
        {
            return CmbBxEventLength.Text != "" && DtPkrEventRangeFrom.Text != "" && DtPkrEventRangeTo.Text != "";
        }
        private bool attendeeSelected()
        {
            return LstBxAttendeesList.SelectedIndex != -1;
        }
        private void saveCreateEvent()
        {

            if (!eventExists())
            {
                
                Event.eventsList.Add(new Event());
                eventIndex = Event.eventsList.Count - 1;
            }
            Event.eventsList[eventIndex].name = TxtbxEventName.Text;
            Event.eventsList[eventIndex].duration = TimeSpan.Parse(CmbBxEventLength.Text + ":00");
            Event.eventsList[eventIndex].eventRange = new DateTimeRange(DateTime.Parse(DtPkrEventRangeFrom.Text), DateTime.Parse(DtPkrEventRangeTo.Text + " 23:59:59"));
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            saveCreateEvent();
            this.Close();
        }

        private void BtnViewAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (eventExists() && attendeeSelected())
            {
                AttendeeWindow window = new AttendeeWindow();
                window.eventIndex = eventIndex;
                window.attendeeIndex = LstBxAttendeesList.SelectedIndex;
                window.Show();
            }
        }


        private void DtPkrEventRangeFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DtPkrEventRangeTo.Text == "")
                DtPkrEventRangeTo.Text = DtPkrEventRangeFrom.Text;
        }

        private void BtnEditAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (eventExists() && attendeeSelected())
            {
                AttendeeWindow window = new AttendeeWindow();
                window.eventIndex = eventIndex;
                window.attendeeIndex = LstBxAttendeesList.SelectedIndex;
                window.Show();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
