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
    /// Interaction logic for DoorbellManagerDialog.xaml
    /// </summary>
    public partial class DoorbellManagerDialog : Window
    {
        public DoorbellManagerDialog()
        {
            InitializeComponent();
        }

        public DoorbellManagerDialog(Building b)
        {
            InitializeComponent();
            this.Title = "Doorbell Manager - " + b.Street + " " + b.HouseNumber;
        }
    }
}
