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
        public Dashboard()
        {
            InitializeComponent();
            this.Title = "Dashboard (" + User.CurrentUser.Name + ")";
            lblLoggedInAs.Content = "Logged in as: " + User.CurrentUser.Name;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            tabLogControl.Refresh();
            InitDoorbellsControll();
        }

        private void InitDoorbellsControll()
        {
            tabDoorbellControl.LoadBuildingsInCombobox();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void tabLog_Clicked(object sender, MouseButtonEventArgs e)
        {
            tabLogControl.Refresh();
        }
    }
}
