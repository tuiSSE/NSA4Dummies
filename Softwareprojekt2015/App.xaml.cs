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
	/// Interaktionslogik für "App.xaml"
	/// </summary>
	public partial class App : Application
	{
        private TaskbarIcon tb;

		public static Dictionary<string,string> translation;

		public PacketSniffer Sniffer { get; set; }

		public static LanguageFile.Language CurrentLanguage
		{
			get;
			set;
		}

		public App()
		{
            

			Sniffer = new PacketSniffer();

			// Load settings...

			string lan = NSA4Dummies.Properties.Settings.Default.lan;

			// Load language file...

            translation = LanguageFile.GetTranslation(lan);

			Sniffer.StartSniffer();
		}

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Sniffer.StopSniffer();

        }

		private void ti_start_Click(object sender, RoutedEventArgs e)
		{
			if (!Sniffer.SnifferRunning())
			{
				Sniffer.StartSniffer();
			}
		}

		private void ti_stop_Click(object sender, RoutedEventArgs e)
		{
			if (Sniffer.SnifferRunning())
			{
				Sniffer.StopSniffer();
			}
		}

		private void ti_settings_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ti_exit_Click(object sender, RoutedEventArgs e)
		{
			this.Shutdown();
		}

	}
}
