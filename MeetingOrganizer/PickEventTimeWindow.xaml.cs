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
        public int eventIndex { get; set; }
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
            if (eventIndex != -1 && CmbBxEventTimeSlot.SelectedIndex != -1)
            {
                DateTime chosenTimeSlot;
                if (DateTime.TryParse(CmbBxEventTimeSlot.SelectedItem.ToString(), out chosenTimeSlot))
                {
                    Event.eventsList[eventIndex].chosenTimeSlot = chosenTimeSlot;
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
    }
}
