using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softwareprojekt2015{

    public static class IP2Country
    {
        static IPLookupLINQDataContext db = new IPLookupLINQDataContext();
        static IDictionary<uint, string> cacheDict = new Dictionary<uint, string>();

        public static uint adress2Numer(string adress)
        {
            char[] splitchar = { '.' };
            string[] subnets = adress.Split(splitchar);
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

            if (query.Count() >= 1)
            {
                // Default value for striing is NULL, so we will test, if a result exists
                country = query.FirstOrDefault().countryShort;
            }

            cacheDict[number] = country;

            return country;
        }

    }
}
