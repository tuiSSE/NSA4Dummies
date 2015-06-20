using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace NSA4Dummies
{
    /// <summary>
    /// Class for handling language files
    /// </summary>
	public static class LanguageFile
	{
        /// <summary>
        /// Substruct to keep track of languages
        /// </summary>
		public struct Language
		{
            /// <summary>
            /// Full name of the language e.g. English
            /// </summary>
			public string name;
            /// <summary>
            /// Language abbreviation e.g. en
            /// </summary>
			public string sName;

			public override string ToString()
			{

				return this.name + " - " + this.sName;
			}
		}

		private static Language[] languages = null;

        /// <summary>
        /// Gets all languages found in language folder of the application
        /// </summary>
        /// <returns>Returns an array of type LanguageFile.Language containing all found languages or null if no language could be found</returns>
		public static Language[] GetLanguages()
		{
			if (null == languages)
			{
				string[] files = Directory.GetFiles(@"language\");
				
				if (!(0 == files.Length))
				{
					languages = new Language[files.Length];
					XmlDocument doc = new XmlDocument();
					for (int i = 0; i < files.Length; i++)
					{
						doc.Load(@files[i]);

						languages[i] = DetermineLanguage(doc, files[i]);
					}
				}
			}
			
			

			return languages;

		}

        /// <summary>
        /// Parses a languagefile and returns it as a Dictonary<string,string>
        /// </summary>
        /// <param name="language">The abbrevation of the language. This must match the language-files name e.g. for a file called en.lan use en</param>
        /// <returns>Returns a Dictonary<string,string> containing all defined keys and the corresponding strings</returns>
		public static Dictionary<string, string> GetTranslation(string language)
		{
			XmlDocument doc = new XmlDocument();
			Dictionary<string,string> translation = new Dictionary<string,string>();

			string path = "language/" + language + ".lan";

			if (File.Exists(path))
			{
				doc.Load(@path);
			}
			else
			{
				path = "language/en.lan";
				doc.Load(@path);
			}



			App.CurrentLanguage = DetermineLanguage(doc, path);

			try
			{
				foreach (XmlNode category in doc.DocumentElement.ChildNodes)
				{
                    foreach (XmlNode str in category.ChildNodes)
                    {
                        string key = category.Attributes["name"].Value + "." + str.Attributes["id"].Value;
                        string value = str.InnerText;
                        translation.Add(key, value);
                    }
					
				}
			}
			catch (Exception e)
			{
				throw new Exception("Error - " + path + " : " + e.Message);
			}

			return translation;
		}

        /// <summary>
        /// Constructs a LanguageFile.Language object from a given file and path
        /// </summary>
        /// <param name="doc">The xml document containing the language</param>
        /// <param name="path">The name of the language file</param>
        /// <returns></returns>
		private static Language DetermineLanguage(XmlDocument doc, string path){

			Language lan;
			lan.name = doc.DocumentElement.Attributes["lan"].Value;
			string patten = "([a-z]{2,3})\\.lan$";

			lan.sName = Regex.Match(path, patten).Groups[1].Value;

			return lan;

		}
	}
}
