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


namespace Softwareprojekt2015
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random rnd = new Random();
        
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {            
            InitializeComponent();

			//binding the Apps dictonary as datacontext
            // this.DataContext = App.translation;
            this.DataContext = new GUIViewModel();
        }


        /// <summary>
        /// Handle all Window Update, required when a new packet arives here. Packet content can be accessed in e.UserState.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Update(object sender, ProgressChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(new Action(() => updateGUI(e)));
        }


        public void updateGUI(ProgressChangedEventArgs e)
        {
            var packet = e.UserState as DataPacket;

            // Add a packet to the country that is associated with the IP address
            WorldMap.addPackageToCountryUpdate(packet.DestIP.ToString());

            string[] fileTypes = { "mp3", "jpeg", "html", "js", "css", "gif", "png", "flv" };
            int choice = rnd.Next(0, fileTypes.Length);
            ((GUIViewModel)this.DataContext).addFileType(fileTypes[choice]);

            string[] domains = { "google.com", "facebook.com", "web.de", "gmail.com", "youtube.com", "tu-ilmenau.de" };
            choice = rnd.Next(0, domains.Length);
            ((GUIViewModel)this.DataContext).addDomain(domains[choice]);

            bool[] encrypted = { true, false };
            choice = rnd.Next(0, encrypted.Length);
            ((GUIViewModel)this.DataContext).addPackage(encrypted[choice]);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            ((App)App.Current).snifferWorker.ProgressChanged += new ProgressChangedEventHandler(Update);
            
        }
    }
}
