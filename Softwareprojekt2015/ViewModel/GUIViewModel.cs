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
        //public DelegateCommand AddSeriesCommand { get; set; }
        public List<double> FontSizes { get; set; }
        public List<double> DoughnutInnerRadiusRatios { get; set; }
        public List<string> SelectionBrushes { get; set; }
        public ObservableCollectionEx<ChartType> ViewTypes { get; set; }
        public Dictionary<string, De.TorstenMandelkow.MetroChart.ResourceDictionaryCollection> Palettes { get; set; }


        /*
         *  Chart-Data dictionaries
         * */
        private Dictionary<string, UInt64> FileTypes = new Dictionary<string, UInt64>();
        private Dictionary<string, UInt64> Domains = new Dictionary<string, UInt64>();
        private UInt64[] Encryption = new UInt64[2];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileType"></param>
        public void addFileType(string fileType)
        {

            fileType = fileType.ToUpper();
            
            // Isert data to directionary
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
        /// 
        /// </summary>
        /// <param name="domain"></param>
        public void addDomain(string domain)
        {

            domain = domain.ToLower();

            // Isert data to directionary
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
        /// 
        /// </summary>
        /// <param name="domain"></param>
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
        /// This function is called whenever the graphs shall be updated
        /// </summary>
        public void updateDataGraphs()
        {
            Filetypes.Clear();
            TopWebsites.Clear();
            EncryptionStatus.Clear();

            foreach (var d in Domains)
            {
                TopWebsites.Add(new TestClass() { Category = d.Key, Number = d.Value });
            }

            foreach (var f in FileTypes)
            {
                Filetypes.Add(new TestClass() { Category = f.Key, Number = f.Value });
            }

            int enc = 0;

            if (Encryption[1] + Encryption[0] != 0)
            {
                enc = (int)(((float)Encryption[1] / ((float)Encryption[1] + (float)Encryption[0])) * 100);
            }
            
            EncryptionStatus.Add(new TestClass() { Category = "Unencrypted", Number = enc });
        }


        /*
         *  Language dictionary
         * */
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

        /*
         *  Get/Set of layout elements
         * */
        public string Foreground
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF00ff24";
                }
                return "#FF006666";
            }
        }
        public string MainForeground
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF00ff24";
                }
                return "#FFFA611F";
            }
        }
        public string Background
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF333333";
                }
                return "#FF9AC9F8";
            }
        }
        public string MainBackground
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF000000";
                }
                return "#FF1E1E1E";
            }
        }
        public string MapDefaultColor
        {
            get
            {
                if (hackerLayout)
                {
                    return "#FF38FF62";
                }
                return "#FFFFAF66";
            }
        }

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
            ObservableCollectionEx<TestClass> data = new ObservableCollectionEx<TestClass>();

            data.Add(new TestClass() { Category = "Globalization", Number = 5 });
            data.Add(new TestClass() { Category = "Features", Number = 10 });
            data.Add(new TestClass() { Category = "ContentTypes", Number = 15 });
            data.Add(new TestClass() { Category = "Correctness", Number = 20 });
            data.Add(new TestClass() { Category = "Naming", Number = 15 });
            data.Add(new TestClass() { Category = "Best Practices", Number = 10 });

            newSeriesCounter++;
        }

        /*
         *  Constructor
         * */
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

            TopWebsites = new ObservableCollectionEx<TestClass>();
            EncryptionStatus = new ObservableCollectionEx<TestClass>();
            Filetypes = new ObservableCollectionEx<TestClass>();


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

        public ObservableCollectionEx<TestClass> TopWebsites
        {
            get;
            set;
        }

        public ObservableCollectionEx<TestClass> EncryptionStatus
        {
            get;
            set;
        }

        public ObservableCollectionEx<TestClass> Filetypes
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


    public class TestClass : INotifyPropertyChanged
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
