using System;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;

namespace NSA4Dummies
{
    public partial class WorldMap : INotifyPropertyChanged
    {
        private static IDictionary<string, uint> _mapDictionary = new Dictionary<string, uint>();
        private static IDictionary<string, double> _shadingDictionary = new Dictionary<string, double>();
        private static IDictionary<string, Path> _pathDictionary = new Dictionary<string, Path>();
        private static SolidColorBrush defaultShading = new SolidColorBrush(Color.FromArgb(255, 255, 175, 102));
        private static SolidColorBrush mouseOverShading = new SolidColorBrush(Color.FromArgb(255, 255, 121, 0));


        /*
         *  Events
         * */
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// This function collects references to all Paths which resemble a country on the worldmap (Name has prefix 'Map')
        /// This function is called as soon as the map gets loaded.
        /// This happens every time the map gets visual.
        /// This function also performs updateCountries() if there were any packages before Map_OnLoaded
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">arguments for the event</param>
        private void Map_OnLoaded(object sender, EventArgs e)
        {
            foreach (var p in FindVisualChildren<Path>(((MainWindow)System.Windows.Application.Current.MainWindow).mainGrid))
            {
                if (p.Name.ToUpper().StartsWith("MAP"))
                {
                    _pathDictionary[cleanCountryCode(p.Name).ToUpper()] = p;
                }
            }

            updateCountries();
        }


        /// <summary>
        /// This functions fills (shades) the country on which the mouse is over with the current 'mouseOverShading'
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">arguments for the event</param>
        private void Map_Country_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var country = sender as Path;

            country.Fill = mouseOverShading;
        }


        /// <summary>
        /// This function is called whenever the mouse leaves a country on the map.
        /// This function will fill (shade) the country back to it's former color.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">arguments for the event</param>
        private void Map_Country_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var country = sender as Path;

            updateCountry(country.Name);
        }


        /// <summary>
        /// This function increments the package-counter for a specific country.
        /// This function does NOT update the country or the map filling!
        /// </summary>
        /// <param name="IPAddress">The IP address string of the country</param>
        public static void addPackageToCountry(string IPAddress)
        {
            if (IPAddress != null)
            {
                uint number = IP2Country.address2Number(IPAddress);
                string country = IP2Country.number2Country(number).ToUpper();
                if (country != null)
                {
                    if (_mapDictionary.ContainsKey(country))
                    {
                        _mapDictionary[country] = _mapDictionary[country] + 1;
                    }
                    else
                    {
                        _mapDictionary[country] = 1;
                    }
                }
            }
        }


        /// <summary>
        /// This function increments the package-counter for a specific country.
        /// This function DOES update the country filling!
        /// </summary>
        /// <param name="IPAddress">The IP address string of the country</param>
        public static void addPackageToCountryUpdate(string IPAddress)
        {
            if (IPAddress != null)
            {
                uint number = IP2Country.address2Number(IPAddress);
                string country = IP2Country.number2Country(number).ToUpper();
                if (country != null)
                {
                    if (_mapDictionary.ContainsKey(country))
                    {
                        _mapDictionary[country] = _mapDictionary[country] + 1;
                    }
                    else
                    {
                        _mapDictionary[country] = 1;
                    }

                    updateCountry(country);
                }
            }
        }


        /// <summary>
        /// This function sets the package counter of a specific country to a chosen value.
        /// This function does NOT update the country or the map filling!
        /// </summary>
        /// <param name="countryCode">The two-letter country code</param>
        /// <param name="value"></param>
        public void setData(string countryCode, uint value)
        {
            if (countryCode != null)
            {
                countryCode = cleanCountryCode(countryCode).ToUpper();
                _mapDictionary[countryCode] = value;
            }
        }


        /// <summary>
        /// This function returns the package count for a specific country
        /// By default 0 is returned.
        /// </summary>
        /// <param name="countryCode">The two-letter country code</param>
        /// <returns>The package count</returns>
        public uint getData(string countryCode)
        {
            if (countryCode != null)
            {
                countryCode = cleanCountryCode(countryCode).ToUpper();
                if (_mapDictionary.ContainsKey(countryCode))
                {
                    return _mapDictionary[countryCode];
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// This function updates the filling of a specific country using the _shadingDictionary
        /// If the country does not exist in the _shadingDictionary the current default shading is used.
        /// </summary>
        /// <param name="countryCode">The two-letter country code</param>
        public static void updateCountry(string countryCode)
        {
            if (countryCode != null)
            {
                countryCode = cleanCountryCode(countryCode).ToUpper();

                if (_shadingDictionary.ContainsKey(countryCode) && _pathDictionary.ContainsKey(countryCode))
                {
                    // Sets shading color for countries on worldmap when receiving/sending packages from/to country
                    SolidColorBrush packageShading = new SolidColorBrush(Colors.SkyBlue);

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
        }


        /// <summary>
        /// This function updates the fillings of all countries.
        /// </summary>
        public static void updateCountries()
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

            // Shade countries
            foreach (var e in _mapDictionary)
            {
                double opacity = (e.Value - minVal) / (maxVal - minVal);

                // Minimum opacity is 33% for better visibility
                if (opacity < 0.33  )
                {
                    opacity = 0.33;
                }

                _shadingDictionary[e.Key] = opacity;
            }


            foreach (var p in FindVisualChildren<Path>(((MainWindow)System.Windows.Application.Current.MainWindow).mainGrid))
            {
                if (p.Name != null)
                {
                    string name = cleanCountryCode(p.Name).ToUpper();

                    if (_shadingDictionary.ContainsKey(name))
                    {
                        SolidColorBrush packageShading = new SolidColorBrush(Colors.SkyBlue);

                        packageShading.Opacity = _shadingDictionary[name];
                        p.Fill = packageShading;
                    }
                }
            }
        }


        /// <summary>
        /// This function returns a clean two-letter country code (without the 'Map' prefix)
        /// If the passed string has no 'Map' prefix (case insensitive), the first two letters are returned (in uppercase).
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Two letter country code, or first to letters if no 'Map' prefix</returns>
        public static string cleanCountryCode(string code)
        {
            if (code != null)
            {
                if (code.ToUpper().StartsWith("MAP"))
                {
                    // Remove 'Map' prefix
                    code = code.Substring(3);
                }
                return code.Substring(0, 2);
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// This function searches for a visual children of a given type.
        /// This function is used to find the map tiles to get a reference to them.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
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
