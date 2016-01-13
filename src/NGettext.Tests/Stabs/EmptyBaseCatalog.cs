using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace NGettext.Tests.Stabs
{
	public class EmptyBaseCatalog : BaseCatalog
	{
		public EmptyBaseCatalog() : base(CultureInfo.CurrentUICulture) { }

		public EmptyBaseCatalog(CultureInfo cultureInfo) : base(cultureInfo) { }
	}
}