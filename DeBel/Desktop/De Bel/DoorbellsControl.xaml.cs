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
        public delegate void mouseup_delegate(object obj, MouseButtonEventArgs args);

        private bool userMadeChanges = false;

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
            RefreshBuildings();
        }

        private void RefreshBuildings()
        {
            cbboxBuildings.ItemsSource = null;
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
                colName.MinWidth = 100;
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

                    var style = new Style();
                    style.Setters.Add(new EventSetter(CheckBox.CheckedEvent, new RoutedEventHandler(CheckBox_CheckedChanged)));
                    style.Setters.Add(new EventSetter(CheckBox.UncheckedEvent, new RoutedEventHandler(CheckBox_CheckedChanged)));
                    col.CellStyle = style;

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

        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Checked changed");
            if (!userMadeChanges)
                userMadeChanges = true;
        }

        private void btnRemoveDoorbell_Click(object sender, RoutedEventArgs e)
        {
            if (cbboxBuildings.SelectedIndex >= 0)
            {
                var building = cbboxBuildings.SelectedItem as Building;
                var dialog = new RemoveDoorbellDialog(building);
                dialog.ShowDialog();
                if (dialog.MadeChanges)
                {
                    RefreshDataGrid();
                }
            }
        }

        private void btnRemoveBuilding_Click(object sender, RoutedEventArgs e)
        {
            if (cbboxBuildings.SelectedIndex >= 0)
            {
                var building = cbboxBuildings.SelectedItem as Building;
                if (building.Doorbells.Count > 0)
                {
                    MessageBox.Show("Error: Cannot remove a building with doorbells attached to it. In order to remove the building, please remove the doorbells first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to remove the building located at '" + building.Street + " " + building.HouseNumber + "'?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (building.RemoveBuilding())
                        {
                            RefreshBuildings();
                        }
                        else MessageBox.Show("Error: Could not remove the building. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbboxBuildings.SelectedIndex >= 0)
            {
                var success = true;
                var building = (Building)cbboxBuildings.SelectedItem;
                var doorbells = building.Doorbells;
                var permissions = (List<UserDoorbellPermission>)dgGrid.ItemsSource;

                for (int i = 0; i < doorbells.Count; i++)
                {
                    // Get users that have acces
                    var usersThatAccess = new List<User>();
                    foreach (var item in permissions)
                    {
                        if (item.Permissions[i])
                            usersThatAccess.Add(item.User);
                    }

                    // Remove the users that don't have access
                    if (!doorbells[i].RemoveUsersThatDontHaveAccess(usersThatAccess))
                        success = false;

                    // Add the users that do have access if they're not already there
                    if (!doorbells[i].AddUserAccess(usersThatAccess))
                        success = false;
                }

                if (success)
                    MessageBox.Show("Changes successfully saved.");
                else MessageBox.Show("There were problems saving the changes. Please try again later.");
            }
        }
    }
}
