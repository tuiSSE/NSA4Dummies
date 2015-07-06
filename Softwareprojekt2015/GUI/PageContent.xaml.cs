using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NSA4Dummies
{
    /// <summary>
    /// This code-behind class handles events triggered by the GUI-Buttons (close, minimize, normal)
    /// </summary>
    public partial class PageContent
    {
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
