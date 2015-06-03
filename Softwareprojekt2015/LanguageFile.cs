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
	public static class LanguageFile
	{

		public struct Language
		{
			public string name;
			public string sName;

			public override string ToString()
			{

				return this.name + " - " + this.sName;
			}
		}

		public static Language[] GetLanguages()
		{
			string[] files = Directory.GetFiles(@"language\");
			Language[] lans = new Language[files.Length];
			XmlDocument doc = new XmlDocument();
			for (int i = 0; i < files.Length; i++)
			{
				doc.Load(@files[i]);

				lans[i] = DetermineLanguage(doc, files[i]);
			}

			return lans;

		}

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

		private static Language DetermineLanguage(XmlDocument doc, string path){

			Language lan;
			lan.name = doc.DocumentElement.Attributes["lan"].Value;
			string patten = "([a-z]{2,3})\\.lan$";

			lan.sName = Regex.Match(path, patten).Groups[1].Value;

			return lan;

		}
	}
}
