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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnInvoer_Click(object sender, RoutedEventArgs e)
        {
            lbTest.Items.Add(tbNaamInvoer.Text);
            tbNaamInvoer.Text = "";
        }

        private void lbTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string curItem = lbTest.SelectedItem.ToString();
            lblNaam.Content = curItem;
            if (curItem == "max")
            {
                picFoto.Source = new BitmapImage(new Uri(@"C:\Users\Loek\Desktop\FHICT\media\html\images\Max.jpg"));
                lblEmail.Content = "e-mail";
                lblTelnr.Content = "Telefoonnummer";
            }
            else if (curItem == "nalah")
            {
                picFoto.Source = new BitmapImage(new Uri(@"C:\Users\Loek\Desktop\FHICT\media\html\images\nalah.jpeg"));
            }
            else if (curItem == "dennis")
            {
                picFoto.Source = new BitmapImage(new Uri(@"C:\Users\Loek\Desktop\FHICT\media\html\images\dennis.jpg"));
            }
            else picFoto.Source = new BitmapImage(new Uri(@"C:\Users\Loek\Desktop\FHICT\media\html\images\no-image.jpg"));
        }
    }
}
