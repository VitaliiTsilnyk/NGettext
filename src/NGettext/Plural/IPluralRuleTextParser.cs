using System;

namespace NGettext.Plural
{
	/// <summary>
	/// Represents a plural rule generator that can generate plural rules based on parsed text.
	/// </summary>
	public interface IPluralRuleTextParser : IPluralRuleGenerator
	{
		/// <summary>
		/// Sets a plural rule text representation that needs to be parsed.
		/// </summary>
		/// <param name="pluralRuleText">Plural rule text representation.</param>
		void SetPluralRuleText(string pluralRuleText);
	}
}