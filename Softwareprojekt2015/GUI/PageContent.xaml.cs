using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Softwareprojekt2015
{
    public partial class PageContent
    {
        public PageContent(){
            
        }
        
        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string value = comboBox.SelectedItem as string;

            if(value == "Deutsch"){
                App.translation = LanguageFile.GetTranslation("de");
            }
            else if (value == "English")
            {
                App.translation = LanguageFile.GetTranslation("en");
            }

            foreach (Window w in App.Current.Windows)
            {
                w.DataContext = new GUIViewModel();;
            }
        }

        private void LangComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            List<string> data = new List<string>();
            data.Add("Deutsch");
            data.Add("English");

            comboBox.ItemsSource = data;

            // comboBox.SelectedIndex = 0;
        }
    }
}
