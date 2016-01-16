using System;
using System.IO;
using NGettext.Loaders;

namespace NGettext.PluralCompile
{
	/// <summary>
	/// A catalog loader that loads data from files in the GNU/Gettext MO file format and generates
	/// a plural form rule using <see cref="CompiledPluralRuleGenerator"/>.
	/// </summary>
	public class MoCompilingPluralLoader : MoLoader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class which will try to load a MO file
		/// that will be located in the localeDir using the domain name and catalog's culture info.
		/// <see cref="CompiledPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="localeDir"></param>
		/// <param name="parser"></param>
		public MoCompilingPluralLoader(string domain, string localeDir, MoFileParser parser)
			: base(domain, localeDir, new CompiledPluralRuleGenerator(), parser)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class which will try to load a MO file
		/// from the specified path.
		/// <see cref="CompiledPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="parser"></param>
		public MoCompilingPluralLoader(string filePath, MoFileParser parser)
			: base(filePath, new CompiledPluralRuleGenerator(), parser)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class which will try to load a MO file
		/// from the specified stream.
		/// <see cref="CompiledPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="moStream"></param>
		/// <param name="parser"></param>
		public MoCompilingPluralLoader(Stream moStream, MoFileParser parser)
			: base(moStream, new CompiledPluralRuleGenerator(), parser)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class which will try to load a MO file
		/// that will be located in the localeDir using the domain name and catalog's culture info.
		/// <see cref="CompiledPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="localeDir"></param>
		public MoCompilingPluralLoader(string domain, string localeDir)
			: base(domain, localeDir, new CompiledPluralRuleGenerator())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class which will try to load a MO file
		/// from the specified path.
		/// <see cref="CompiledPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="filePath"></param>
		public MoCompilingPluralLoader(string filePath)
			: base(filePath, new CompiledPluralRuleGenerator())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MoLoader"/> class which will try to load a MO file
		/// from the specified stream.
		/// <see cref="CompiledPluralRuleGenerator"/> will be used to generate a plural form rule.
		/// </summary>
		/// <param name="moStream"></param>
		public MoCompilingPluralLoader(Stream moStream)
			: base(moStream, new CompiledPluralRuleGenerator())
		{
		}
	}
}
