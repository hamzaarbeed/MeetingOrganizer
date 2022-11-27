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
        public Attendee tempAttendee;
        public Event tempEvent;

        public AttendeeWindow()
        {
            InitializeComponent();
        }

        private void AttendeeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (attendeeIndex != -1)
            {
                TxtBxAttendeeName.Text = tempAttendee.name;
                TxtBxAttendeeEmail.Text = tempAttendee.email;
                for (int i = 0; i < tempAttendee.availabilities.Count; i++)
                {
                    LstBxAvailabilityList.Items.Add(tempAttendee.availabilities[i].ToString());
                }
            }

            TxtBlockEventRange.Text = tempEvent.eventRange.ToString();
        }


        private void saveCreateEvent()
        {
            tempAttendee.name = TxtBxAttendeeName.Text;
            tempAttendee.email = TxtBxAttendeeEmail.Text;
            if (attendeeIndex == -1)
            {
                tempEvent.attendees.Add(tempAttendee);
            }
            else
            {
                tempEvent.attendees[attendeeIndex] = tempAttendee;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            saveCreateEvent();
            this.Close();
        }

        private void BtnAddAvailability_Click(object sender, RoutedEventArgs e)
        {

            DateTimeRange dateTimeRange = new DateTimeRange(DateTime.Parse(DtPkrAvailabilityFrom.Text + " " + CmbBxAvailabilityFrom.Text + ":00")
                , DateTime.Parse(DtPkrAvailabilityTo.Text + " " + CmbBxAvailabilityTo.Text + ":00"));

            LstBxAvailabilityList.Items.Add(dateTimeRange);

            tempAttendee.availabilities.Add(dateTimeRange);

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
            tempAttendee.availabilities.RemoveAt(LstBxAvailabilityList.SelectedIndex);
            LstBxAvailabilityList.Items.RemoveAt(LstBxAvailabilityList.SelectedIndex);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
