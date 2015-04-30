using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Softwareprojekt2015
{
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {


        public SettingsWindow()
        {
            InitializeComponent();

			this.DataContext = App.translation;

			// Überprüfe vorhandene Sprachdateien...
			LanguageFile.Language[] lans = LanguageFile.GetLanguages();
			foreach (LanguageFile.Language lan in lans)
			{
				cb_language.Items.Add(lan);
			}
			cb_language.SelectedItem = App.CurrentLanguage;


        }

		private void cb_language_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}

		private void btn_save_Click(object sender, RoutedEventArgs e)
		{
			string lan = ((LanguageFile.Language)cb_language.SelectedItem).sName;
			App.translation = LanguageFile.GetTranslation(lan);
			App parent = (App)App.Current;
			parent.UpdateTranslations();

			Softwareprojekt2015.Properties.Settings.Default.lan = lan;
			Softwareprojekt2015.Properties.Settings.Default.Save();

			this.Close();

		}


    }
}
