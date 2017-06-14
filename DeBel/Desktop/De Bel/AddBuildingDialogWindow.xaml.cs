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

namespace De_Bel
{
    /// <summary>
    /// Interaction logic for AddBuildingDialogWindow.xaml
    /// </summary>
    public partial class AddBuildingDialogWindow : Window
    {
        public string Street
        {
            get { return tboxStreet.Text; }
        }

        public string HouseNumber
        {
            get { return tboxHouseNumber.Text; }
        }

        public string Zipcode
        {
            get { return tboxZipcode.Text; }
        }

        public AddBuildingDialogWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
