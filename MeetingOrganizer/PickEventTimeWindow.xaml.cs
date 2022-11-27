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
    public partial class PickEventTimeWindow : Window
    {
        Dictionary<DateTime, HashSet<int>> _eventTimes = new Dictionary<DateTime, HashSet<int>>();
        //public int eventIndex { get; set; }
        public Event tempEvent;

        public PickEventTimeWindow()
        {
            InitializeComponent();
        }
        private void PickEventTimeWindow_Activated(object sender, EventArgs e)
        {
            List<string> timeslotRanges = Timeslots.TimeslotsToRangeStrings();
            for (int i = 0; i < timeslotRanges.Count; i++)
            {
                LstBxTimeslots.Items.Add(timeslotRanges[i]);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (CmbBxEventTimeSlot.SelectedIndex != -1)
            {
                DateTime chosenTimeSlot;
                if (DateTime.TryParse(CmbBxEventTimeSlot.SelectedItem.ToString(), out chosenTimeSlot))
                {
                    tempEvent.chosenTimeSlot = chosenTimeSlot;
                    this.Close();
                }
            }
        }
        private void LstBxTimeslots_SelectedRangeChanged(object sender, SelectionChangedEventArgs e)
        {
            CmbBxEventTimeSlot.Items.Clear();
            foreach (var slot in Timeslots.RangeStringToTimeslots(LstBxTimeslots.SelectedItem.ToString()))
            {
                CmbBxEventTimeSlot.Items.Add(slot);
            }
        }

        private void CmbBxTimeslots_SelectedRangeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbBxEventTimeSlot.SelectedIndex != -1)//should we clear the list if no timeslot selected? (eg. if range changes)
            {
                DateTime chosenTimeSlot;
                if (DateTime.TryParse(CmbBxEventTimeSlot.SelectedItem.ToString(), out chosenTimeSlot))//also initializes chosenTimeSlot
                {
                    //clear boxes and calculate who can and cannot attend
                    LstBxCanAttend.Items.Clear();
                    LstBxCantAttend.Items.Clear();
                    (var canAttend, var cantAttend) = Timeslots.WhoCanAndCannotAttend(tempEvent.eventRange.start, chosenTimeSlot, tempEvent.attendees.Count);

                    //add each attendee's name to the appropriate box
                    foreach(var attendeeIndex in canAttend)
                        LstBxCanAttend.Items.Add(tempEvent.attendees[attendeeIndex].name);
                    foreach(var attendeeIndex in cantAttend)
                        LstBxCantAttend.Items.Add(tempEvent.attendees[attendeeIndex].name);
                }
            }
        }
    }
}
