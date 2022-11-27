using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Windows.Media.Animation;
using System.Collections.Specialized;

namespace MeetingOrganizer
{
    /// <summary>
    /// Interaction logic for EmailingWindow.xaml
    /// </summary>
    public partial class EmailingWindow : Window
    {
        public Event tempEvent;
        public HashSet<int> whoCanAttend { get; set; }//made this a field for emailing in the future
        public HashSet<int> whoCantAttend { get; set; }

        public EmailingWindow()
        {
            InitializeComponent();
        }

        private void EmailingWindow_Activated(object sender, EventArgs e)
        {
            whoCanAttend = new HashSet<int>();
            whoCantAttend = new HashSet<int>();
            LstbxCanAttend.Items.Clear();
            LstbxCantAttend.Items.Clear();

            DateTimeRange eventRange = tempEvent.eventRange;
            TimeSpan duration = tempEvent.duration;
            DateTime chosenTimeslot = tempEvent.chosenTimeSlot;
            List<Attendee> attendees = tempEvent.attendees;

            //need to remake the conflicts list in case the Timeslots variable was cleared/changed
            //*Possible Issue* There could be discrepancies in the list between making the timeslot and opening the EmailingWindow (if there were changes in some Event fields)
            Timeslots.CalculateConflicts(eventRange, duration, tempEvent.attendees);

            //This block lists the attendees by name. It might be difficult to extract the emails the way this is implemented currently.
            (whoCanAttend, whoCantAttend) = Timeslots.WhoCanAndCannotAttend(eventRange.start, chosenTimeslot, attendees.Count);
            foreach (int canAttendIndex in whoCanAttend)
                LstbxCanAttend.Items.Add(new ListBoxItem() { Content = tempEvent.attendees[canAttendIndex].name,Tag = canAttendIndex });
            foreach (int cantAttendIndex in whoCantAttend)
                LstbxCantAttend.Items.Add(new ListBoxItem() { Content = tempEvent.attendees[cantAttendIndex].name, Tag = cantAttendIndex });

        }

        private void BtnSendEmails_Click(object sender, RoutedEventArgs e)
        {
            string appEmail = "meetingorganizercodesage@outlook.com";
            var smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                
                Port = 587,
                Credentials = new NetworkCredential(appEmail, PswrdBx.Password.ToString()),
                EnableSsl = true,
            };
            foreach (ListBoxItem item in LstbxCanAttend.Items)
            {
                Attendee attendee = tempEvent.attendees[(int)item.Tag];
                Event eventt = tempEvent;

                smtpClient.Send(appEmail, attendee.email, "Invite to Scheduled Event: " + tempEvent.name, "Hello " + attendee.name + ",\n" +
                "You are invited to attend " + eventt.name + " scheduled at " + eventt.chosenTimeSlot + ".\n Thank you");
            }
            foreach (ListBoxItem item in LstbxCantAttend.Items)
            {
                Attendee attendee = tempEvent.attendees[(int)item.Tag];
                Event eventt = tempEvent;

                smtpClient.Send(appEmail, attendee.email, "Scheduled Event: " + tempEvent.name, "Hello " + attendee.name + ",\n\n" +
                "We are sorry to let you know that you can't attend " + eventt.name + " scheduled at " + eventt.chosenTimeSlot + " because of your availabilities." +
                "\n\nThank you");
            }

            MessageBox.Show("Emails are sent");
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
