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
        private bool windowJustOpened { get; set; }//we need this to not reinitialize Event.selectedEvent; We must set Event.selectedEvent after eventIndex is set
        public EventWindow()
        {
            eventIndex = -1;
            windowJustOpened = true;
            InitializeComponent();
            
        }
        private void EventWindow_Activated(object sender, EventArgs e)
        {
            if (windowJustOpened)//initialize Event.selectedEvent only once (on window startup)
            {
                if (eventWasSelected())
                {
                    Event.selectedEvent = Event.DeepCopy(Event.eventsList[eventIndex]);
                }
                else
                {
                    Event.selectedEvent = new Event();
                }
                windowJustOpened = false;
            }
            if (eventWasSelected())
            {
                TxtbxEventName.Text = Event.selectedEvent.name;
                CmbBxEventLength.Text = Event.selectedEvent.duration.ToString(@"hh\:mm");
                DtPkrEventRangeFrom.Text = Event.selectedEvent.eventRange.start.ToString();
                DtPkrEventRangeTo.Text = Event.selectedEvent.eventRange.end.ToString();
            }
            List<Attendee> list = Event.selectedEvent.attendees;
            TxtblkChosenTimeslot.Text = Event.selectedEvent.chosenTimeSlot.ToString();
            if (list != null)
            {
                LstBxAttendeesList.Items.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    LstBxAttendeesList.Items.Add(list[i].name);
                }
            }
            
        }
        private void BtnAddanAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (isEventRangeAndDurationValid())
            {
                setEventRangeAndDuration();
                AttendeeWindow window = new AttendeeWindow();
                window.Show();
            }

        }


        private void BtnPickEventTime_Click(object sender, RoutedEventArgs e)
        {
            if (isEventRangeAndDurationValid())
            {
                setEventRangeAndDuration();
                Timeslots.CalculateConflicts(Event.selectedEvent.eventRange, Event.selectedEvent.duration, Event.selectedEvent.attendees);
                Timeslots.BestTimeslots(Event.selectedEvent.eventRange.start);
                PickEventTimeWindow window = new PickEventTimeWindow();
                window.Show();
            }

        }

        private void BtnDeleteAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (attendeeSelected())
            {
                Event.selectedEvent.attendees.RemoveAt(LstBxAttendeesList.SelectedIndex);
                LstBxAttendeesList.Items.RemoveAt(LstBxAttendeesList.SelectedIndex);
            }
        }

        private void BtnEmailingAttendeeWindow_Click(object sender, RoutedEventArgs e)
        {
            if (isChosenTimeslotValid())
            {
                EmailingWindow window= new EmailingWindow();
                window.Show();
            }
        }


        
        private bool eventWasSelected()
        {
            return eventIndex != -1;
        }
        private bool isEventRangeAndDurationValid()
        {
            return CmbBxEventLength.Text != "" && DtPkrEventRangeFrom.Text != "" && DtPkrEventRangeTo.Text != "";
        }
        private bool attendeeSelected()
        {
            return LstBxAttendeesList.SelectedIndex != -1;
        }
        private bool isChosenTimeslotValid()
        {
            DateTime from;
            DateTime to;
            DateTime slotChosen;

            bool slotWithinRange = DateTime.TryParse(DtPkrEventRangeFrom.Text, out from);
            slotWithinRange = DateTime.TryParse(DtPkrEventRangeTo.Text + " 23:59:59", out to) && slotWithinRange;
            slotWithinRange = DateTime.TryParse(TxtblkChosenTimeslot.Text, out slotChosen) && slotWithinRange;

            slotWithinRange = slotWithinRange && (slotChosen>=from) && slotChosen<to;
            return isEventRangeAndDurationValid() && slotWithinRange;
        }
        private bool allFieldsValid()
        {
            return (TxtbxEventName.Text!="" &&
                    isChosenTimeslotValid());//Do we need to add attendees to the conditions?
        }
        private void setEventRangeAndDuration()
        {
            Event.selectedEvent.duration = TimeSpan.Parse(CmbBxEventLength.Text + ":00");
            Event.selectedEvent.eventRange = new DateTimeRange(DateTime.Parse(DtPkrEventRangeFrom.Text), DateTime.Parse(DtPkrEventRangeTo.Text + " 23:59:59"));
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (allFieldsValid())
            {
                Event.selectedEvent.name = TxtbxEventName.Text;
                Event.selectedEvent.duration = TimeSpan.Parse(CmbBxEventLength.Text + ":00");
                Event.selectedEvent.eventRange = new DateTimeRange(DateTime.Parse(DtPkrEventRangeFrom.Text), DateTime.Parse(DtPkrEventRangeTo.Text + " 23:59:59"));
                if (eventWasSelected())
                    Event.eventsList[eventIndex] = Event.selectedEvent;
                else
                {
                    Event.eventsList.Add(Event.selectedEvent);
                    eventIndex = Event.eventsList.Count - 1;
                }
                this.Close();
            }
        }

        private void BtnViewAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (attendeeSelected() && isEventRangeAndDurationValid())
            {
                setEventRangeAndDuration();
                AttendeeWindow window = new AttendeeWindow();
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
            if (attendeeSelected() && isEventRangeAndDurationValid())
            {
                setEventRangeAndDuration();
                AttendeeWindow window = new AttendeeWindow();
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
