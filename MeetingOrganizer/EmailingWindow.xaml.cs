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
    /// Interaction logic for EmailingWindow.xaml
    /// </summary>
    public partial class EmailingWindow : Window
    {
        public int eventIndex { get; set; }
        public HashSet<int> whoCanAttend { get; set; }//made this a field for emailing in the future
        public HashSet<int> whoCantAttend { get; set; }

        public EmailingWindow()
        {
            InitializeComponent();
        }

        private void EmailingWindow_Activated(object sender, EventArgs e)
        {
            if (eventIndex != -1)
            {
                whoCanAttend = new HashSet<int>();
                whoCantAttend = new HashSet<int>();
                LstbxCanAttend.Items.Clear();
                LstbxCantAttend.Items.Clear();

                DateTimeRange eventRange = Event.eventsList[eventIndex].eventRange;
                TimeSpan duration = Event.eventsList[eventIndex].duration;
                DateTime chosenTimeslot = Event.eventsList[eventIndex].chosenTimeSlot;
                List<Attendee> attendees = Event.eventsList[eventIndex].attendees;

                //need to remake the conflicts list in case the Timeslots variable was cleared/changed
                //*Possible Issue* There could be discrepancies in the list between making the timeslot and opening the EmailingWindow (if there were changes in some Event fields)
                Timeslots.CalculateConflicts(eventRange, duration, Event.eventsList[eventIndex].attendees);

                //This block lists the attendees by name. It might be difficult to extract the emails the way this is implemented currently.
                (whoCanAttend, whoCantAttend) = Timeslots.WhoCanAndCannotAttend(eventRange.start, chosenTimeslot, attendees.Count);
                foreach (int canAttendIndex in whoCanAttend)
                    LstbxCanAttend.Items.Add(Event.eventsList[eventIndex].attendees[canAttendIndex].name);
                foreach (int cantAttendIndex in whoCantAttend)
                    LstbxCantAttend.Items.Add(Event.eventsList[eventIndex].attendees[cantAttendIndex].name);
            }
        }

        private void BtnSendEmails_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Emails are sent");

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
