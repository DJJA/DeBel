using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for UsersControl.xaml
    /// </summary>
    public partial class UsersControl : UserControl
    {
        public UsersControl()
        {
            InitializeComponent();
            DataTable dt = User.GetUsers();
            datagrid1.ItemsSource = dt.DefaultView;
            //StartUp();
        }

        MySqlDataAdapter dataAdp;
        DataTable dt;
        MySqlCommandBuilder builder;

        private void btnUpdate_Click_1(object sender, RoutedEventArgs e)
        {
            bool updateCheck = User.UpdateUsers(dt, dataAdp);
            if (updateCheck == true)
            {
                User.GetUsers();
                MessageBox.Show("succes");
            }
        }
    }
}
