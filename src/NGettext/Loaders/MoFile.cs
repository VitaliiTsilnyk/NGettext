using System;
using System.Collections.Generic;
using System.Text;

namespace NGettext.Loaders
{
	/// <summary>
	/// Represents a parsed MO file data.
	/// </summary>
    public class MoFile
	{
		/// <summary>
		/// Gets parsed file's format revision.
		/// </summary>
		public Version FormatRevision { get; protected set; }

		/// <summary>
		/// Gets a value that indicates whenever loaded file was in the BigEndian format.
		/// </summary>
		public bool BigEndian { get; protected set; }

		/// <summary>
		/// Gets or sets file's encoding.
		/// </summary>
		public Encoding Encoding { get; set; }

		/// <summary>
		/// Gets parsed file's meta data.
		/// </summary>
		public Dictionary<string, string> Headers { get; protected set; }

		/// <summary>
		/// Gets parsed file's translation strings.
		/// </summary>
		public Dictionary<string, string[]> Translations { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MoFile"/> class.
		/// </summary>
		/// <param name="formatRevision">File format revision.</param>
		/// <param name="encoding">File encoding.</param>
		/// <param name="bigEndian">File endianness.</param>
		public MoFile(Version formatRevision, Encoding encoding = null, bool bigEndian = false)
		{
			this.FormatRevision = formatRevision;
			this.BigEndian = bigEndian;
			this.Encoding = encoding;
			this.Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			this.Translations = new Dictionary<string, string[]>();
		}
	}
}
