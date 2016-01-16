using System.Globalization;
using System.IO;
using Xunit;
using NGettext.Loaders;
using System.Collections.Generic;

namespace NGettext.Tests.Loaders
{
    public class MoAstPluralLoaderTest
	{
		public string LocalesDir;

		public MoAstPluralLoaderTest()
		{
			this.LocalesDir = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("TestResources", "locales"));
		}

		public static IEnumerable<object[]> GetTestAstPluralRuleGeneratorData()
		{
			yield return new object[] { 1, "1 минута" };
			yield return new object[] { 2, "2 минуты" };
			yield return new object[] { 3, "3 минуты" };
			yield return new object[] { 4, "4 минуты" };
			yield return new object[] { 5, "5 минут" };
			yield return new object[] { 6, "6 минут" };
			yield return new object[] { 7, "7 минут" };
			yield return new object[] { 8, "8 минут" };
			yield return new object[] { 9, "9 минут" };
			yield return new object[] { 10, "10 минут" };
			yield return new object[] { 11, "11 минут" };
			yield return new object[] { 21, "21 минута" };
			yield return new object[] { 22, "22 минуты" };
			yield return new object[] { 25, "25 минут" };
			yield return new object[] { 101, "101 минута" };
		}

		[Fact]
		public void TestAstPluralRuleGenerator()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test.mo")))))
			{
				var ignoredConflictingCulture = new CultureInfo("en-US");
				var t = new Catalog(new MoAstPluralLoader(stream), ignoredConflictingCulture);

				foreach (object[] mapping in GetTestAstPluralRuleGeneratorData())
				{
					Assert.Equal((string)mapping[1], t.GetPluralString("{0} minute", "{0} minutes", (int)mapping[0], (int)mapping[0]));
				}
			}
		}
	}
}
