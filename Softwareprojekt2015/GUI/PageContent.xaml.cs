using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NSA4Dummies
{
    public partial class PageContent
    {
        public PageContent(){
            
        }
        
        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            LanguageFile.Language value = (LanguageFile.Language)comboBox.SelectedItem;

            
            App.translation = LanguageFile.GetTranslation(value.sName);
            

            foreach (Window w in App.Current.Windows)
            {
                w.DataContext = new GUIViewModel();;
            }
        }

        private void LangComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            LanguageFile.Language[] data = LanguageFile.GetLanguages();

            comboBox.ItemsSource = data;

            comboBox.SelectedItem = App.CurrentLanguage;

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            // Stop GUI timer
            ((MainWindow)System.Windows.Application.Current.MainWindow).guiUpdateTimer.Stop();
            App.Current.Shutdown();
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {

            foreach(Window window in App.Current.Windows) {
                window.WindowState = WindowState.Minimized;
            }

        }

        private void btn_normal_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window.WindowState != WindowState.Normal)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                }
                
            }

        }
    }
}
