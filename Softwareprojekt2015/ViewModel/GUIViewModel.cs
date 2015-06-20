using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NSA4Dummies
{
    public class GUIViewModel : INotifyPropertyChanged
    {

        public struct ChartType
        {
            public ChartType(string title, string key)
            {
                this.title = title;
                this.key = key;
            }

            private string title;
            private string key;

            public string Title
            {
                get
                {
                    return title;
                }
                set
                {
                    title = value;
                }
            }
            public string Key
            {
                get
                {
                    return key;
                }
                set
                {
                    key = value;
                }
            }

            public override string ToString()
            {
                return Title;
            }
        }

        /*
         *  Public members
         * */
        public List<double> FontSizes { get; set; }
        public List<double> DoughnutInnerRadiusRatios { get; set; }
        public List<string> SelectionBrushes { get; set; }
        public ObservableCollectionEx<ChartType> ViewTypes { get; set; }
        public Dictionary<string, De.TorstenMandelkow.MetroChart.ResourceDictionaryCollection> Palettes { get; set; }


        /*
         *  Chart-Data dictionaries
         * */
        private Dictionary<string, uint> FileTypes = new Dictionary<string, uint>();
        private Dictionary<string, uint> Domains = new Dictionary<string, uint>();
        private uint[] Encryption = new uint[2];

        
        /// <summary>
        /// This function adds a package to the filetype-chart with the specific filetype
        /// </summary>
        /// <param name="fileType">The filetype string</param>
        public void addFileType(string fileType)
        {

            fileType = fileType.ToUpper();
            
            // Insert data to directionary
            if (FileTypes.ContainsKey(fileType))
            {
                FileTypes[fileType] += 1;
            }
            else
            {
                FileTypes.Add(fileType, 1);
            }

            // Update chart
            // Filetypes.Add(new TestClass() { Category = fileType, Number = FileTypes[fileType] });
        }


        /// <summary>
        /// This function adds a package to the domain-chart with the specific domain
        /// </summary>
        /// <param name="domain">The domain</param>
        public void addDomain(string domain)
        {

            domain = domain.ToLower();

            // Insert data to directionary
            if (Domains.ContainsKey(domain))
            {
                Domains[domain] += 1;
            }
            else
            {
                Domains.Add(domain, 1);
            }

            // Update chart
            //TopWebsites.Add(new TestClass() { Category = domain, Number = Domains[domain] });
        }


        /// <summary>
        /// Adds a package to the charts
        /// </summary>
        /// <param name="encrypted">The encryption status of that package</param>
        public void addPackage(bool encrypted)
        {
            
            if (encrypted)
            {
                Encryption[0] += 1;
            }
            else
            {
                Encryption[1] += 1;
            }

            // Update chart
            //EncryptionStatus.Add(new TestClass() { Category = "Unencrypted", Number = Encryption[1] });
        }


        /// <summary>
        /// This function is called whenever graphs shall be updated
        /// </summary>
        public void updateDataGraphs()
        {
            Filetypes.Clear();
            TopWebsites.Clear();
            EncryptionStatus.Clear();

            foreach (var d in Domains)
            {
                TopWebsites.Add(new DataClass() { Category = d.Key, Number = d.Value });
            }

            foreach (var f in FileTypes)
            {
                Filetypes.Add(new DataClass() { Category = f.Key, Number = f.Value });
            }

            int enc = 0;

            if (Encryption[1] + Encryption[0] != 0)
            {
                enc = (int)(((float)Encryption[1] / ((float)Encryption[1] + (float)Encryption[0])) * 100);
            }

            EncryptionStatus.Add(new DataClass() { Category = "Unencrypted", Number = enc });
        }


        /// <summary>
        /// The dictionary that holds the translations for the whole program
        /// </summary>
        public static Dictionary<string, string> T
        {
            get
            {
                return App.translation;
            }
        }
       

        /*
         *  selected layout elements
         * */
        private ChartType selectedChartType;
        public ChartType SelectedChartType
        {
            get
            {
                return selectedChartType;
            }
            set
            {
                selectedChartType = value;
                NotifyPropertyChanged("SelectedChartType");
            }
        }

        private object selectedPalette = null;
        public object SelectedPalette
        {
            get
            {
                return selectedPalette;
            }
            set
            {
                selectedPalette = value;
                NotifyPropertyChanged("SelectedPalette");
            }
        }

        private string selectedBrush = null;
        public string SelectedBrush
        {
            get
            {
                return selectedBrush;
            }
            set
            {
                selectedBrush = value;
                NotifyPropertyChanged("SelectedBrush");
            }
        }

        private double selectedDoughnutInnerRadiusRatio = 0.75;
        public double SelectedDoughnutInnerRadiusRatio
        {
            get
            {
                return selectedDoughnutInnerRadiusRatio;
            }
            set
            {
                selectedDoughnutInnerRadiusRatio = value;
                NotifyPropertyChanged("SelectedDoughnutInnerRadiusRatio");
                NotifyPropertyChanged("SelectedDoughnutInnerRadiusRatioString");
            }
        }

        public string SelectedFontSizeString
        {
            get
            {
                return SelectedFontSize.ToString() + "px";
            }
        }

        private object selectedItem = null;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        private double fontSize = 11.0;
        public double SelectedFontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
                NotifyPropertyChanged("SelectedFontSize");
                NotifyPropertyChanged("SelectedFontSizeString");
            }
        }

        private bool isRowColumnSwitched = false;
        public bool IsRowColumnSwitched
        {
            get
            {
                return isRowColumnSwitched;
            }
            set
            {
                isRowColumnSwitched = value;
                NotifyPropertyChanged("IsRowColumnSwitched");
            }
        }

        private bool isLegendVisible = true;
        public bool IsLegendVisible
        {
            get
            {
                return isLegendVisible;
            }
            set
            {
                isLegendVisible = value;
                NotifyPropertyChanged("IsLegendVisible");
            }
        }

        private bool isTitleVisible = true;
        public bool IsTitleVisible
        {
            get
            {
                return isTitleVisible;
            }
            set
            {
                isTitleVisible = value;
                NotifyPropertyChanged("IsTitleVisible");
            }
        }

        public string SelectedDoughnutInnerRadiusRatioString
        {
            get
            {
                return String.Format("{0:P1}.", SelectedDoughnutInnerRadiusRatio);
            }
        }


        
        /*----------
         * 
         *  Layout
         * 
         * --------*/

        /*
         *  Layouts
         * */
        private bool hackerLayout = false;
        public bool HackerLayout
        {
            get
            {
                return hackerLayout;
            }
            set
            {
                hackerLayout = value;
                NotifyPropertyChanged("DarkLayout");
                NotifyPropertyChanged("Foreground");
                NotifyPropertyChanged("Background");
                NotifyPropertyChanged("MainBackground");
                NotifyPropertyChanged("MainForeground");
                NotifyPropertyChanged("MapDefaultColor");
            }
        }


        /*
         *  Get/Set of layout elements
         * */
        /// <summary>
        /// Color of Charts-Text
        /// </summary>
        public string Foreground
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF00ff24";
                }
                //default
                return "#FF000000";
            }
        }
        /// <summary>
        /// Color of Header texts
        /// </summary>
        public string MainForeground
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF00ff24";
                }
                //default
                return "#FFFA611F";
            }
        }
        /// <summary>
        /// The background color
        /// </summary>
        public string Background
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF333333";
                }
                // default
                return "#FF9AC9F8";
            }
        }
        /// <summary>
        /// The main background color of the program
        /// </summary>
        public string MainBackground
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF000000";
                }
                // default
                return "#FF1e1e1e";
            }
        }
        /// <summary>
        /// Default color of the countries
        /// </summary>
        public string MapDefaultColor
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF38FF62";
                }
                // default
                return "#FFFFAF66";
            }
        }



        /*
         *  Events
         * */
        public event PropertyChangedEventHandler PropertyChanged;

        public string ToolTipFormat
        {
            get
            {
                return "{0} in series '{2}' has value '{1}' ({3:P2})";
            }
        }

        /*
         *  Private methods
         * */
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }

        private void LoadPalettes()
        {
            Palettes = new Dictionary<string, De.TorstenMandelkow.MetroChart.ResourceDictionaryCollection>();
            Palettes.Add("Default", null);

            var resources = Application.Current.Resources.MergedDictionaries.ToList();
            foreach (var dict in resources)
            {
                foreach (var objkey in dict.Keys)
                {
                    if (dict[objkey] is De.TorstenMandelkow.MetroChart.ResourceDictionaryCollection)
                    {
                        Palettes.Add(objkey.ToString(), dict[objkey] as De.TorstenMandelkow.MetroChart.ResourceDictionaryCollection);
                    }
                }
            }

            SelectedPalette = Palettes.FirstOrDefault();
        }

        int newSeriesCounter = 1;
        private void AddSeries()
        {
            ObservableCollectionEx<DataClass> data = new ObservableCollectionEx<DataClass>();

            data.Add(new DataClass() { Category = "Globalization", Number = 5 });
            data.Add(new DataClass() { Category = "Features", Number = 10 });
            data.Add(new DataClass() { Category = "ContentTypes", Number = 15 });
            data.Add(new DataClass() { Category = "Correctness", Number = 20 });
            data.Add(new DataClass() { Category = "Naming", Number = 15 });
            data.Add(new DataClass() { Category = "Best Practices", Number = 10 });

            newSeriesCounter++;
        }

        /// <summary>
        /// The constructor of GUIViewModel
        /// </summary>
        public GUIViewModel()
        {
            LoadPalettes();

            //AddSeriesCommand = new DelegateCommand(x => AddSeries());

            ViewTypes = new ObservableCollectionEx<ChartType>();
            ViewTypes.Add(new ChartType(App.translation["mainWindow.map"], "Map"));
            ViewTypes.Add(new ChartType(App.translation["mainWindow.stats"], "Statistics"));
            ViewTypes.Add(new ChartType(App.translation["mainWindow.settings"], "Settings"));
            ViewTypes.Add(new ChartType(App.translation["mainWindow.credits"], "Credits"));
            SelectedChartType = ViewTypes.FirstOrDefault();
			

            FontSizes = new List<double>();
            FontSizes.Add(8.0);
            FontSizes.Add(9.0);
            FontSizes.Add(10.0);
            FontSizes.Add(11.0);
            FontSizes.Add(12.0);
            FontSizes.Add(13.0);
            FontSizes.Add(18.0);
            SelectedFontSize = 12.0;

            DoughnutInnerRadiusRatios = new List<double>();
            DoughnutInnerRadiusRatios.Add(0.90);
            DoughnutInnerRadiusRatios.Add(0.75);
            DoughnutInnerRadiusRatios.Add(0.5);
            DoughnutInnerRadiusRatios.Add(0.25);
            DoughnutInnerRadiusRatios.Add(0.1);
            SelectedDoughnutInnerRadiusRatio = 0.75;

            SelectionBrushes = new List<string>();
            SelectionBrushes.Add("Orange");
            SelectionBrushes.Add("Red");
            SelectionBrushes.Add("Yellow");
            SelectionBrushes.Add("Blue");
            SelectionBrushes.Add("[NoColor]");
            SelectedBrush = SelectionBrushes.FirstOrDefault();

            TopWebsites = new ObservableCollectionEx<DataClass>();
            EncryptionStatus = new ObservableCollectionEx<DataClass>();
            Filetypes = new ObservableCollectionEx<DataClass>();


            // Disable Notifications of ObservableCollections
            TopWebsites = TopWebsites.DisableNotifications();
            EncryptionStatus = EncryptionStatus.DisableNotifications();
            Filetypes = Filetypes.DisableNotifications();

            /*
            TopWebsites.Add(new TestClass() { Category = "facebook.com", Number = 75 });
            TopWebsites.Add(new TestClass() { Category = "google.com", Number = 2 });
            TopWebsites.Add(new TestClass() { Category = "youtube.com", Number = 12 });
            TopWebsites.Add(new TestClass() { Category = "gmail.com", Number = 83 });
            **/

            // EncryptionStatus.Add(new TestClass() { Category = "Unencrypted", Number = 83 });

            /*
            Filetypes.Add(new TestClass() { Category = "JPEG", Number = 83 });
            Filetypes.Add(new TestClass() { Category = "Mp3", Number = 15 });
            Filetypes.Add(new TestClass() { Category = "Gif", Number = 60 });
            Filetypes.Add(new TestClass() { Category = "PNG", Number = 47 });
            Filetypes.Add(new TestClass() { Category = "HTML", Number = 120 });
            Filetypes.Add(new TestClass() { Category = "CSS", Number = 122 });
            Filetypes.Add(new TestClass() { Category = "JS", Number = 89 });
            Filetypes.Add(new TestClass() { Category = "JS", Number = 90 });
            Filetypes.Add(new TestClass() { Category = "JS", Number = 91 });
            Filetypes.Add(new TestClass() { Category = "JS", Number = 20 });
             * */

        }

        /// <summary>
        /// The collection that holds the top websites
        /// </summary>
        public ObservableCollectionEx<DataClass> TopWebsites
        {
            get;
            set;
        }

        /// <summary>
        /// The collection that holds the (un-)encrypted packages
        /// </summary>
        public ObservableCollectionEx<DataClass> EncryptionStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The collection that holds the filetypes of the packages
        /// </summary>
        public ObservableCollectionEx<DataClass> Filetypes
        {
            get;
            set;
        }

    }


    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<object> execute,
                       Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }


    /*
    public class SeriesData
    {
        public string SeriesDisplayName { get; set; }

        public string SeriesDescription { get; set; }

        public ObservableCollection<TestClass> Items { get; set; }
    }
     * */


    /// <summary>
    /// The class that is used to store data to the charts
    /// </summary>
    public class DataClass : INotifyPropertyChanged
    {
        public string Category { get; set; }

        private float _number = 0;
        public float Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Number"));
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
