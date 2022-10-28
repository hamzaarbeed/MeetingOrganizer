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
    public partial class AddingEventWindow : Window
    {
        public AddingEventWindow()
        {
            InitializeComponent();
        }

        private void BtnAddanAttendee_Click(object sender, RoutedEventArgs e)
        {
            AttendeeWindow window = new AttendeeWindow();
            window.Show();
        }

        private void BtnPickEventTime_Click(object sender, RoutedEventArgs e)
        {
            PickEventTimeWindow window= new PickEventTimeWindow();
            window.Show();
        }

        private void BtnDeleteAttendee_Click(object sender, RoutedEventArgs e)
        {
            YouSureWindow window= new YouSureWindow();
            window.Show();
        }

        private void BtnEmailingAttendeeWindow_Click(object sender, RoutedEventArgs e)
        {
            EmailingWindow window= new EmailingWindow();
            window.Show();
        }
    }
}
