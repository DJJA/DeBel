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


namespace CommunicationLink
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


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
    }
}
