using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace De_Bel
{
    /// <summary>
    /// Interaction logic for LogControl.xaml
    /// </summary>
    public partial class LogControl : UserControl
    {
        public LogControl()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            cboxBuilding.ItemsSource = null;
            cboxBuilding.ItemsSource = Dashboard.User.GetBuildings();
            if (cboxBuilding.Items.Count > 0)
                cboxBuilding.SelectedIndex = 0;
        }


        private void cboxBuilding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var building = (Building)e.AddedItems[0];
                cboxDoorbell.ItemsSource = building.GetDoorbells(Dashboard.User);
                if (cboxDoorbell.Items.Count > 0)
                    cboxDoorbell.SelectedIndex = 0;
            }
        }

        private void cboxDoorbell_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var doorbell = (Doorbell)e.AddedItems[0];
                lbLog.ItemsSource = Log.AddDataLabelsToMatchesList(doorbell.Logs);
                if (lbLog.Items.Count > 1)
                    lbLog.SelectedIndex = 1;
            }
            else lbLog.ItemsSource = null;
        }

        private void lbLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear GUI
            imgPhoto.Source = null;
            tblkMessage.Text = string.Empty;

            if (e.AddedItems.Count > 0)
            {
                var log = (Log)e.AddedItems[0];
                if(log.Type == LogType.DoorbellRang)
                {
                    imgPhoto.Source = new BitmapImage(new Uri(log.PicturePath));
                    tblkMessage.Text = "Someone rang the doorbell at " + log.DateTime.ToString("HH:mm:ss");
                }
                else if(log.Type == LogType.Error)
                {
                    tblkMessage.Text = "ERROR: " + log.ErrorMessage;
                }
            }

        }
    }
}
