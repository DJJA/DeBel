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
                int housenmbr = 0;
                int.TryParse(dialog.HouseNumber, out housenmbr);
                var building = new Building(-1, -1, dialog.Street, dialog.Zipcode, housenmbr);
                if (building.AddBuilding())
                {
                    LoadBuildingsInCombobox();
                    for (int i = 0; i < cbboxBuildings.Items.Count; i++)
                    {
                        var b = (Building)cbboxBuildings.Items[i];
                        if (b.Street == dialog.Street && b.Zipcode == dialog.Zipcode && b.HouseNumber == housenmbr)
                        {
                            cbboxBuildings.SelectedIndex = i;
                            break;
                        }
                    }
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
            if (cbboxBuildings.SelectedIndex >= 0)
            {
                var dialog = new AddDoorbellDialog();
                dialog.ShowDialog();
                if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                {
                    var building = (Building)cbboxBuildings.SelectedItem;
                    var doorbell = new Doorbell(-1, dialog.DoorbellName, building.Id);
                    if (doorbell.AddDoorbell())
                    {
                        MessageBox.Show("Doorbell added to the database.");
                        building.RefreshDoorbells();
                        RefreshDataGrid();
                    }
                    else MessageBox.Show("Error: Could not add doorbell to database.");

                }
                else
                    MessageBox.Show("Cancel");
            }
        }

        public void LoadBuildingsInCombobox()
        {
            cbboxBuildings.ItemsSource = Building.GetBuildings();
            if (cbboxBuildings.Items.Count > 0)
                cbboxBuildings.SelectedIndex = 0;
        }

        private void cbboxBuildings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            // Clear the datagrid
            dgGrid.ItemsSource = null;
            dgGrid.Columns.Clear();

            if (cbboxBuildings.SelectedIndex >= 0)
            {
                var building = (Building)cbboxBuildings.SelectedItem;

                var doorbells = building.Doorbells;

                // Add the columns
                var colName = new DataGridTextColumn();
                colName.Header = "Name";
                //colName.CellStyle = (Style)FindResource("DataGridColumnSeparatorStyle");
                colName.Binding = new Binding("User.Name");
                dgGrid.Columns.Add(colName);

                var colSeperator = new DataGridTemplateColumn();
                colSeperator.CellStyle = (Style)FindResource("DataGridColumnSeparatorStyle");
                colSeperator.MinWidth = 4;
                colSeperator.MaxWidth = 4;
                dgGrid.Columns.Add(colSeperator);

                for (int i = 0; i < building.Doorbells.Count; i++)
                {
                    var col = new DataGridCheckBoxColumn();
                    col.Header = building.Doorbells[i].Name;
                    col.Binding = new Binding("Permissions[" + i + "]");
                    dgGrid.Columns.Add(col);
                }

                var users = User.GetUsersAsList();

                // Format data to correct format
                var items = new List<UserDoorbellPermission>();
                foreach (var user in users)
                {
                    items.Add(new UserDoorbellPermission(user, doorbells));
                }

                // Add data to the datagrid
                dgGrid.ItemsSource = items;
            }
        }
    }
}
