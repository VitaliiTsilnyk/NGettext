using System;
using System.Globalization;
using System.Text.RegularExpressions;
using NGettext.Plural.Ast;

namespace NGettext.Plural
{
	/// <summary>
	/// Plural rule generator that can parse a string that contains a plural rule and generate an AstPluralRule from it.
	/// </summary>
	public class AstPluralRuleGenerator : DefaultPluralRuleGenerator, IPluralRuleTextParser
	{
		private static readonly Regex NPluralsRegex = new Regex(@"(nplurals=(?<nplurals>\d+))",
#if DNXCORE50
			RegexOptions.IgnoreCase
#else
			RegexOptions.IgnoreCase | RegexOptions.Compiled
#endif
		);

		private static readonly Regex PluralRegex = new Regex(@"(plural=(?<plural>[^;\n]+))",
#if DNXCORE50
			RegexOptions.IgnoreCase
#else
			RegexOptions.IgnoreCase | RegexOptions.Compiled
#endif
		);


		/// <summary>
		/// Gets a plural rule text.
		/// </summary>
		protected string PluralRuleText { get; private set; }

		public AstTokenParser Parser { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AstPluralRuleGenerator"/> class with no plural rule text.
		/// </summary>
		public AstPluralRuleGenerator()
		{
			this.Parser = new AstTokenParser();
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
				var numPlurals = this.ParseNumPlurals(this.PluralRuleText);
				var plural = this.ParsePluralFormulaText(this.PluralRuleText);
				var astTree = this.Parser.Parse(plural);

				return new AstPluralRule(numPlurals, astTree);
			}

			return base.CreateRule(cultureInfo);
		}

		public int ParseNumPlurals(string input)
		{
			var match = NPluralsRegex.Match(input);
			if (!match.Success)
				throw new FormatException("Failed to parse 'nplurals' parameter from the plural rule text: invalid format");

			return int.Parse(match.Groups["nplurals"].Value);
		}

		public string ParsePluralFormulaText(string input)
		{
			var match = PluralRegex.Match(input);
			if (!match.Success)
				throw new FormatException("Failed to parse 'plural' parameter from the plural rule text: invalid format");

			return match.Groups["plural"].Value;
		}
	}
}