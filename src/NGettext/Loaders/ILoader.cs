using System;

namespace NGettext.Loaders
{
	/// <summary>
	/// Represents an abstract loader that loads required data to the catalog.
	/// </summary>
	public interface ILoader
	{
		/// <summary>
		/// Loads translations to the specified catalog using catalog's culture info.
		/// </summary>
		/// <param name="catalog">A catalog instance to load translations to.</param>
		void Load(Catalog catalog);
	}
}