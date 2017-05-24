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
using System.Windows.Shapes;

namespace De_Bel
{
    public class TestObject
    {
        public override string ToString()
        {
            return "hallo";
        }
    }
    /// <summary>
    /// Interaction logic for Doorbells.xaml
    /// </summary>
    public partial class Doorbells : Window
    {
        public Doorbells()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*var list = new List<TestObject>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TestObject());
            }
            
            dgGrid.ItemsSource = list;*/
            /*dgGrid.Columns[0].Header = "Username";
            dgGrid.Columns[0].Header = "Bel 1";
            dgGrid.Columns[0].Header = "Bel 2";*/
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns[0].ColumnName = "kolomnaam 1";
            dt.Columns.Add();
            dt.Columns[1].ColumnName = "kolomnaam 2";
            dgGrid.ItemsSource = dt.Columns;
        }

        private void btnAddBuilding_Click(object sender, RoutedEventArgs e)
        {
            AddBuildingDialogWindow dialog = new AddBuildingDialogWindow();
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                MessageBox.Show("Oké");
            else
                MessageBox.Show("Cancel");
        }
    }
}
