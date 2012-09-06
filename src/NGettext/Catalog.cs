using System;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Text;

using NGettext.Loaders;

namespace NGettext
{
	/// <summary>
	/// Represents a Gettext catalog instance.
	/// Loads translations from gettext *.mo files.
	/// </summary>
	public class Catalog : BaseCatalog
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Catalog"/> class with no translations and with current CultureInfo.
		/// </summary>
		public Catalog()
			: base(CultureInfo.CurrentUICulture)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Catalog"/> class with no translations and with given CultureInfo.
		/// </summary>
		/// <param name="cultureInfo">Culture info</param>
		public Catalog(CultureInfo cultureInfo)
			: base(cultureInfo)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Catalog"/> class with current CultureInfo
		/// and loads MO translations from given stream.
		/// </summary>
		/// <param name="moStream">Stream that contain binary data in the MO file format</param>
		public Catalog(Stream moStream)
			: this()
		{
			this.Load(moStream);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Catalog"/> class with given CultureInfo
		/// and loads MO translations from given stream.
		/// </summary>
		/// <param name="moStream">Stream that contain binary data in the MO file format</param>
		/// <param name="cultureInfo">Culture info</param>
		public Catalog(Stream moStream, CultureInfo cultureInfo)
			: this(cultureInfo)
		{
			this.Load(moStream);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Catalog"/> class with current CultureInfo
		/// and loads translations from MO file that can be found by given parameters.
		/// </summary>
		/// <param name="domain">Catalog domain name</param>
		/// <param name="localeDir">Directory that contains gettext localization files</param>
		public Catalog(string domain, string localeDir)
			: this()
		{
			this.Load(CultureInfo, domain, localeDir);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Catalog"/> class with given CultureInfo
		/// and loads translations from MO file that can be found by given parameters.
		/// </summary>
		/// <param name="domain">Catalog domain name</param>
		/// <param name="localeDir">Directory that contains gettext localization files</param>
		/// <param name="cultureInfo">Culture info</param>
		public Catalog(string domain, string localeDir, CultureInfo cultureInfo)
			: this(cultureInfo)
		{
			this.Load(cultureInfo, domain, localeDir);
		}

		/// <summary>
		/// Load translations from MO file that can be found by given parameters.
		/// </summary>
		/// <param name="cultureInfo">Culture info</param>
		/// <param name="domain">Catalog domain name</param>
		/// <param name="localeDir">Directory that contains gettext localization files</param>
		public void Load(CultureInfo cultureInfo, string domain, string localeDir)
		{
			var path = this.GetFileName(localeDir, cultureInfo.Name, domain);

			if (!File.Exists(path))
			{
				path = this.GetFileName(localeDir, cultureInfo.TwoLetterISOLanguageName, domain);
			}

			if (!File.Exists(path))
			{
				throw new FileNotFoundException("Can not find MO file name.");
			}

			this.Load(path);
		}

		/// <summary>
		/// Load translations from MO file that can be found by given path.
		/// </summary>
		/// <param name="path">Path to *.mo file</param>
		public void Load(string path)
		{
			Trace.WriteLine(String.Format("Loading translations from file \"{0}\"...", path), "NGettext");
			using (var stream = File.OpenRead(path))
			{
				this.Load(stream);
			}
		}

		/// <summary>
		/// Load translations from given MO file stream using given encoding (UTF-8 encoding by default).
		/// </summary>
		/// <param name="moStream">Stream that contain binary data in the MO file format</param>
		/// <param name="encoding">File encoding (UTF-8 by default)</param>
		public void Load(Stream moStream, Encoding encoding = null)
		{
			var parser = new MoFileParser();
			if (encoding != null)
			{
				parser.Encoding = encoding;
			}

			var translations = parser.GetTranslations(moStream);

			this.Translations = translations;
		}

		private string GetFileName(string localeDir, string locale, string domain)
		{
			var relativePath =
				locale.Replace('-', '_') + Path.AltDirectorySeparatorChar +
				"LC_MESSAGES" + Path.AltDirectorySeparatorChar +
				domain + ".mo";

			return Path.Combine(localeDir, relativePath);
		}
	}
}