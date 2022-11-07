﻿using System;
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
            for (int i = 0; i < Timeslots.bestTimeslots.Count; i++)
            {
                LstBxTimeslots.Items.Add(Timeslots.bestTimeslots[i].dateAndTime.ToString());
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (eventIndex != -1 && LstBxTimeslots.SelectedIndex != -1)
            {
                Event.eventsList[eventIndex].chosenTimeSlot = Timeslots.bestTimeslots[LstBxTimeslots.SelectedIndex].dateAndTime;
                this.Close();
            }
        }
    }
}
