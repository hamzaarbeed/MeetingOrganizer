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
        public Event tempEvent;

        public EventWindow()
        {
            InitializeComponent();
            
        }

        private bool isAllFieldsFilled()
        {
            return (TxtbxEventName.Text != "" && CmbBxEventLength.Text != "" && DtPkrEventRangeFrom.Text != "" && DtPkrEventRangeTo.Text != "");
        }
        private void BtnAddanAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (isAllFieldsFilled() == false)
                return;

            saveBasicEventInfo();

            AttendeeWindow window = new AttendeeWindow();
            window.tempEvent = tempEvent;
            window.attendeeIndex = -1;
            window.tempAttendee = new Attendee();
            window.Show();
        }


        private void BtnPickEventTime_Click(object sender, RoutedEventArgs e)
        {
            if (isAllFieldsFilled() == false)
                return;

            Timeslots.CalculateConflicts(tempEvent.eventRange, tempEvent.duration, tempEvent.attendees);
            Timeslots.BestTimeslots(tempEvent.eventRange.start);
            PickEventTimeWindow window = new PickEventTimeWindow();
            window.tempEvent= tempEvent;
            window.Show();

        }

        private void BtnDeleteAttendee_Click(object sender, RoutedEventArgs e)
        {
            if (attendeeSelected())
            {
                tempEvent.attendees.RemoveAt(LstBxAttendeesList.SelectedIndex);
                LstBxAttendeesList.Items.RemoveAt(LstBxAttendeesList.SelectedIndex);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------
        private void BtnEmailingAttendeeWindow_Click(object sender, RoutedEventArgs e)
        {
            if (tempEvent.chosenTimeSlot != new DateTime())
            {
                EmailingWindow window = new EmailingWindow();
                window.tempEvent = tempEvent;
                window.Show();
            }
        }


        private void EventWindow_Activated(object sender, EventArgs e)
        {
            TxtbxEventName.Text = tempEvent.name;
            CmbBxEventLength.Text = tempEvent.duration.ToString(@"hh\:mm");
            if (tempEvent.eventRange != null) { 
                DtPkrEventRangeFrom.Text = tempEvent.eventRange.start.ToString();
                DtPkrEventRangeTo.Text = tempEvent.eventRange.end.ToString();
            }
            List<Attendee> list = tempEvent.attendees;
            if (tempEvent.chosenTimeSlot > new DateTime())
                TxtblkChosenTimeslot.Text = tempEvent.chosenTimeSlot.ToString();
            if (list != null)
            {
                LstBxAttendeesList.Items.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    LstBxAttendeesList.Items.Add(list[i].name);
                }
            }
        }

        private bool eventExists()
        {
            return eventIndex != -1;
        }

        private bool attendeeSelected()
        {
            return LstBxAttendeesList.SelectedIndex != -1;
        }

        private void saveBasicEventInfo()
        {
            tempEvent.name = TxtbxEventName.Text;
            tempEvent.duration = TimeSpan.Parse(CmbBxEventLength.Text + ":00");
            tempEvent.eventRange = new DateTimeRange(DateTime.Parse(DtPkrEventRangeFrom.Text), DateTime.Parse(DtPkrEventRangeTo.Text + " 23:59:59"));
        }
        private void saveCreateEvent()
        {
            saveBasicEventInfo();
            if (!eventExists()){          
                Event.eventsList.Add(tempEvent);
            }else{
                Event.eventsList[eventIndex] = tempEvent;
            }
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
                window.attendeeIndex = LstBxAttendeesList.SelectedIndex;
                window.tempAttendee = tempEvent.attendees[window.attendeeIndex].DeepCopy();
                window.tempEvent = tempEvent;
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
                window.attendeeIndex = LstBxAttendeesList.SelectedIndex;
                window.tempAttendee = tempEvent.attendees[window.attendeeIndex].DeepCopy();
                window.tempEvent = tempEvent;
                window.Show();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
