using System;
using System.IO;
using NGettext.Plural;

namespace NGettext.Loaders
{
	/// <summary>
	/// A catalog loader that loads data from files in the GNU/Gettext MO file format and generates
	/// a plural form rule using <see cref="AstPluralRuleGenerator"/>.
	/// </summary>
	public class MoAstPluralLoader : MoLoader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MoAstPluralLoader"/> class whitch will try to load a MO file
		/// that will be located in the localeDir using the domain name and catalog's culture info.
		/// <see cref="AstPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="localeDir"></param>
		/// <param name="parser"></param>
		public MoAstPluralLoader(string domain, string localeDir, MoFileParser parser)
			: base(domain, localeDir, new AstPluralRuleGenerator(), parser)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class whitch will try to load a MO file
		/// from the specified path.
		/// <see cref="AstPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="parser"></param>
		public MoAstPluralLoader(string filePath, MoFileParser parser)
			: base(filePath, new AstPluralRuleGenerator(), parser)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class whitch will try to load a MO file
		/// from the specified stream.
		/// <see cref="AstPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="moStream"></param>
		/// <param name="parser"></param>
		public MoAstPluralLoader(Stream moStream, MoFileParser parser)
			: base(moStream, new AstPluralRuleGenerator(), parser)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class whitch will try to load a MO file
		/// that will be located in the localeDir using the domain name and catalog's culture info.
		/// <see cref="AstPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="localeDir"></param>
		public MoAstPluralLoader(string domain, string localeDir)
			: base(domain, localeDir, new AstPluralRuleGenerator(), new MoFileParser())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class whitch will try to load a MO file
		/// from the specified path.
		/// <see cref="AstPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="filePath"></param>
		public MoAstPluralLoader(string filePath)
			: base(filePath, new AstPluralRuleGenerator(), new MoFileParser())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class whitch will try to load a MO file
		/// from the specified stream.
		/// <see cref="AstPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="moStream"></param>
		public MoAstPluralLoader(Stream moStream)
			: base(moStream, new AstPluralRuleGenerator(), new MoFileParser())
		{
		}
	}
}