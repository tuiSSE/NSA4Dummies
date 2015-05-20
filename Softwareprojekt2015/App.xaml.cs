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

namespace Softwareprojekt2015
{
	/// <summary>
	/// Interaktionslogik für "App.xaml"
	/// </summary>
	public partial class App : Application
	{

		public static Dictionary<string,string> translation;

        public BackgroundWorker snifferWorker;

        private PacketSniffer sniffer;

		public static LanguageFile.Language CurrentLanguage
		{
			get;
			set;
		}

		public App()
		{

            sniffer = new PacketSniffer();

            snifferWorker = new BackgroundWorker();
            snifferWorker.WorkerReportsProgress = true;
            snifferWorker.WorkerSupportsCancellation = true;
            snifferWorker.DoWork += new DoWorkEventHandler(sniffer.RunPacketSniffer);

			// Load settings...

			string lan = Softwareprojekt2015.Properties.Settings.Default.lan;

			// Load language file...

            translation = LanguageFile.GetTranslation(lan);

            snifferWorker.RunWorkerAsync();

		}

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            sniffer.Cancel();

        }

	}
}
