using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSA4Dummies{

    public static class IP2Country
    {
        private static IPLookupLINQDataContext db = new IPLookupLINQDataContext();
        private static IDictionary<uint, string> cacheDict = new Dictionary<uint, string>();

        /// <summary>
        /// This function converts a IPv4 address string (e.g. "173.194.113.23") to it's numerical representation
        /// </summary>
        /// <param name="address">The IP address as a string; format: "xxx.xxx.xxx.xxx"</param>
        /// <returns>The numerical representation of the IP as a uint</returns>
        public static uint address2Number(string address)
        {
            if (address != null)
            {
                char[] splitchar = { '.' };
                string[] subnets = address.Split(splitchar);
                uint s1, s2, s3, s4;
                uint number = 0;

                if (subnets.Length == 4)
                {
                    uint.TryParse(subnets[0], out s1);
                    uint.TryParse(subnets[1], out s2);
                    uint.TryParse(subnets[2], out s3);
                    uint.TryParse(subnets[3], out s4);

                    if (s1 <= 255 && s2 <= 255 && s3 <= 255 && s4 <= 255)
                    {
                        number = s1 * 16777216 + s2 * 65536 + s3 * 256 + s4;
                    }
                }

                return number;
            }
            else
            {
                return 0;
            }
        }

        
        /// <summary>
        /// This function returns the corresponding two-letter country code to a given IP number.
        /// To get the country code the function looks up the IP address number in the database 'IPLookupLINQ'.
        /// Initially this can take some time!
        /// The function implements a cache, so future request will be much faster.
        /// </summary>
        /// <param name="number">The IP number for which the country code should be searched.</param>
        /// <returns>The two letter country code (in uppercase)</returns>
        public static string number2Country(uint number)
        {
            string country = "";

            if (cacheDict.ContainsKey(number))
            {
                return cacheDict[number];
            }

            var query = from c in db.IPLookupTable
                        where c.startAddress <= number
                            && c.endAddress >= number
                        select c;

            // Default value for string is NULL, so we will test, if a result exists
            if (query.Count() >= 1)
            {
                country = query.FirstOrDefault().countryShort;
            }

            cacheDict[number] = country;

            return country.ToUpper();
        }

    }
}
