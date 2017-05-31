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
    /// Interaction logic for LogControl.xaml
    /// </summary>
    public partial class LogControl : UserControl
    {
        public List<Log> mLogEntries;
        public List<Log> LogEntries
        {
            get { return mLogEntries; }
            set
            {
                mLogEntries = value;
                lbTest.ItemsSource = mLogEntries;
            }
        }

        public List<Log> AddDataLabelsToMatchesList(List<Log> LogEntries)
        {
            //matches.Insert(0, new Match(-1, DateTime.Now.ToBinary(), DateTime.Now.ToBinary()));

            DateTime lastDate = DateTime.MaxValue;
            for (int i = 0; i < LogEntries.Count; i++)
            {
                if (lastDate.Date != LogEntries[i].DateTime.Date)
                {
                    lastDate = LogEntries[i].DateTime;
                    LogEntries.Insert(i, new Log(LogType.None, lastDate, null, null));
                }
            }
            return LogEntries;
        }

        public LogControl()
        {
            InitializeComponent();
        }

        private void BtnInvoer_Click(object sender, RoutedEventArgs e)
        {
            lbTest.Items.Add(tbNaamInvoer.Text);
            tbNaamInvoer.Text = "";
        }
        private void tbNaamInvoer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lbTest.Items.Add(tbNaamInvoer.Text);
                tbNaamInvoer.Text = "";
            }
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
