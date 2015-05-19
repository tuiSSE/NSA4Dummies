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

namespace Softwareprojekt2015
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

			//binding the Apps dictonary as datacontext
            // this.DataContext = App.translation;
            this.DataContext = new GUIViewModel();
        }



        public void Update(object sender, ProgressChangedEventArgs e)
        {
            // Handle all Window Update, required when a new packet arives here. Packet content can be accessed in e.UserState.
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).snifferWorker.ProgressChanged += new ProgressChangedEventHandler(Update);
        }


    }
}
