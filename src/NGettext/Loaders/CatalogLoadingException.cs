using System;

namespace NGettext.Loaders
{
    public class CatalogLoadingException : Exception
	{
		public CatalogLoadingException() : base() { }
		public CatalogLoadingException(string message) : base(message) { }
		public CatalogLoadingException(string message, Exception innerException) : base(message, innerException) { }
	}
}
