using System;
using System.Text;
using System.Diagnostics;
using System.IO;

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
		/// Default encoding for decoding all strings in given MO file.
		/// Must be binary compatible with US-ASCII to be able to read file headers.
		/// </summary>
		/// <remarks>
		/// Default value is UTF-8 as it is compatible with required by specifications US-ASCII.
		/// </remarks>
		public Encoding DefaultEncoding { get; set; }

		/// <summary>
		/// Gets or sets a value that indicates whenever the parser can detect file encoding using the Content-Type MIME header.
		/// </summary>
		public bool AutoDetectEncoding { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MoFileParser"/> class with UTF-8 as default encoding and with enabled automatic encoding detection.
		/// </summary>
		public MoFileParser()
		{
			this.DefaultEncoding = Encoding.UTF8;
			this.AutoDetectEncoding = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoFileParser"/> class using given default encoding and given automatic encoding detection option.
		/// </summary>
		public MoFileParser(Encoding defaultEncoding, bool autoDetectEncoding = true)
		{
			this.DefaultEncoding = defaultEncoding;
			this.AutoDetectEncoding = autoDetectEncoding;
		}

		/// <summary>
		/// Parses a GNU MO file from the given stream and loads all available data.
		/// </summary>
		/// <remarks>
		///	http://www.gnu.org/software/gettext/manual/html_node/MO-Files.html
		/// </remarks>
		/// <param name="stream">Stream that contain binary data in the MO file format</param>
		/// <returns>Parsed file data.</returns>
		public MoFile Parse(Stream stream)
		{
#if DEBUG && !NETSTANDARD1_0
			Trace.WriteLine("Trying to parse a MO file stream...", "NGettext");
#endif

			if (stream == null || stream.Length < 20)
			{
				throw new ArgumentException("Stream can not be null of less than 20 bytes long.");
			}

			var bigEndian = false;
			var reader = new BinaryReader(new ReadOnlyStreamWrapper(stream));
			try
			{
				var magicNumber = reader.ReadUInt32();
				if (magicNumber != MO_FILE_MAGIC)
				{
					// System.IO.BinaryReader does not respect machine endianness and always uses LittleEndian
					// So we need to detect and read BigEendian files by ourselves
					if (_ReverseBytes(magicNumber) == MO_FILE_MAGIC)
					{
#if DEBUG && !NETSTANDARD1_0
						Trace.WriteLine("BigEndian file detected. Switching readers...", "NGettext");
#endif
						bigEndian = true;
						((IDisposable)reader).Dispose();
						reader = new BigEndianBinaryReader(new ReadOnlyStreamWrapper(stream));
					}
					else
					{
						throw new ArgumentException("Invalid stream: can not find MO file magic number.");
					}
				}

				var revision = reader.ReadInt32();
				var parsedFile = new MoFile(new Version(revision >> 16, revision & 0xffff), this.DefaultEncoding, bigEndian);

#if DEBUG && !NETSTANDARD1_0
				Trace.WriteLine(String.Format("MO File Revision: {0}.{1}.", parsedFile.FormatRevision.Major, parsedFile.FormatRevision.Minor), "NGettext");
#endif

				if (parsedFile.FormatRevision.Major > MAX_SUPPORTED_VERSION)
				{
					throw new CatalogLoadingException(String.Format("Unsupported MO file major revision: {0}.", parsedFile.FormatRevision.Major));
				}

				var stringCount = reader.ReadInt32();
				var originalTableOffset = reader.ReadInt32();
				var translationTableOffset = reader.ReadInt32();

				// We don't support hash tables and system dependent segments.

#if DEBUG && !NETSTANDARD1_0
				Trace.WriteLine(String.Format("MO File contains {0} strings.", stringCount), "NGettext");
#endif

				var originalTable = new StringOffsetTable[stringCount];
				var translationTable = new StringOffsetTable[stringCount];

#if DEBUG && !NETSTANDARD1_0
				Trace.WriteLine(String.Format("Trying to parse strings using encoding \"{0}\"...", parsedFile.Encoding), "NGettext");
#endif

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
					var originalStrings = this._ReadStrings(reader, originalTable[i].Offset, originalTable[i].Length, parsedFile.Encoding);
					var translatedStrings = this._ReadStrings(reader, translationTable[i].Offset, translationTable[i].Length, parsedFile.Encoding);

					if (originalStrings.Length == 0 || translatedStrings.Length == 0) continue;

					if (originalStrings[0].Length == 0)
					{
						// MO file meta data processing
						foreach (var headerText in translatedStrings[0].Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
						{
							var separatorIndex = headerText.IndexOf(':');
							if (separatorIndex > 0)
							{
								var headerName = headerText.Substring(0, separatorIndex);
								var headerValue = headerText.Substring(separatorIndex + 1).Trim();
								parsedFile.Headers.Add(headerName, headerValue.Trim());
							}
						}

						if (this.AutoDetectEncoding && parsedFile.Headers.ContainsKey("Content-Type"))
						{
							try
							{
								var contentType = new ContentType(parsedFile.Headers["Content-Type"]);
								if (!String.IsNullOrEmpty(contentType.CharSet))
								{
									parsedFile.Encoding = Encoding.GetEncoding(contentType.CharSet);
#if DEBUG && !NETSTANDARD1_0
									Trace.WriteLine(String.Format("File encoding switched to \"{0}\" (\"{1}\" requested).", parsedFile.Encoding, contentType.CharSet), "NGettext");
#endif
								}
							}
							catch (Exception exception)
							{
								throw new CatalogLoadingException(String.Format("Unable to change parser encoding using the Content-Type header: \"{0}\".", exception.Message), exception);
							}
						}
					}

					parsedFile.Translations.Add(originalStrings[0], translatedStrings);
				}

#if DEBUG && !NETSTANDARD1_0
				Trace.WriteLine("String parsing completed.", "NGettext");
#endif
				return parsedFile;
			}
			finally
			{
				((IDisposable)reader).Dispose();
			}
		}

		private string[] _ReadStrings(BinaryReader reader, int offset, int length, Encoding encoding)
		{
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			var stringBytes = reader.ReadBytes(length);
			return encoding.GetString(stringBytes, 0, stringBytes.Length).Split('\0');
		}

		private static uint _ReverseBytes(uint value)
		{
			return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
				   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
		}
	}
}