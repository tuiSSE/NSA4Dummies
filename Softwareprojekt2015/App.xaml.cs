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

                // Start network sniffing
                Sniffer.StartSniffer();
            }

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
            this.niStartSniffer.Text = "Start Sniffer";
            this.niStartSniffer.Click += new System.EventHandler(this.niStart_Click);

            // Initialize niStopSniffer.
            this.niStopSniffer.Index = 1;
            this.niStopSniffer.Text = "Stop Sniffer";
            this.niStopSniffer.Click += new System.EventHandler(this.niStop_Click);
            
            // Initialize niSettings.
            this.niSettings.Index = 2;
            this.niSettings.Text = "Settings";
            this.niSettings.Click += new System.EventHandler(this.niSettings_Click);

            // Initialize niExit.
            this.niExit.Index = 3;
            this.niExit.Text = "Exit Application";
            this.niExit.Click += new System.EventHandler(this.niExit_Click);

            // Create the NotifyIcon.
            this.ntfyIcon = new System.Windows.Forms.NotifyIcon(this.components);

            // Set the icon for the systray.
			ntfyIcon.Icon = (Icon)NSA4Dummies.Properties.Resources.NotifyIconTest;
            ntfyIcon.ContextMenu = this.niContextMenu;
            ntfyIcon.Text = "NSA 4 Dummies";
            ntfyIcon.Visible = true;
            ntfyIcon.BalloonTipTitle = "NSA 4 Dummies";
            ntfyIcon.BalloonTipText = "Testtext Testtext Testtext";

            // Event for DoubleClick on NotifyIcon.
            // ntfyIcon.DoubleClick += new System.EventHandler(this.ntfyIcon_DoubleClick);

			
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
        
        private void Application_Startup(object Sender, StartupEventArgs e)
        {
            //base.OnStartup(e);
            if (languageFileMissing)
            {
                

                this.Shutdown();
            }
        
        }

	}
}
