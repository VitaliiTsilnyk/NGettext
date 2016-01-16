using System;
using Xunit;
using NGettext.Plural;

namespace NGettext.Tests.Plural
{
    public class AstPluralRuleGeneratorTest
	{

		[Fact]
		public void ParseNumPluralsTest()
		{
			var generator = new AstPluralRuleGenerator();
			Assert.Equal(9, generator.ParseNumPlurals("nplurals=9; plural=n"));
		}

		[Fact]
		public void ParsePluralFormulaTextTest()
		{
			var generator = new AstPluralRuleGenerator();
			var formulaString = "n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2";
			Assert.Equal(formulaString, generator.ParsePluralFormulaText("nplurals=9; plural=" + formulaString));
		}
	}
}
