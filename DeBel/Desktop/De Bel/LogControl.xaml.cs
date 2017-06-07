using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for LogControl.xaml
    /// </summary>
    public partial class LogControl : UserControl
    {
        public LogControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public List<Building> GetBuildings()
        {
            var list = new List<Building>();
            string query = "SELECT * FROM Building b, Building_Person bp WHERE b.ID = bp.Building_ID AND bp.Person_ID = @Person_ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@Person_ID", Id);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["ID"]);
                        int companyID = Convert.ToInt32(dt.Rows[i]["Company_ID"]);
                        string street = (string)dt.Rows[i]["Street"];
                        string zipcode = (string)dt.Rows[i]["Zipcode"];
                        int houseNumber = Convert.ToInt32(dt.Rows[i]["HouseNumber"]);
                        list.Add(new Building(id, companyID, street, zipcode, houseNumber));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }

        private void cboxBuilding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var building = (Building)e.AddedItems[0];
            cboxDoorbell.ItemsSource = building.GetDoorbells(null);
        }

        private void cboxDoorbell_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doorbell = (Doorbell)e.AddedItems[0];
            lbLog.ItemsSource = Log.AddDataLabelsToMatchesList(doorbell.GetLog());
        }

        private void lbLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var log = (Log)e.AddedItems[0];

        }
    }
}
