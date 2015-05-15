using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;

namespace NGettext.Loaders
{
	/// <summary>
	/// MO file format parser.
	/// See http://www.gnu.org/software/gettext/manual/html_node/MO-Files.html
	/// </summary>
	public class MoFileParser
	{
		/// <summary>
		/// MO file format magic number.
		/// </summary>
		private const uint MO_FILE_MAGIC = 0x950412de;

		private const ushort MAX_SUPPORTED_VERSION = 1;

		private struct StringOffsetTable
		{
			public int Length;
			public int Offset;
		}

		/// <summary>
		/// Current encoding for decoding all strings in given MO file.
		/// </summary>
		/// <remarks>
		/// Default value is UTF-8 as it is compatible with required by specifications us-ascii.
		/// This value could be changed when DetectEncoding property is set to True and the MO file
		/// contains a valid charset name in the Content-Type header.
		/// </remarks>
		public Encoding Encoding { get; set; }

		/// <summary>
		/// Gets or sets a value that indicates whenever the parser can detect file encoding using the Content-Type MIME header.
		/// </summary>
		public bool DetectEncoding { get; set; }

		/// <summary>
		/// Gets a value that indicates whenever loaded file was in the big-endian format.
		/// </summary>
		public bool IsBigEndian { get; protected set; }

		/// <summary>
		/// Gets parsed file's format revision.
		/// </summary>
		public Version FormatRevision { get; protected set; }

		/// <summary>
		/// Gets parsed file's metadata.
		/// </summary>
		public Dictionary<string, string> Headers { get; protected set; }

		/// <summary>
		/// Gets parsed file's translation strings.
		/// </summary>
		public Dictionary<string, string[]> Translations { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MoFileParser"/> class.
		/// </summary>
		public MoFileParser()
		{
			this.Encoding = Encoding.UTF8;
			this.DetectEncoding = true;
			this._Init();
		}

		/// <summary>
		/// Parses a GNU MO file from the given stream and loads all available data.
		/// </summary>
		/// <remarks>
		///	http://www.gnu.org/software/gettext/manual/html_node/MO-Files.html
		/// </remarks>
		/// <param name="stream">Stream that contain binary data in the MO file format</param>
		public void Parse(Stream stream)
		{
			this._Init();
			Trace.WriteLine("Trying to parse a MO file stream...", "NGettext");

			if (stream == null || stream.Length < 20)
			{
				throw new ArgumentException("Stream can not be null of less than 20 bytes long.");
			}

			var reader = new BinaryReader(new ReadOnlyStreamWrapper(stream));
			try
			{
				var magicNumber = reader.ReadUInt32();
				if (magicNumber != MO_FILE_MAGIC)
				{
					// System.IO.BinaryReader does not respect machine endianness and always uses little-endian
					// So we need to detect and read big-endian files by ourselves
					if (_ReverseBytes(magicNumber) == MO_FILE_MAGIC)
					{
						Trace.WriteLine("Big Endian file detected. Switching readers...", "NGettext");
						this.IsBigEndian = true;
						reader.Close();
						reader = new BigEndianBinaryReader(new ReadOnlyStreamWrapper(stream));
					}
					else
					{
						throw new ArgumentException("Invalid stream: can not find MO file magic number.");
					}
				}

				var revision = reader.ReadInt32();
				this.FormatRevision = new Version(revision >> 16, revision & 0xffff);

				Trace.WriteLine(String.Format("MO File Revision: {0}.{1}.", this.FormatRevision.Major, this.FormatRevision.Minor), "NGettext");

				if (this.FormatRevision.Major > MAX_SUPPORTED_VERSION)
				{
					throw new Exception(String.Format("Unsupported MO file major revision: {0}.", this.FormatRevision.Major));
				}

				var stringCount = reader.ReadInt32();
				var originalTableOffset = reader.ReadInt32();
				var translationTableOffset = reader.ReadInt32();

				// We don't support hash tables and system dependent segments.

				Trace.WriteLine(String.Format("MO File contains {0} strings.", stringCount), "NGettext");

				var originalTable = new StringOffsetTable[stringCount];
				var translationTable = new StringOffsetTable[stringCount];

				Trace.WriteLine(String.Format("Trying to parse strings using encoding \"{0}\"...", this.Encoding), "NGettext");

				reader.BaseStream.Seek(originalTableOffset, SeekOrigin.Begin);
				for (int i = 0; i < stringCount; i++)
				{
					originalTable[i].Length = reader.ReadInt32();
					originalTable[i].Offset = reader.ReadInt32();
				}

				reader.BaseStream.Seek(translationTableOffset, SeekOrigin.Begin);
				for (int i = 0; i < stringCount; i++)
				{
					translationTable[i].Length = reader.ReadInt32();
					translationTable[i].Offset = reader.ReadInt32();
				}


				for (int i = 0; i < stringCount; i++)
				{
					var originalStrings = this._ReadStrings(reader, originalTable[i].Offset, originalTable[i].Length);
					var translatedStrings = this._ReadStrings(reader, translationTable[i].Offset, translationTable[i].Length);

					if (originalStrings.Length == 0 || translatedStrings.Length == 0) continue;

					if (originalStrings[0].Length == 0)
					{
						// MO file metadata processing
						foreach (var headerText in translatedStrings[0].Split(new []{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries))
						{
							var header = headerText.Split(new [] {':'}, 2);
							if (header.Length == 2)
							{
								this.Headers.Add(header[0], header[1].Trim());
							}
						}

						if (this.DetectEncoding && this.Headers.ContainsKey("Content-Type"))
						{
							try
							{
								var contentType = new ContentType(this.Headers["Content-Type"]);
								if (!String.IsNullOrEmpty(contentType.CharSet))
								{
									this.Encoding = Encoding.GetEncoding(contentType.CharSet);
									Trace.WriteLine(String.Format("File encoding switched to \"{0}\" (\"{1}\" requested).", this.Encoding, contentType.CharSet), "NGettext");
								}
							}
							catch (Exception exception)
							{
								Trace.WriteLine(String.Format("Unable to change parser encoding using the Content-Type header: \"{0}\".", exception.Message), "NGettext");
							}
						}
						if (this.Headers.ContainsKey("Plural-Forms"))
						{
							//TODO: Plural forms parsing.
						}
					}

					this.Translations.Add(originalStrings[0], translatedStrings);
				}

				Trace.WriteLine("String parsing completed.", "NGettext");

			}
			finally
			{
				reader.Close();
			}
		}

		private void _Init()
		{
			this.Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			this.Translations = new Dictionary<string, string[]>();
			this.FormatRevision = null;
			this.IsBigEndian = false;
		}

		private string[] _ReadStrings(BinaryReader reader, int offset, int length)
		{
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			var stringBytes = reader.ReadBytes(length);
			return this.Encoding.GetString(stringBytes).Split('\0');
		}

		private static uint _ReverseBytes(uint value)
		{
			return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
				   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
		}
	}
}