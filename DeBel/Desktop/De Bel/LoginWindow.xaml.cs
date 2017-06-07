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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            tboxUsername.GotFocus += TboxUsername_GotFocus;
            tboxUsername.LostFocus += TboxUsername_LostFocus;
            TboxUsername_LostFocus(null, null);
        }

        private void TboxUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxUsername.Text))
            {
                tboxUsername.Foreground = Brushes.LightGray;
                tboxUsername.Text = "Enter username...";
            }
        }

        private void TboxUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxUsername.Text) || tboxUsername.Text == "Enter username...")
            {
                tboxUsername.Text = string.Empty;
                tboxUsername.Foreground = Brushes.Black;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            User LoginCheck = User.LogInCheck(tboxUsername.Text, tboxPassword.Password);
            if (LoginCheck != null)
            {
                Dashboard dashboard = new Dashboard();
                Dashboard.User = LoginCheck;
                dashboard.Show();
                this.Close();
            }
            else MessageBox.Show("Username and Password don't match");
        }

        private void tboxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                User LoginCheck = User.LogInCheck(tboxUsername.Text, tboxPassword.Password);
                if (LoginCheck != null)
                {
                    Dashboard dashboard = new Dashboard();
                    Dashboard.User = LoginCheck;
                    dashboard.Show();
                    this.Close();
                }
            }
        }

        private void tboxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                User LoginCheck = User.LogInCheck(tboxUsername.Text, tboxPassword.Password);
                if (LoginCheck != null)
                {
                    Dashboard dashboard = new Dashboard();
                    Dashboard.User = LoginCheck;
                    dashboard.Show();
                    this.Close();
                }
            }
        }
    }
}
