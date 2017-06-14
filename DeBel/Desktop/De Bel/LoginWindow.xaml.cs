using System;
using System.Collections.Generic;
using System.IO;
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
        private static readonly string passwordPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\password";
        private static readonly string usernamePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\username";
        public LoginWindow()
        {
            InitializeComponent();

            tboxUsername.GotFocus += TboxUsername_GotFocus;
            tboxUsername.LostFocus += TboxUsername_LostFocus;
            TboxUsername_LostFocus(null, null);
            chkbxUsername.Unchecked += ChkbxUsername_Unchecked;
            chkbxPassword.Unchecked += ChkbxPassword_Unchecked;
        }

        private void ChkbxPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            if (File.Exists(passwordPath))
                File.Delete(passwordPath);

        }

        private void ChkbxUsername_Unchecked(object sender, RoutedEventArgs e)
        {
            if (File.Exists(usernamePath))
                File.Delete(usernamePath);
        }

        private void TboxUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxUsername.Text))
            {
                tboxUsername.Foreground = Brushes.LightGray;
                tboxUsername.Text = "Enter username...";
            }
            else
            {
                //SaveUsername();
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
            CheckLogin();
        }

        private void tboxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckLogin();
            }
        }

        private void tboxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckLogin();
            }
        }

        public void CheckLogin()
        {
            if (!String.IsNullOrEmpty(tboxUsername.Text) && tboxUsername.Text != "Enter username..." && !String.IsNullOrEmpty(tboxPassword.Password))
            {
                if (chkbxUsername.IsChecked == true)
                    SaveUsername();
                if (chkbxPassword.IsChecked == true)
                    SavePassword();
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
            else MessageBox.Show("Please enter a username and password.");
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadUsernameAndPassword();
        }

        private void LoadUsernameAndPassword()
        {
            try
            {
                using (var sr = new StreamReader(usernamePath))
                {
                    var value = sr.ReadLine();
                    if (!String.IsNullOrEmpty(value))
                    {
                        chkbxUsername.IsChecked = true;
                        TboxUsername_GotFocus(null, null);
                        tboxUsername.Text = value;
                        TboxUsername_LostFocus(null, null);
                    }
                }
                using (var sr = new StreamReader(passwordPath))
                {
                    var value = sr.ReadLine();
                    if (!String.IsNullOrEmpty(value))
                    {
                        chkbxPassword.IsChecked = true;
                        tboxPassword.Password = value;

                    }
                }
            }
            catch (Exception) { }
        }

        private void SaveUsername()
        {
            try
            {
                using (var sw = new StreamWriter(usernamePath, false))
                {
                    sw.WriteLine(tboxUsername.Text);
                }
            }
            catch (Exception) { }
        }

        private void SavePassword()
        {
            try
            {
                using (var sw = new StreamWriter(passwordPath, false))
                {
                    sw.WriteLine(tboxPassword.Password);
                }
            }
            catch (Exception) { }
        }

        private void tboxPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            //SavePassword();
        }
    }
}
