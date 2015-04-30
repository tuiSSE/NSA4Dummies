using System;
using System.Collections.Generic;
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

		public static LanguageFile.Language CurrentLanguage
		{
			get;
			set;
		}

		public App()
		{


			// Lade Settings...

			string lan = Softwareprojekt2015.Properties.Settings.Default.lan;

			// Lade Sprachdatei...

			translation = LanguageFile.GetTranslation(lan);

			UpdateTranslations();

		}

		public void UpdateTranslations(){

			foreach (Window w in Windows)
			{
				w.DataContext = App.translation;
			}
		}

	}
}
