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
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

namespace De_Bel
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
        }
        private static MySqlConnection connection = new MySqlConnection
        (@"Server=studmysql01.fhict.local;Uid=dbi338083;Database=dbi338083;Pwd=bossmonster;");

        private void btnLoadTable_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            string Query = "SELECT * FROM person";
            MySqlCommand createCommand = new MySqlCommand(Query, connection);
            createCommand.ExecuteNonQuery();

            MySqlDataAdapter dataAdp = new MySqlDataAdapter(createCommand);
            DataTable dt = new DataTable("person");
            dataAdp.Fill(dt);
            datagrid1.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);
            connection.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
