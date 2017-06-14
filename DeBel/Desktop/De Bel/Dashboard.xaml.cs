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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public static User User { get; set; }
        public static Doorbell doorbell { get;set; }
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            tabLogControl.cboxBuilding.ItemsSource = Dashboard.User.GetBuildings();
            if (tabLogControl.cboxBuilding.Items.Count > 0)
                tabLogControl.cboxBuilding.SelectedIndex = 0;
        }
    }
}
