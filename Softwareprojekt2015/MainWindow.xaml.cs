// Doxygen mainpage
/*! \mainpage NSA for Dummies
 *
 * \section intro_sec General
 *
 * Our software project titled "NSA 4 Dummies" is a small, lightweight application to show unencrpyted traffic and IP-Routing in your own home network. 
 * There's no need for further knowledge regarding network security, so this tool is for everyone who want's to get a fast and general overview of their data traffic.
 * "NSA 4 Dummies" was developed by a team of students from the "Technische Universität Ilmenau" during their mandatory 4th semester course called "Softwareprojekt".
 * Encrypted data isn't supposed to be logged and intruding secured networks isn't able with this application. 
 * Using the software does not violate §202a, §202b, §202c StGB.
 * 
 * For more details have a look at https://github.com/Softwareprojekt2015/NSA4Dummies
 *
 *
 * \section credits Credits
 * 
 * \subsection third_party Third Party Libraries
 * 
 * WinPcap\n
 * Modern UI Charts
 * 
 * \subsection developer Developer
 * 
 * Peter Brodkorb\n
 * Clemens Fischer\n
 * Dennis Pietsch\n
 * Timo Sadzik\n
 * Franziska Selle\n
 * Martin Wolf
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using System.Timers;


namespace NSA4Dummies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random rnd = new Random();
        public Timer guiUpdateTimer = new Timer();
        
        /// <summary>
        /// The constructor of the main window
        /// </summary>
        public MainWindow()
        {            
            InitializeComponent();

			//binding the Apps dictonary as datacontext
            // this.DataContext = App.translation;
            this.DataContext = new GUIViewModel();

            // GUI update timer
            guiUpdateTimer.Elapsed += new ElapsedEventHandler(updateGUI);
            guiUpdateTimer.Interval = 5000; // 1000 ms is one second

        }


        /// <summary>
        /// Since the package sniffer runs in another thread we need a function that invokes the 'updateGUIData' function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Update(object sender, ProgressChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => updateGUIData(e)));
        }


        /// <summary>
        /// This function updates the data of the GUI (not the GUI itself), required when a new packet arrives here. Packet content can be accessed in e.UserState.
        /// </summary>
        /// <param name="e"></param>
        public void updateGUIData(ProgressChangedEventArgs e)
        {
            var packet = e.UserState as DataPacket;

            System.Net.IPAddress ip = packet.DestIP;

            if (ip != null)
            {
                // Add a packet to the country that is associated with the IP address
                WorldMap.addPackageToCountryUpdate(ip.ToString());
                ((GUIViewModel)this.DataContext).addPackage(packet.Length, packet.Protocol, IP2Country.number2Country(IP2Country.address2Number(ip.ToString())));
            }        
        }


        /// <summary>
        /// Since the GUI update timer runs in another thread we need to invoke 'updateGUI' with this function
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void updateGUI(object source, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => updateGUI()));
        }


        /// <summary>
        /// This function updates the GUI every guiUpdateTimer.Interval milliseconds
        /// </summary>
        public void updateGUI()
        {
            // Update graphs
            ((GUIViewModel)this.DataContext).updateDataGraphs();

            // Update map
            WorldMap.updateCountries();
        }

        
        /// <summary>
        /// This function is called as soon as the Window is loaded since some objects are only available if they are visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			((App)App.Current).Sniffer.AddPacketHandler(new ProgressChangedEventHandler(Update));
            guiUpdateTimer.Start();
            
        }

        /// <summary>
        /// Lets the user move the window when not maximized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
			if (WindowState.Normal == this.WindowState)
			{
				this.Cursor = Cursors.SizeAll;
				this.DragMove();
				
			}
            
        }

		private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (WindowState.Normal == this.WindowState)
			{
				this.Cursor = Cursors.Arrow;

			}
		}
    }
}
