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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace De_Bel
{
    /// <summary>
    /// Interaction logic for DoorbellsControl.xaml
    /// </summary>
    public partial class DoorbellsControl : UserControl
    {
        public DoorbellsControl()
        {
            InitializeComponent();
        }

        private void btnAddBuilding_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddBuildingDialogWindow();
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                int i = 0;
                int.TryParse(dialog.HouseNumber, out i);
                var building = new Building(-1, -1, dialog.Street, dialog.Zipcode, i);
                if (building.AddBuilding())
                {
                    LoadBuildingsInCombobox();
                    MessageBox.Show("Building added to the database.");
                }
                else
                    MessageBox.Show("Error: Could not add building to database.");
            }
            else
                MessageBox.Show("Cancel");
        }

        private void btnAddDoorbell_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddDoorbellDialog();
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                var doorbell = new Doorbell(-1, dialog.DoorbellName, ((Building)cbboxBuildings.SelectedItem).Id);
                if (doorbell.AddDoorbell())
                {
                    MessageBox.Show("Doorbell added to the database.");
                }
                else MessageBox.Show("Error: Could not add doorbell to database.");

            }
            else
                MessageBox.Show("Cancel");
        }

        public void LoadBuildingsInCombobox()
        {
            cbboxBuildings.ItemsSource = Dashboard.User.GetBuildings();
            if (cbboxBuildings.Items.Count > 0)
                cbboxBuildings.SelectedIndex = 0;
        }

        private void cbboxBuildings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var building = (Building)e.AddedItems[0];
                //dgGrid.ItemsSource = building.Doorbells;
                dgGrid.Columns.Clear();
                for (int i = 0; i < building.Doorbells.Count; i++)
                {
                    var col = new DataGridCheckBoxColumn();

                }
            }
        }
    }
}
