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
            StartUp();
        }
        private static MySqlConnection connection = new MySqlConnection
        (@"Server=studmysql01.fhict.local;Uid=dbi338083;Database=dbi338083;Pwd=bossmonster;");
        MySqlDataAdapter dataAdp;
        DataTable dt;
        MySqlCommandBuilder builder;

        private void StartUp()
        {
            connection.Open();
            string Query = "SELECT * FROM person";
            MySqlCommand createCommand = new MySqlCommand(Query, connection);
            createCommand.ExecuteNonQuery();

            dataAdp = new MySqlDataAdapter(createCommand);
            dt = new DataTable("person");
            dataAdp.Fill(dt);
            datagrid1.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);
            connection.Close();
        }

        private void btnUpdate_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                builder = new MySqlCommandBuilder(dataAdp);
                dataAdp.Update(dt);
                MessageBox.Show("succes!");
                StartUp();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voer alle velden in");
            }
        }
    }
}
