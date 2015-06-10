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
        /// <summary>
        /// Constructor of PageContent
        /// </summary>
        public PageContent(){
            
        }
        
        
        /// <summary>
        /// This function is called when the user selects a language from the ComboBox
        /// This function then updates the DataContext of the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            LanguageFile.Language value = (LanguageFile.Language)comboBox.SelectedItem;

            Properties.Settings.Default.lan = value.sName;
            Properties.Settings.Default.Save();

            
            App.translation = LanguageFile.GetTranslation(value.sName);
            

            foreach (Window w in App.Current.Windows)
            {
                w.DataContext = new GUIViewModel();;
            }
        }

        
        /// <summary>
        /// This function is called when the ComboBox is loaded and sets up the options depending on the available languages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LangComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            LanguageFile.Language[] data = LanguageFile.GetLanguages();

            comboBox.ItemsSource = data;

            comboBox.SelectedItem = App.CurrentLanguage;

        }

        

        /// <summary>
        /// This function is called when the user clicks on the "X" (close) button
        /// The GUI Update timer will be stopped and the program is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            // Stop GUI timer
            ((MainWindow)System.Windows.Application.Current.MainWindow).guiUpdateTimer.Stop();
            App.Current.Shutdown();
        }

        
        /// <summary>
        /// This function is called when the user clicks on the "_" (minimize) button
        /// All windows will be minimized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {

            foreach(Window window in App.Current.Windows) {
                window.WindowState = WindowState.Minimized;
            }

        }

        
        /// <summary>
        /// This function is called when the user clicks on the "□" (normal/maximize) button
        /// If the window is in maximized mode it will set in normal mode and vice versa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
