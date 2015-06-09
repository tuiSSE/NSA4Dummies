using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Controls;

namespace NSA4Dummies
{
	/// <summary>
	/// Logic for "App.xaml"
	/// </summary>
	public partial class App : Application
	{
        //private TaskbarIcon tb;

        /// <summary>
        /// Contains all strings in the currently chosen language
        /// </summary>
		public static Dictionary<string,string> translation;

        /// <summary>
        /// The actual sniffer
        /// </summary>
		public PacketSniffer Sniffer { get; set; }


        /// <summary>
        /// The currently selected language
        /// </summary>
		public static LanguageFile.Language CurrentLanguage
		{
			get;
			set;
		}


        /// <summary>
        /// Consturctor
        /// </summary>
		public App()
		{
            

			Sniffer = new PacketSniffer();

			// Load settings...

			string lan = NSA4Dummies.Properties.Settings.Default.lan;

			// Load language file...

            translation = LanguageFile.GetTranslation(lan);

            // Start network sniffing
			Sniffer.StartSniffer();
		}

        /// <summary>
        /// Gets called on application shutdown and stops the sniffer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (Sniffer.SnifferRunning())
            {
                Sniffer.StopSniffer();
            }
            

        }

        /// <summary>
        /// Not used yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ti_start_Click(object sender, RoutedEventArgs e)
		{
			if (!Sniffer.SnifferRunning())
			{
				Sniffer.StartSniffer();
			}
		}

        /// <summary>
        /// Not used yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ti_stop_Click(object sender, RoutedEventArgs e)
		{
			if (Sniffer.SnifferRunning())
			{
				Sniffer.StopSniffer();
			}
		}

        /// <summary>
        /// Not used yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ti_settings_Click(object sender, RoutedEventArgs e)
		{

		}

        /// <summary>
        /// Not used yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ti_exit_Click(object sender, RoutedEventArgs e)
		{
			this.Shutdown();
		}

	}
}
