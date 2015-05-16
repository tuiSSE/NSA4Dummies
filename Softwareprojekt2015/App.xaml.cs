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

        private BackgroundWorker snifferWorker;

		public static LanguageFile.Language CurrentLanguage
		{
			get;
			set;
		}

		public App()
		{

            snifferWorker = new BackgroundWorker();
            snifferWorker.WorkerReportsProgress = true;
            snifferWorker.DoWork += new DoWorkEventHandler();  // Add the sniffers run methode here !!! (endless loop reacting on incoming pakets)
            snifferWorker.ProgressChanged += new ProgressChangedEventHandler(((MainWindow)this.MainWindow).Update);


			// Load settings...

			string lan = Softwareprojekt2015.Properties.Settings.Default.lan;

			// Load language file...

            translation = LanguageFile.GetTranslation(lan);



		}

	}
}
