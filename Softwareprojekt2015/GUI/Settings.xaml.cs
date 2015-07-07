using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NSA4Dummies
{
	public partial class Settings
	{
		private bool onLoad = false;

		/// <summary>
		/// This function is called when the user selects a language from the ComboBox
		/// This function then updates the DataContext of the program.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cb_language_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (onLoad)
			{
				e.Handled = true;
				onLoad = false;
				return;
			}
			
			var comboBox = sender as ComboBox;

			LanguageFile.Language value = (LanguageFile.Language)comboBox.SelectedItem;

			Properties.Settings.Default.lan = value.sName;

			App.translation = LanguageFile.GetTranslation(value.sName);
		
		}

		
		/// <summary>
		/// This function is called when the user clicks on the save-button in the settings menu.
        /// This function saves the settings in the Settings.Default Object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btn_save_Click(object sender, RoutedEventArgs e)
		{
			
				Properties.Settings.Default.Save();

				GUIViewModel t = new GUIViewModel();

                foreach (Window w in App.Current.Windows)
                {
                    w.DataContext = t;
                }

				((App)App.Current).UpdateNotifyLanguage();		
		}


		/// <summary>
		/// This function is called when the language combobox is loaded and sets up the options depending on the available languages
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cb_language_Loaded(object sender, RoutedEventArgs e)
		{
			var comboBox = sender as ComboBox;

			onLoad = true;

			comboBox.ItemsSource = LanguageFile.GetLanguages();

			comboBox.SelectedItem = App.CurrentLanguage;
		}
	}
}
