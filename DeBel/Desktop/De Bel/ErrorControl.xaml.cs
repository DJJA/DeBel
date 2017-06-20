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

namespace De_Bel
{
    /// <summary>
    /// Interaction logic for ErrorControl.xaml
    /// </summary>
    public partial class ErrorControl : UserControl
    {
        //Doorbell doorbell = new Doorbell();
        public ErrorControl()
        {
            InitializeComponent();
            //List<Log> error = doorbell.GetErrors();
            //lbError.ItemsSource = Log.AddDataLabelsToMatchesList(doorbell.Logs);
        }

        private void ErrorControl1_Loaded(object sender, RoutedEventArgs e)
        {
            //lbError.ItemsSource = Log.AddDataLabelsToMatchesList(doorbell.Logs);
            //if (lbError.Items.Count > 1)
            //    lbError.SelectedIndex = 1;
        }
    }
}
