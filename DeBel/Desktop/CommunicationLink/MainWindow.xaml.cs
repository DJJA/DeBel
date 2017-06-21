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
            CheckFTP();

        }

        private string CheckFTP()
        {
            string result = "";
            string serverpath = "ftp://i377919@iris.fhict.nl/domains/i377919.iris.fhict.nl/public_html/PT12";
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverpath);
            ftpRequest.KeepAlive = true;
            ftpRequest.UsePassive = true;
            ftpRequest.UseBinary = true;
            
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            ftpRequest.Credentials = new NetworkCredential("i377919","Bossmonsters");

            FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string line = streamReader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                if (line.Contains(".jpg"))
                { 
                    lstbtemp.Items.Add(line);
                    //AddToSQL(line);
                }
                
                line = streamReader.ReadLine();
            }

            



            return result;
        }

        private void AddToSQL(string path)
        {
            string query = "";

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PathToFile", path);

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

            }
            else if (message.StartsWith("RPI_LIVE"))
            {

            }
            else if (message.StartsWith("RPI_NOTLIVE"))
            {
                
            }
            else if (message.StartsWith("CHECKFTP"))
            {
                
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
            catch (ManagementException e)
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
