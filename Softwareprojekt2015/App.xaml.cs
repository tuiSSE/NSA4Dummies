using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Hardcodet.Wpf.TaskbarNotification;

namespace Softwareprojekt2015
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
            tb = (TaskbarIcon) FindResource("NotifyIcon");

			Sniffer = new PacketSniffer();

			// Load settings...

			string lan = Softwareprojekt2015.Properties.Settings.Default.lan;

			// Load language file...

            translation = LanguageFile.GetTranslation(lan);

			Sniffer.StartSniffer();

		}

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Sniffer.StopSniffer();

        }

	}
}
