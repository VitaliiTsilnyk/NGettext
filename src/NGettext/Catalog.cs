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
			try
			{
				this.Load(moStream);
			}
			catch (FileNotFoundException exception)
			{
				Trace.WriteLine(String.Format("Translation file loading fail: {0}", exception.Message), "NGettext");
			}
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
			try
			{
				this.Load(moStream);
			}
			catch (FileNotFoundException exception)
			{
				Trace.WriteLine(String.Format("Translation file loading fail: {0}", exception.Message), "NGettext");
			}
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
			try
			{
				this.Load(this.CultureInfo, domain, localeDir);
			}
			catch (FileNotFoundException exception)
			{
				Trace.WriteLine(String.Format("Translation file loading fail: {0}", exception.Message), "NGettext");
			}
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
			try
			{
				this.Load(this.CultureInfo, domain, localeDir);
			}
			catch (FileNotFoundException exception)
			{
				Trace.WriteLine(String.Format("Translation file loading fail: {0}", exception.Message), "NGettext");
			}
		}

		/// <summary>
		/// Load translations from MO file that can be found by given parameters.
		/// </summary>
		/// <param name="cultureInfo">Culture info</param>
		/// <param name="domain">Catalog domain name</param>
		/// <param name="localeDir">Directory that contains gettext localization files</param>
		public void Load(CultureInfo cultureInfo, string domain, string localeDir)
		{
			var path = this._FindTranslationFile(cultureInfo, domain, localeDir);
			if (path == null)
			{
				throw new FileNotFoundException(String.Format("Can not find MO file name in locale directory \"{0}\".", localeDir));
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
		/// Load translations from given MO file stream using given encoding.
		/// </summary>
		/// <remarks>
		/// By default, parser will try to detect file encoding automatically with fallback to UTF-8 encoding.
		/// If you specify any encoding in this method, auto-detect will be turned off and given encoding will be used instead.
		/// </remarks>
		/// <param name="moStream">Stream that contain binary data in the MO file format</param>
		/// <param name="encoding">File encoding (auto detect by default)</param>
		public void Load(Stream moStream, Encoding encoding = null)
		{
			var parser = new MoFileParser();
			if (encoding != null)
			{
				parser.DetectEncoding = false;
				parser.Encoding = encoding;
			}

			parser.Parse(moStream);

			this.Translations = parser.Translations;
		}

		private string _FindTranslationFile(CultureInfo cultureInfo, string domain, string localeDir)
		{
			var possibleFiles = new [] {
				this._GetFileName(localeDir, domain, cultureInfo.Name),
				this._GetFileName(localeDir, domain, cultureInfo.TwoLetterISOLanguageName)
			};

			foreach (var possibleFilePath in possibleFiles)
			{
				if (File.Exists(possibleFilePath))
				{
					return possibleFilePath;
				}
			}

			return null;
		}

		private string _GetFileName(string localeDir, string domain, string locale)
		{
			var relativePath =
				locale.Replace('-', '_') + Path.DirectorySeparatorChar +
				"LC_MESSAGES" + Path.DirectorySeparatorChar +
				domain + ".mo";

			return Path.Combine(localeDir, relativePath);
		}
	}
}