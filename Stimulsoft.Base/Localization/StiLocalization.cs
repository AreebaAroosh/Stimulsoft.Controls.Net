#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{																	}
{	Copyright (C) 2003-2016 Stimulsoft     							}
{	ALL RIGHTS RESERVED												}
{																	}
{	The entire contents of this file is protected by U.S. and		}
{	International Copyright Laws. Unauthorized reproduction,		}
{	reverse-engineering, and distribution of all or any portion of	}
{	the code contained in this file is strictly prohibited and may	}
{	result in severe civil and criminal penalties and will be		}
{	prosecuted to the maximum extent possible under the law.		}
{																	}
{	RESTRICTIONS													}
{																	}
{	THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES			}
{	ARE CONFIDENTIAL AND PROPRIETARY								}
{	TRADE SECRETS OF Stimulsoft										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Collections;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.Localization
{          
	public class StiLocalization
	{
		#region Fields
		private static Hashtable categories = null;
        private static bool isLocalizationLoaded = false;
        private static string prevLocalization = string.Empty;
		#endregion

		#region Properties
		private static bool searchLocalizationFromRegistry = true;
		public static bool SearchLocalizationFromRegistry
		{
			get
			{
				return searchLocalizationFromRegistry;
			}
			set
			{
				searchLocalizationFromRegistry = value;
			}
		}


		private static string directoryLocalization = "Localization";
		/// <summary>
		/// Gets or sets string that containing path to directory in which are located files with localized resource.
		/// </summary>
		public static string DirectoryLocalization
		{
			get
			{
				return directoryLocalization;
			}
			set
			{
				directoryLocalization = value;
			}
		}

		
		private static string localization = string.Empty;
		/// <summary>
		/// Gets or sets name of file with localized resource.
		/// </summary>
		public static string Localization
		{
			get
			{
				return localization;
			}
			set
			{
				localization = value;
			}
		}


		private static string language;
		/// <summary>
		/// Gets or sets name of the language  of the localization.
		/// </summary>
		public static string Language
		{
			get
			{
				return language;
			}
			set
			{
				language = value;
			}
		}


		private static string description;
		/// <summary>
		/// Gets or sets description of the localization.
		/// </summary>
		public static string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}


		private static string cultureName;
		/// <summary>
		/// Gets or sets the name of the culture of the localization.
		/// </summary>
		public static string CultureName
		{
			get
			{
				return cultureName;
			}
			set
			{
				cultureName = value;
			}
		}


        private static bool blockLocalizationExceptions = false;
        /// <summary>
        /// Gets or sets the name of the culture of the localization.
        /// </summary>
        public static bool BlockLocalizationExceptions
        {
            get
            {
                return blockLocalizationExceptions;
            }
            set
            {
                blockLocalizationExceptions = value;
            }
        }

        private static bool blockLocalizationLoading = false;
        public static bool BlockLocalizationLoading
        {
            get
            {
                return blockLocalizationLoading;
            }
            set
            {
                blockLocalizationLoading = value;
            }
        }
		#endregion

		#region Methods
		public static string GetDirectoryLocalization()
		{
			//Get directory from registry
			if ((!Directory.Exists(StiLocalization.DirectoryLocalization)) && SearchLocalizationFromRegistry)
			{
                 string dirPath = Registry.GetValue("Localization");
                 if (dirPath != null)
                 {
                     if (dirPath != null && Directory.Exists(dirPath))return dirPath;
                 }
			}
			return null;
		}

        public static string GetEnumValue(string key)
        {
            if (categories == null) LoadDefaultLocalization();
            if (categories == null)
                return null;

            var categoryHash = categories["PropertyEnum"] as Hashtable;
            if (categoryHash == null)
                return null;

            var value = categoryHash[key] as string;
            if (value == null)
            {
                return (key.EndsWith("Category")) ? key.Substring(0, key.Length - 8) : null;
            }

            return value;
        }

		public static void LoadDefaultLocalization()
		{
			Stream stream = null;
			try
			{
				stream = typeof(StiLocalization).Assembly.GetManifestResourceStream("Stimulsoft.Base.Localization.en.xml");
				if (stream == null)throw new Exception(string.Format("Resourse '{0}' is not founded!", 
									   "Stimulsoft.Base.Localization.en.xml"));
				LoadInternal(stream);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
                    stream.Dispose();
					stream = null;
				}
			}
		}

        private static DirectoryInfo GetDirectoryInfoFromRegister()
        {
            if (!SearchLocalizationFromRegistry) return null;

            string dirPath = Registry.GetValue("Localization");
            if (dirPath != null)
            {
                if (dirPath != null && Directory.Exists(dirPath)) return new DirectoryInfo(dirPath);
            }
            
            return null;
        }

        private static void Check()
        {
            if (isLocalizationLoaded && prevLocalization == Localization)
                return;
            isLocalizationLoaded = true;
            prevLocalization = Localization;
            LoadCurrentLocalization();
        }

		/// <summary>
		/// Loads the current localization.
		/// </summary>
		public static void LoadCurrentLocalization()
		{
            if (BlockLocalizationLoading) return;

			DirectoryInfo di = null;
			if (Directory.Exists(DirectoryLocalization))di = new DirectoryInfo(DirectoryLocalization);

			//Get directory from registry
            if (di == null || (!di.Exists))
            {
                di = GetDirectoryInfoFromRegister();
            }			
				

			if (di != null && di.Exists)
			{
				FileInfo[] files = di.GetFiles();
			
				if (Localization.Length > 0)
				{
					foreach (FileInfo file in files)
					{
						if (file.Name == Localization)
						{
							StiLocalization.Load(file.FullName);
							return;
						}
					}
				}
				else
				{
					string cultureName = CultureInfo.CurrentCulture.Name;
					string cultureISOName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

					foreach (FileInfo file in files)
					{
						string fileName = Path.GetFileNameWithoutExtension(file.Name);
					
						if (fileName == cultureName || fileName == cultureISOName)
						{
							StiLocalization.Load(file.FullName);
							return;
						}
					}
				}
			}
            StiLocalization.LoadDefaultLocalization();
		}


		/// <summary>
		/// Loads a localization from the file.
		/// </summary>
		/// <param name="file">The name of the file to load a localization.</param>
		public static void Load(string file)
		{
			FileStream stream = null;
			try
			{
				stream = new FileStream(file, FileMode.Open, FileAccess.Read);
				Load(stream);
			}
			finally
			{
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
			}
		}


		/// <summary>
		/// Loads a localization from the stream.
		/// </summary>
		public static void Load(Stream stream)
		{
            if (BlockLocalizationLoading) return;

			if (categories != null)
			{
				categories.Clear();
				categories = null;
			}

			LoadDefaultLocalization();
			LoadInternal(stream);
		}


		private static void LoadInternal(Stream stream)
		{
            if (BlockLocalizationLoading) return;

			XmlTextReader tr = new XmlTextReader(stream);

			tr.Read();
			tr.Read();
			tr.Read();
			if (tr.Name == "Localization")
			{
				language = tr.GetAttribute("language");
				description = tr.GetAttribute("description");
				cultureName = tr.GetAttribute("cultureName");

				string category = string.Empty;
					
				while (tr.Read())
				{
					if (tr.IsStartElement()) 
					{
						string name = tr.Name;
						if (tr.Depth == 1)
						{
							category = name;
						}
						else
						{
							Add(category, name, tr.ReadString());
						}							
					}
					tr.Read();
				}
			}
		}


		/// <summary>
		/// Loads localization parameters from the file.
		/// </summary>
		/// <param name="file">The name of the file of a localization.</param>
		public static void GetParam(string file, out string language, out string description, out string cultureName)
		{
			if (File.Exists(file))
			{
				FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
				XmlTextReader tr = new XmlTextReader(stream);

				tr.Read();
				tr.Read();
				tr.Read();

				language = tr.GetAttribute("language");
				description = tr.GetAttribute("description");
				cultureName = tr.GetAttribute("cultureName");
	
				stream.Close();
                stream.Dispose();
			}
			else
			{
				language = "Default";
				description = "English";
				cultureName = "en";
			}
		}

	    /// <summary>
	    /// Loads localization parameters from the file.
	    /// </summary>
	    /// <param name="file">The name of the file of a localization.</param>
	    public static void GetParam(Stream stream, out string language, out string description, out string cultureName)
	    {
	        XmlTextReader tr = new XmlTextReader(stream);

	        tr.Read();
	        tr.Read();
	        tr.Read();

	        language = tr.GetAttribute("language");
	        description = tr.GetAttribute("description");
	        cultureName = tr.GetAttribute("cultureName");
	    }

	    private static object lockStaticArrays = new object();

		public static void Add(string category, string key, string value)
		{
            Check();

            lock (lockStaticArrays)
            {
                if (categories == null) categories = new Hashtable();

                Hashtable categoryHash = categories[category] as Hashtable;
                if (categoryHash == null)
                {
                    categoryHash = new Hashtable();
                    categories[category] = categoryHash;
                }

                categoryHash[key] = value;
            }
		}


		public static string Get(string category, string key)
		{
            return Get(category, key, true);
		}

		public static string GetValue(string category, string key)
		{
			return Get(category, key);
		}


		public static string Get(string category, string key, bool throwError)
		{
            Check();

            bool baseThrowError = throwError;
            if (BlockLocalizationExceptions) throwError = false;
   
			if (categories == null) LoadDefaultLocalization();
			if (categories == null)
			{
				if (throwError)throw new Exception("Localization file is not loaded!");
				return null;
			}

			Hashtable categoryHash = categories[category] as Hashtable;
			if (categoryHash == null)
			{
                if (baseThrowError)
                {
                    LoadDefaultLocalization();
                    categoryHash = categories[category] as Hashtable;
                }
                if (categoryHash == null)
                {
                    if (throwError) throw new Exception(string.Format("Category '{0}' is not found!", category));
                    return null;
                }
			}

			string value = categoryHash[key] as string;
            if (value == null)
            {
                if (key.EndsWith("Category"))
                {
                    return key.Substring(0, key.Length - 8);
                }
                if (baseThrowError)
                {
                    LoadDefaultLocalization();
                    value = categoryHash[key] as string;
                }
                if (value == null)
                {
                    if (throwError) throw new Exception(string.Format("Key '{0}' is not found!", key));
                    return null;
                }
            }
			
			return value;
		}


        public static void Set(string category, string key, string value)
        {
            Check();

            bool throwError = true;
            if (categories == null) LoadDefaultLocalization();
            if (categories == null)
            {
                if (throwError) throw new Exception("Localization file is not loaded!");
            }

            Hashtable categoryHash = categories[category] as Hashtable;
            if (categoryHash == null)
            {
                if (throwError) throw new Exception(string.Format("Category '{0}' is not found!", category));
            }

            lock (lockStaticArrays)
            {
                categoryHash[key] = value;
            }
        }


		public static string[] GetKeys(string category)
		{
            Check();

			if (categories == null)
			{
				throw new Exception("Localization file is not loaded!");
			}

			Hashtable categoryHash = categories[category] as Hashtable;
			if (categoryHash == null)
			{
				throw new Exception(string.Format("Category '{0}' is not found!", category));
			}

            lock (lockStaticArrays)
            {
                string[] keys = new string[categoryHash.Keys.Count];
                categoryHash.Keys.CopyTo(keys, 0);


                Array.Sort(keys);

                return keys;
            }
		}


		public static string[] GetValues(string category)
		{
            Check();

			if (categories == null)
			{
				throw new Exception("Localization file is not loaded!");
			}

			Hashtable categoryHash = categories[category] as Hashtable;
			if (categoryHash == null)
			{
				throw new Exception(string.Format("Category '{0}' is not found!", category));
			}

            lock (lockStaticArrays)
            {
                string[] values = new string[categoryHash.Values.Count];
                categoryHash.Values.CopyTo(values, 0);


                Array.Sort(values);

                return values;
            }
		}

        public static string[] GetCategories()
        {
            Check();

            if (categories == null)
            {
                throw new Exception("Localization file is not loaded!");
            }

            string[] keys = new string[categories.Count];
            categories.Keys.CopyTo(keys, 0);

            return keys;
        }

        public static string GetLocalization(bool format)
        {
            if (categories == null) LoadDefaultLocalization();
            return JsonConvert.SerializeObject(categories, format ? Json.Formatting.Indented : Json.Formatting.None);
        }
		#endregion
	}
}