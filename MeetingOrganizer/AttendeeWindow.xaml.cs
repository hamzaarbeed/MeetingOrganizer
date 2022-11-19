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
        public Attendee attendee { get; set; }

        public AttendeeWindow()
        {
            attendeeIndex = -1;
            attendee = new Attendee();
            InitializeComponent();
        }

        private void AttendeeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (attendeeIndex != -1)
            {
                attendee.name = Event.eventsList[eventIndex].attendees[attendeeIndex].name;
                attendee.email = Event.eventsList[eventIndex].attendees[attendeeIndex].email;

                //make a deep copy of availabilities, otherwise it will be linked to the original
                foreach(var dtr in Event.eventsList[eventIndex].attendees[attendeeIndex].availabilities)
                {
                    attendee.availabilities.Add(dtr);
                }
                TxtBxAttendeeName.Text = attendee.name;
                TxtBxAttendeeEmail.Text = attendee.email;
                TxtBlockEventRange.Text = Event.eventsList[eventIndex].eventRange.ToString();
                for (int i = 0; i < attendee.availabilities.Count; i++)
                {
                    LstBxAvailabilityList.Items.Add(attendee.availabilities[i].ToString());
                }
            }
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (allFieldsValid())
            {
                attendee.name = TxtBxAttendeeName.Text;
                attendee.email = TxtBxAttendeeEmail.Text;
                if (attendeeIndex == -1)
                {
                    Event.eventsList[eventIndex].attendees.Add(attendee);
                    attendeeIndex= Event.eventsList[eventIndex].attendees.Count-1;
                }
                else
                    Event.eventsList[eventIndex].attendees[attendeeIndex]=attendee;
                this.Close();
            }
        }
        private bool isValidName()
        {
            return TxtBxAttendeeName.Text!="";
        }
        private bool isValidEmail()
        {
            return TxtBxAttendeeEmail.Text!="";
        }
        private bool isValidRange()
        {
            return (DtPkrAvailabilityFrom.Text!="" &&
                    CmbBxAvailabilityFrom.Text!="" &&
                    DtPkrAvailabilityTo.Text!="" &&
                    CmbBxAvailabilityTo.Text!="");
        }
        private bool allFieldsValid()
        {
            return  isValidName() && isValidEmail();
        }
        private bool isValidAttendee()
        {
            return attendee.name!="" && attendee.email!="";//is 0 availabilities ok?
        }
        private void BtnAddAvailability_Click(object sender, RoutedEventArgs e)
        {
            if (isValidRange())
            {
                DateTimeRange dateTimeRange = new DateTimeRange(DateTime.Parse(DtPkrAvailabilityFrom.Text + " " + CmbBxAvailabilityFrom.Text + ":00")
                    , DateTime.Parse(DtPkrAvailabilityTo.Text + " " + CmbBxAvailabilityTo.Text + ":00"));
                attendee.availabilities.Add(dateTimeRange);
                LstBxAvailabilityList.Items.Add(dateTimeRange);
            }

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
            attendee.availabilities.RemoveAt(LstBxAvailabilityList.SelectedIndex);
            LstBxAvailabilityList.Items.RemoveAt(LstBxAvailabilityList.SelectedIndex);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
