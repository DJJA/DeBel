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
    /// Interaction logic for UserDashboard.xaml
    /// </summary>
    public partial class UserDashboard : Window
    {
        public UserDashboard()
        {
            InitializeComponent();
            this.Title = "Dashboard (" + User.CurrentUser.Name + ")";
            lblLoggedInAs.Content = "Logged in as: " + User.CurrentUser.Name;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            lcLogControl.Refresh();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            lcLogControl.Refresh();
        }
    }
}
