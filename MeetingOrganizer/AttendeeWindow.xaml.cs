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
    /// Interaction logic for AttendeeWindow.xaml
    /// </summary>
    public partial class AttendeeWindow : Window

    {
        public int attendeeIndex { get; set; }
        public int eventIndex { get; set; }

        public AttendeeWindow()
        {
            attendeeIndex = -1;
            InitializeComponent();
        }

        private void AttendeeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (attendeeIndex != -1)
            {
                Attendee attendee = Event.eventsList[eventIndex].attendees[attendeeIndex];
                TxtBxAttendeeName.Text = attendee.name;
                TxtBxAttendeeEmail.Text = attendee.email;
                TxtBlockEventRange.Text = Event.eventsList[eventIndex].eventRange.ToString();
                for (int i = 0; i < attendee.availabilities.Count; i++)
                {
                    LstBxAvailabilityList.Items.Add(attendee.availabilities[i].ToString());
                }
            }
        }


        private void saveCreateAttendee(){

            
            
            if (attendeeIndex == -1)
            {
                Attendee temp = new Attendee();
                temp.name = TxtBxAttendeeName.Text;
                temp.email = TxtBxAttendeeEmail.Text;
                Event.eventsList[eventIndex].attendees.Add(temp);
                attendeeIndex= Event.eventsList[eventIndex].attendees.Count-1;
            }
            else
            {
                Event.eventsList[eventIndex].attendees[attendeeIndex].name = TxtBxAttendeeName.Text;
                Event.eventsList[eventIndex].attendees[attendeeIndex].email = TxtBxAttendeeEmail.Text;
            }
            

        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            saveCreateAttendee();
            this.Close();
        }

        private void BtnAddAvailability_Click(object sender, RoutedEventArgs e)
        {
            saveCreateAttendee();

            DateTimeRange dateTimeRange = new DateTimeRange(DateTime.Parse(DtPkrAvailabilityFrom.Text + " " + CmbBxAvailabilityFrom.Text + ":00")
                , DateTime.Parse(DtPkrAvailabilityTo.Text + " " + CmbBxAvailabilityTo.Text + ":00"));
            LstBxAvailabilityList.Items.Add(dateTimeRange);
            Event.eventsList[eventIndex].attendees[attendeeIndex].availabilities.Add(dateTimeRange);

        }

        private void DtPkrAvailabilityFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DtPkrAvailabilityTo.Text = DtPkrAvailabilityFrom.Text;
        }

        private void CmbBxAvailabilityFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            CmbBxAvailabilityTo.SelectedIndex = CmbBxAvailabilityFrom.SelectedIndex;
        }

        private void BtnRemoveAvailability_Click(object sender, RoutedEventArgs e)
        {
            Event.eventsList[eventIndex].attendees[attendeeIndex].availabilities.RemoveAt(LstBxAvailabilityList.SelectedIndex);
            LstBxAvailabilityList.Items.RemoveAt(LstBxAvailabilityList.SelectedIndex);
        }
    }
}
