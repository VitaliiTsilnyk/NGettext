using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NGettext.Plural;
using NUnit.Framework;

namespace NGettext.Tests.Plural
{
	[TestFixture]
	public class PluralRuleTest
	{
		[SetUp]
		public void Init()
		{

		}

		#region Custom forms

		[Test]
		public void TestCustomForms()
		{
			var rule1 = new PluralRule(1, number => (int)number);
			var rule2 = new PluralRule(1, number => (int)(number * 2));

			foreach (var n in new long[] { 0, 1, 2, 3, 100, 101, 102, 103 })
			{
				Assert.AreEqual(n, rule1.Evaluate(n));
				Assert.AreEqual(n * 2, rule2.Evaluate(n));
			}
		}

		#endregion
	}
}