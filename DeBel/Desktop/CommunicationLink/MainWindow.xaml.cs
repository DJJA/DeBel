using System;
using System.Timers;
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
using System.Management;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;

namespace CommunicationLink
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string connectionString = @"Server=studmysql01.fhict.local;Uid=dbi377967;Database=dbi377967;Pwd=bossmonster;";
        private SerialMessenger serialMessenger;
        private System.Timers.Timer readMessageTimer;
        private string serverpath = "ftp://i377919@iris.fhict.nl/domains/i377919.iris.fhict.nl/public_html/PT12";

        public MainWindow()
        {
            InitializeComponent();
            MessageBuilder messageBuilder = new MessageBuilder('#', '%');


            if (AutodetectArduinoPort() != null)
            {
                serialMessenger = new SerialMessenger(AutodetectArduinoPort(), 115200, messageBuilder);
            }
            else
            {
                MessageBox.Show("No Arduino detected, Check connection and try again");
            }

            readMessageTimer = new Timer()
            {
                Interval = 10,
                AutoReset = true,
            };
            readMessageTimer.Elapsed += ReadMessageTimer_Tick;
            try
            {
                serialMessenger.SendMessage("CHECK");

            }
            catch (Exception)
            {
                MessageBox.Show("No Arduino connected");
            }
            checkFTP();

        }

        void checkFTP()
        {
            List<string> defaultPics = GetPictureList();
            List<string> newPics = defaultPics;
            while (defaultPics.Count == newPics.Count)
            {
                newPics = GetPictureList();
            }
            var newPic = newPics.First();
            AddToSQL(newPic);
        }

        private List<string> GetPictureList()
        {
            List<string> pics = new List<string>();

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverpath);
            ftpRequest.KeepAlive = true;
            ftpRequest.UsePassive = true;
            ftpRequest.UseBinary = true;

            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            ftpRequest.Credentials = new NetworkCredential("i377919", "Bossmonsters");

            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string line = streamReader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                if (line.Contains(".jpg"))
                {
                    pics.Add(line);
                    //AddToSQL(line);
                }

                line = streamReader.ReadLine();
            }

            return pics;
        }

        private void AddToSQL(string path)
        {
            string query = "INSERT INTO eventlog(Picture,EventDate,Doorbell_ID) VALUES(@Picture, @Time,@ID)";

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Picture", "http://i377919.iris.fhict.nl/" + path);
                command.Parameters.AddWithValue("@ID", 1);
                command.Parameters.AddWithValue("@Time", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }


        private void ReadMessageTimer_Tick(Object source, ElapsedEventArgs e)
        {
            string[] messages = serialMessenger.ReadMessages();
            if (messages != null)
            {
                foreach (string message in messages)
                {
                    ProcessReceivedMessage(message);
                }
            }
        }

        private void ProcessReceivedMessage(string message)
        {
            if (message == "A_LIVE")
            {
                MessageBox.Show("Arduino is live!");
            }
            else if (message.StartsWith("RPI_LIVE"))
            {
                MessageBox.Show("RPI is live!");
            }
            else if (message.StartsWith("RPI_NOTLIVE"))
            {
                MessageBox.Show("RPI is NOT live!");
            }
            else if (message.StartsWith("RANG"))
            {
                checkFTP();
            }
        }





        private string AutodetectArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc.Contains("Arduino"))
                    {
                        return deviceId;
                    }
                }
            }
            catch (ManagementException)
            {
                /* Do Nothing */
            }

            return null;
        }

        private void lstbtemp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fullFilePath = @"http://i377919.iris.fhict.nl/" + lstbtemp.SelectedItem.ToString();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            imgtemp.Source = bitmap;

        }
    }
}
