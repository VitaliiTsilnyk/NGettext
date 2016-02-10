using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NGettext.Loaders
{
	internal class ContentType
	{
		private static readonly Regex Regex = new Regex(@"^(?<type>\w+)\/(?<subType>\w+)(?:\s*;\s*(?<paramName>\w+)\s*=\s*(?<paramValue>(?:[0-9\w_-]+)|(?:"".+ "")))*",
#if DNXCORE50 || PORTABLE
			RegexOptions.IgnoreCase
#else
			RegexOptions.IgnoreCase | RegexOptions.Compiled
#endif
		);

		public ContentType(string contentType)
		{
			if (contentType == null)
				throw new ArgumentNullException("contentType");
			if (contentType == String.Empty)
				throw new ArgumentException("Parameter cannot be an empty string", "contentType");

			Source = contentType;
			_parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			ParseValue();
		}

		private IDictionary<string, string> _parameters;

		public string Source { get; private set; }
		public string Type { get; private set; }
		public string SubType { get; private set; }
		public string MediaType
		{
			get { return Type + "/" + MediaType; }
		}

		public string CharSet { get { return GetParameter("charset"); } }

		public string GetParameter(string name)
		{
			string value;
			_parameters.TryGetValue(name, out value);
			return value;
		}

		private void ParseValue()
		{
			var match = Regex.Match(Source);
			if (!match.Success)
				throw new FormatException("Failed to parse content type: invalid format");

			Type = match.Groups["type"].Value;
			SubType = match.Groups["subType"].Value;

			var paramNames = match.Groups["paramName"].Captures;
			var paramValues = match.Groups["paramValue"].Captures;

			for (var i = 0; i < paramNames.Count; i++)
			{
				var paramName = (Capture)paramNames[i];
				var paramValue = (Capture)paramValues[i];

				var name = paramName.Value.ToLowerInvariant();
				var value = paramValue.Value;

				_parameters[name] = value;
			}
		}
	}
}
