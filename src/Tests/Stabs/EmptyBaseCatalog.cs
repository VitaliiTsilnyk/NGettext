using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using NGettext;

namespace Tests.Stabs
{
	public class EmptyBaseCatalog : BaseCatalog
	{
		public EmptyBaseCatalog() : base(CultureInfo.CurrentUICulture) { }

		public EmptyBaseCatalog(CultureInfo cultureInfo) : base(cultureInfo) { }

		public string ProtectedGetString(string messageId, string defaultMessage)
		{
			return this._GetString(messageId, defaultMessage);
		}

		public string ProtectedGetPluralString(string messageId, string defaultMessage, string defaultPluralMessage, long n)
		{
			return this._GetPluralString(messageId, defaultMessage, defaultPluralMessage, n);
		}

		public string[] ProtectedGetTranslations(string messageId)
		{
			return this._GetTranslations(messageId);
		}
	}
}