using Xunit;
using NGettext.Plural;

namespace NGettext.Tests.Plural
{
	public class PluralRuleTest
	{
		#region Custom forms

		[Fact]
		public void TestCustomForms()
		{
			var rule1 = new PluralRule(1, number => (int)number);
			var rule2 = new PluralRule(1, number => (int)(number * 2));

			foreach (var n in new long[] { 0, 1, 2, 3, 100, 101, 102, 103 })
			{
				Assert.Equal(n, rule1.Evaluate(n));
				Assert.Equal(n * 2, rule2.Evaluate(n));
			}
		}

		#endregion
	}
}