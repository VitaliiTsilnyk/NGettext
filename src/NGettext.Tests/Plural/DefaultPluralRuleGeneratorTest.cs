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
	public class DefaultPluralRuleGeneratorTest
	{

		[SetUp]
		public void Init()
		{

		}

		#region Default forms

		[Test]
		public void TestDefaultForms()
		{
			var dict = new Dictionary<string, Dictionary<long, int>>()
				{
					{"en-US", new Dictionary<long, int>()
						{
							{0, 1},
							{1, 0},
							{2, 1},
							{3, 1},
							{5, 1},
							{10, 1},
							{999, 1},
						}},
					{"fa-IR", new Dictionary<long, int>()
						{
							{0, 0},
							{1, 0},
							{2, 0},
							{3, 0},
							{5, 0},
							{10, 0},
							{999, 0},
						}},
					{"ar-SA", new Dictionary<long, int>()
						{
							{0, 0},
							{1, 1},
							{2, 2},
							{3, 3},
							{5, 3},
							{10, 3},
							{999, 5},
						}},
					{"hi", new Dictionary<long, int>()
						{
							{0, 0},
							{1, 0},
							{2, 1},
							{3, 1},
							{5, 1},
							{10, 1},
							{999, 1},
						}},
					{"fr", new Dictionary<long, int>()
						{
							{0, 0},
							{1, 0},
							{2, 1},
							{3, 1},
							{5, 1},
							{10, 1},
							{999, 1},
						}},
					{"lv", new Dictionary<long, int>()
						{
							{0, 0},
							{1, 1},
							{2, 2},
							{3, 2},
							{5, 2},
							{10, 2},
							{999, 2},
						}},
					{"se-SE", new Dictionary<long, int>()
						{
							{0, 2},
							{1, 0},
							{2, 1},
							{3, 2},
							{5, 2},
							{10, 2},
							{999, 2},
						}},
					{"ru-RU", new Dictionary<long, int>()
						{
							{0, 2},
							{1, 0},
							{2, 1},
							{3, 1},
							{5, 2},
							{10, 2},
							{999, 2},
						}},
				};

			var generator = new DefaultPluralRuleGenerator();
			foreach (var testCase in dict)
			{
				var locale = CultureInfo.CreateSpecificCulture(testCase.Key);
				var rule = generator.CreateRule(locale);
				foreach (var data in testCase.Value)
				{
					Assert.AreEqual(data.Value, rule.Evaluate(data.Key));
				}
			}
		}

		#endregion
	}
}