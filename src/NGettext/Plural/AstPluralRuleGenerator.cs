using System;
using System.Globalization;

namespace NGettext.Plural
{
	/// <summary>
	/// Plural rule generator that can parse a string that contains a plural rule and generate an AstPluralRule from it.
	/// </summary>
	public class AstPluralRuleGenerator : DefaultPluralRuleGenerator, IPluralRuleTextParser
	{
		/// <summary>
		/// Gets a plural rule text.
		/// </summary>
		protected string PluralRuleText { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AstPluralRuleGenerator"/> class with no plural rule text.
		/// </summary>
		public AstPluralRuleGenerator()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AstPluralRuleGenerator"/> class and sets a plural rule text.
		/// </summary>
		/// <param name="pluralRuleText"></param>
		public AstPluralRuleGenerator(string pluralRuleText)
			: this()
		{
			this.SetPluralRuleText(pluralRuleText);
		}

		/// <summary>
		/// Sets a plural rule text representation that needs to be parsed.
		/// </summary>
		/// <param name="pluralRuleText">Plural rule text representation.</param>
		public void SetPluralRuleText(string pluralRuleText)
		{
			this.PluralRuleText = pluralRuleText;
		}

		/// <summary>
		/// Creates a plural rule for given culture.
		/// </summary>
		/// <param name="cultureInfo"></param>
		/// <returns></returns>
		public override IPluralRule CreateRule(CultureInfo cultureInfo)
		{
			if (this.PluralRuleText != null)
			{
				//TODO: Try to parse pluralRuleText
				
			}

			return base.CreateRule(cultureInfo);
		}
	}
}