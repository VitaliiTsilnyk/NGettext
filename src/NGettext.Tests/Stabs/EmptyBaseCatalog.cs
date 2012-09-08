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
	}
}