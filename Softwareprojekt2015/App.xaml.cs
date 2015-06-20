using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Forms;

namespace NSA4Dummies
{
	/// <summary>
	/// Logic for "App.xaml"
	/// </summary>
	public partial class App : System.Windows.Application
	{

        /// <summary>
        /// Contains all strings in the currently chosen language
        /// </summary>
		public static Dictionary<string,string> translation;

        /// <summary>
        /// The actual sniffer
        /// </summary>
		public PacketSniffer Sniffer { get; set; }


        /// <summary>
        /// The currently selected language
        /// </summary>
		public static LanguageFile.Language CurrentLanguage
		{
			get;
			set;
		}

        private bool languageFileMissing = false;


        /// <summary>
        /// Consturctor
        /// </summary>
		public App()
		{
            

			Sniffer = new PacketSniffer();

			// Load settings...

			string lan = NSA4Dummies.Properties.Settings.Default.lan;

            // Check if there is at least one language file existing

            if (null == LanguageFile.GetLanguages())
            {
                
                languageFileMissing = true;
                System.Windows.MessageBox.Show("There was no language file found! Please add one manually or reinstall the software.", "ERROR! No language file found", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                // Load language file...

                translation = LanguageFile.GetTranslation(lan);

				this.niContextMenu = new System.Windows.Forms.ContextMenu();
				this.niStartSniffer = new System.Windows.Forms.MenuItem();
				this.niStopSniffer = new System.Windows.Forms.MenuItem();
				this.niSettings = new System.Windows.Forms.MenuItem();
				this.niExit = new System.Windows.Forms.MenuItem();
				this.components = new System.ComponentModel.Container();

				// Initialize niContextMenu.
				this.niContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.niStartSniffer, this.niStopSniffer, this.niSettings, this.niExit });

				// Initialize niStartSniffer.
				this.niStartSniffer.Index = 0;
				this.niStartSniffer.Text = App.translation["notifyIcon.niStartSniffer"];
				this.niStartSniffer.Click += new System.EventHandler(this.niStart_Click);

				// Initialize niStopSniffer.
				this.niStopSniffer.Index = 1;
				this.niStopSniffer.Text = App.translation["notifyIcon.niStopSniffer"];
				this.niStopSniffer.Click += new System.EventHandler(this.niStop_Click);

				// Initialize niSettings.
				this.niSettings.Index = 2;
				this.niSettings.Text = App.translation["notifyIcon.niSettings"];
				this.niSettings.Click += new System.EventHandler(this.niSettings_Click);

				// Initialize niExit.
				this.niExit.Index = 3;
				this.niExit.Text = App.translation["notifyIcon.niExit"];
				this.niExit.Click += new System.EventHandler(this.niExit_Click);

				// Create the NotifyIcon.
				this.ntfyIcon = new System.Windows.Forms.NotifyIcon(this.components);

				// Set the icon for the systray.
				ntfyIcon.Icon = (Icon)NSA4Dummies.Properties.Resources.NotifyIcon;
				ntfyIcon.ContextMenu = this.niContextMenu;
				ntfyIcon.Text = App.translation["notifyIcon.niText"];
				ntfyIcon.Visible = true;

				// Click events for NotifyIcon.
				ntfyIcon.Click += new System.EventHandler(this.ntfyIcon_Click);
				ntfyIcon.DoubleClick += new System.EventHandler(this.ntfyIcon_DoubleClick);

                // Start network sniffing
                Sniffer.StartSniffer();
            }

            

			
		}
        /// <summary>
        /// This method is called to show the balloon tip.
        /// </summary>
        /// <param name="balloonTitle">Sets the title of the balloon tip.</param>
        /// <param name="balloonText">Sets the text of the balloon tip.</param>
        /// <param name="balloonTimeout">Sets the display time of the balloon tip. The default value is 3 seconds.</param>
        /// <param name="balloonIcon">Sets the icon which is shown in the balloon tip. The default icon is set to none.</param>
        public void ShowTaskIconNotification(string balloonTitle, string balloonText, int balloonTimeout = 3000, ToolTipIcon balloonIcon = ToolTipIcon.None)
        {
            ntfyIcon.ShowBalloonTip(balloonTimeout, balloonTitle, balloonText, balloonIcon);
        }

        /// <summary>
        /// Updates the context menu entries of the notify icon when a different language is selected.
        /// </summary>
		public void UpdateNotifyLanguage()
		{
			this.niStartSniffer.Text = App.translation["notifyIcon.niStartSniffer"];
			this.niStopSniffer.Text = App.translation["notifyIcon.niStopSniffer"];
			this.niSettings.Text = App.translation["notifyIcon.niSettings"];
			this.niExit.Text = App.translation["notifyIcon.niExit"];
			ntfyIcon.Text = App.translation["notifyIcon.niText"];
		}

        /// <summary>
        /// Gets called on application shutdown and stops the sniffer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            
            if (Sniffer.SnifferRunning())
            {
                Sniffer.StopSniffer();
            }
            
            if (components != null)
            {
                components.Dispose();
            }

        }


        // Declaration of NotifyIcon and its context menu.
        private System.Windows.Forms.NotifyIcon ntfyIcon;
        private System.Windows.Forms.ContextMenu niContextMenu;
        private System.Windows.Forms.MenuItem niStartSniffer;
        private System.Windows.Forms.MenuItem niStopSniffer;
        private System.Windows.Forms.MenuItem niSettings;
        private System.Windows.Forms.MenuItem niExit;
        private System.ComponentModel.IContainer components;


        /// <summary>
        /// Not used yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void niStart_Click(object sender, EventArgs e)
		{
			if (!Sniffer.SnifferRunning())
			{
				Sniffer.StartSniffer();
			}
		}

        /// <summary>
        /// Not used yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void niStop_Click(object Sender, EventArgs e)
		{
			if (Sniffer.SnifferRunning())
			{
				Sniffer.StopSniffer();
			}
		}

        /// <summary>
        /// Not used yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void niSettings_Click(object Sender, EventArgs e)
		{

		}

        /// <summary>
        /// Not used yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void niExit_Click(object Sender, EventArgs e)
		{
			this.Shutdown();
		}
        /// <summary>
        /// Event for clicking the notify icon.
        /// If you click on the icon once, the balloon tip will show up with information
        /// about the current status of the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntfyIcon_Click(object sender, EventArgs e)
        {
            ntfyIcon.Visible = true;
        }
        /// <summary>
        /// Event for doubleclicking the notify icon.
        /// If the main window is minized, the window will be restored with a double click.
        /// If the main window is visible, it will be minimzed on double click.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ntfyIcon_DoubleClick(object Sender, EventArgs e)
        {
			if (MainWindow.WindowState == WindowState.Minimized)
			{
				MainWindow.WindowState = WindowState.Normal;
			}
			else if (MainWindow.WindowState == WindowState.Normal)
			{
				MainWindow.WindowState = WindowState.Minimized;
			}
			


            MainWindow.Activate();
        }

        private void Application_Startup(object Sender, StartupEventArgs e)
        {
            
            if (languageFileMissing)
            {

                this.Shutdown();
            }
        
        }

	}
}
