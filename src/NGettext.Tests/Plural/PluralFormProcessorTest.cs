using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using NUnit.Framework;

using NGettext.Plural;

namespace Tests.Plural
{
	[TestFixture]
	public class PluralFormProcessorTest
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


			foreach (var testCase in dict)
			{
				var locale = CultureInfo.CreateSpecificCulture(testCase.Key);
				foreach (var data in testCase.Value)
				{
					Assert.AreEqual(data.Value, PluralFormProcessor.Default.GetPluralFormIndex(locale, data.Key));
				}
			}
		}

		#endregion

		#region Custom forms

		[Test]
		public void TestCustomForms()
		{
			var p = new PluralFormProcessor();

			var en = CultureInfo.CreateSpecificCulture("en-US");
			p.SetCustomFormula(en, l => (int)l);

			var fr = CultureInfo.CreateSpecificCulture("fr");
			p.SetCustomFormula(fr, l => (int)(l * 2));

			foreach (var n in new long[] { 0, 1, 2, 3, 100, 101, 102, 103 })
			{
				Assert.AreEqual(n, p.GetPluralFormIndex(en, n));
				Assert.AreEqual(n * 2, p.GetPluralFormIndex(fr, n));
			}
		}

		#endregion

	}
}