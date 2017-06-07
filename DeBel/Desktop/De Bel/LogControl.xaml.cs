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
            
        }

        

        private void cboxBuilding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                var building = (Building)e.AddedItems[0];
                cboxDoorbell.ItemsSource = building.GetDoorbells(Dashboard.User);
            }
        }

        private void cboxDoorbell_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doorbell = (Doorbell)e.AddedItems[0];
            lbLog.ItemsSource = Log.AddDataLabelsToMatchesList(doorbell.GetLog());
        }

        private void lbLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var log = (Log)e.AddedItems[0];

        }
    }
}
