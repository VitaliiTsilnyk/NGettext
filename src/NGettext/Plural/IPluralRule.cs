using System;

namespace NGettext.Plural
{
	/// <summary>
	/// Represents a plural form rule whitch can generate a plural form index by given number.
	/// </summary>
	public interface IPluralRule
	{
		/// <summary>
		/// Maximum number of plural forms supported.
		/// </summary>
		int NumPlurals { get; }

		/// <summary>
		/// Evaluates a number and returns a plural form index.
		/// </summary>
		/// <param name="number">Number whitch needs to be evaluated.</param>
		/// <returns>Plural form index.</returns>
		int Evaluate(long number);
	}
}