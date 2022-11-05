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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeetingOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void BtnLunchAboutWindow_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Show();
        }

        private void BtnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            EventWindow window = new EventWindow();
            window.Show();
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Event.saveToFile();
            MessageBox.Show("Changes have been saved");
        }

        private void BtnDeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            if (LstBxEventsList.SelectedIndex != -1)
            {
                Event.eventsList.RemoveAt(LstBxEventsList.SelectedIndex);
                LstBxEventsList.Items.RemoveAt(LstBxEventsList.SelectedIndex);
                
            }
        }

        private void BtnViewEvent_Click(object sender, RoutedEventArgs e)
        {
            if (LstBxEventsList.SelectedIndex != -1) {
                EventWindow window = new EventWindow();
                window.eventIndex = LstBxEventsList.SelectedIndex;
                window.Show();
                //add lines to avoid changes

            }
        }

        private void BtnEditEvent_Click(object sender, RoutedEventArgs e)
        {
            if (LstBxEventsList.SelectedIndex != -1)
            {
                EventWindow window = new EventWindow();
                window.eventIndex = LstBxEventsList.SelectedIndex;
                window.Show();


            }
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            
            LstBxEventsList.Items.Clear();
            for (int i = 0; i < Event.eventsList.Count; i++)
            {
                LstBxEventsList.Items.Add(Event.eventsList[i].name);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Event.loadFromFile();
            MainWindow_Activated(sender, e);
        }
    }
}
