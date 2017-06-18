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
    /// Interaction logic for RemoveDoorbellDialog.xaml
    /// </summary>
    public partial class RemoveDoorbellDialog : Window
    {
        public bool MadeChanges { get; set; }
        private Building Building { get; set; }

        public RemoveDoorbellDialog(Building b)
        {
            MadeChanges = false;
            this.Building = b;
            InitializeComponent();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lboxDoorbells.SelectedIndex >= 0)
            {
                var doorbell = Building.Doorbells[lboxDoorbells.SelectedIndex];
                if(MessageBox.Show("Are you sure you want to remove the doorbell with name '" + doorbell.Name + "'?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (doorbell.RemoveDoorbell())
                    {
                        Building.RefreshDoorbells();
                        RefreshListBox();
                        MadeChanges = true;
                    }
                    else MessageBox.Show("Error removing doorbell");
                }
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            RefreshListBox();
        }

        private void RefreshListBox()
        {
            lboxDoorbells.ItemsSource = null;
            lboxDoorbells.ItemsSource = Building.Doorbells;
            if (Building.Doorbells.Count > 0)
                lboxDoorbells.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
