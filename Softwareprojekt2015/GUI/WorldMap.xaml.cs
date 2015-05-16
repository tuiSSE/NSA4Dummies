using System;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;

namespace Softwareprojekt2015
{
    public partial class WorldMap : INotifyPropertyChanged
    {

        private static IDictionary<string, UInt64> _mapDictionary = new Dictionary<string, UInt64>();
        private static IDictionary<string, double> _shadingDictionary = new Dictionary<string, double>();
        private static IDictionary<string, Path> _pathDictionary = new Dictionary<string, Path>();
        static SolidColorBrush defaultShading = new SolidColorBrush(Color.FromArgb(255, 255, 175, 102));
        static SolidColorBrush mouseOverShading = new SolidColorBrush(Color.FromArgb(255, 255, 121, 0));


        /*
         *  Events
         * */
        public event PropertyChangedEventHandler PropertyChanged;


        private string countryName = "-Country_Name-";
        public string CountryNameString
        {
            get
            {
                return countryName;
            }
            set
            {
                countryName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(App.Current.MainWindow.DataContext, new PropertyChangedEventArgs("CountryNameString"));
                }
            }

        }


        /*
         * 
         * */
        private void Map_OnLoaded(object sender, EventArgs e)
        {            
            foreach (var p in FindVisualChildren<Path>(((MainWindow)System.Windows.Application.Current.MainWindow).mainGrid))
            {
                _pathDictionary[cleanCountryCode(p.Name).ToUpper()] = p;
            }
        }


        /*
         * 
         * */
        private void Map_Country_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var country = sender as Path;

            country.Fill = mouseOverShading;
            CountryNameString = cleanCountryCode(country.Name);
        }


        /*
         * 
         * */
        private void Map_Country_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var country = sender as Path;

            updateCountry(country.Name);
            CountryNameString = "";
        }


        /*
         * 
         * */
        public void updateCountry(string countryCode)
        {
            countryCode = cleanCountryCode(countryCode).ToUpper();

            if (_shadingDictionary.ContainsKey(countryCode) && _pathDictionary.ContainsKey(countryCode))
            {
                SolidColorBrush packageShading = new SolidColorBrush(Colors.Blue);

                packageShading.Opacity = _shadingDictionary[countryCode];
                _pathDictionary[countryCode].Fill = packageShading;
            }
            else
            {
                if (_pathDictionary.ContainsKey(countryCode))
                {
                    _pathDictionary[countryCode].Fill = defaultShading;
                }
            }
        }

        /*
         * 
         * */
        public void addPackageToCountry(string IPAddress)
        {
            uint number = IP2Country.address2Number(IPAddress);
            string country = IP2Country.number2Country(number);
            if (_mapDictionary.ContainsKey(country))
            {
                _mapDictionary[country] = _mapDictionary[country] + 1;
            }
        }


        /*
         * 
         * */
        public void setData(string countryCode, UInt64 value)
        {
            countryCode = cleanCountryCode(countryCode).ToUpper();
            _mapDictionary[countryCode] = value;
        }


        /*
         * 
         * */
        public UInt64 getData(string countryCode)
        {
            countryCode = cleanCountryCode(countryCode).ToUpper();
            if (_mapDictionary.ContainsKey(countryCode))
            {
                return _mapDictionary[countryCode];
            }else{
                return 0;
            }
        }


        /*
         * 
         * */
        public void updateCountries()
        {
            double minVal = 0.0;
            double maxVal = 0.0;

            // Get min/max values
            foreach (var e in _mapDictionary)
            {
                if (e.Value < minVal)
                {
                    minVal = e.Value;
                }

                if (e.Value > maxVal)
                {
                    maxVal = e.Value;
                }
            }

            _shadingDictionary.Clear();

            // Shade countires
            foreach (var e in _mapDictionary)
            {
                double opacity = (e.Value - minVal) / (maxVal - minVal);

                // Minimum is 3%
                if (opacity < 0.010)
                {
                    opacity = 0.010;
                }

                _shadingDictionary[e.Key] = opacity;
            }


            foreach (var p in FindVisualChildren<Path>(((MainWindow)System.Windows.Application.Current.MainWindow).mainGrid))
            {
                string name = cleanCountryCode(p.Name).ToUpper();

                if (_shadingDictionary.ContainsKey(name))
                {
                    SolidColorBrush packageShading = new SolidColorBrush(Colors.Blue);

                    packageShading.Opacity = _shadingDictionary[name];
                    p.Fill = packageShading;
                }
            }
        }


        /*
         * 
         * */
        public string cleanCountryCode(string code)
        {
            if (code.ToUpper().StartsWith("MAP"))
            {
                // Remove 'Map' prefix
                code = code.Substring(3);
            }
            return code.Substring(0, 2);
        }


        /*
         * 
         * */
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            List<T> foundChilds = new List<T>();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                T childType = child as T;
                if (childType == null)
                {
                    foreach (var other in FindVisualChildren<T>(child))
                        yield return other;
                }
                else
                {
                    yield return (T)child;
                }
            }
        }

    }
}
