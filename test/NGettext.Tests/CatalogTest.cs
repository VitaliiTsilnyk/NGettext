using System.Globalization;
using Xunit;

namespace NGettext.Tests
{
	public class CatalogTest
	{

		#region GetString

		[Fact]
		public void TestGetString()
		{
			var t = new Catalog();
			t.Translations.Add("key1", new[] { "value1" });
			t.Translations.Add("key2", new[] { "value2" });
			t.Translations.Add("key3", new[] { "value3plural1", "value3plural2" });
			t.Translations.Add("", new[] { "emptyIdValue" });

			Assert.Equal("value1", t.GetString("key1"));
			Assert.Equal("value2", t.GetString("key2"));
			Assert.Equal("value3plural1", t.GetString("key3"));
			Assert.Equal("notFoundKey2", t.GetString("notFoundKey2"));
			Assert.Equal("", t.GetString(""));
		}

		[Fact]
		public void TestGetStringFormat()
		{
			var t = new Catalog();

			Assert.Equal("Foo bar", t.GetString("Foo {0}", "bar"));
			Assert.Equal("Foo bar baz", t.GetString("Foo {0} {1}", "bar", "baz"));
			Assert.Equal("Foo 1 2", t.GetString("Foo {0} {1}", 1, 2));
		}

		[Fact]
		public void TestGetStringFormatCulture()
		{
			var catalogEn = new Catalog(new CultureInfo("en-US"));
			var catalogRu = new Catalog(new CultureInfo("ru-RU"));

			Assert.Equal("Foo 1.23", catalogEn.GetString("Foo {0}", 1.23));
			Assert.Equal("Foo 1,23", catalogRu.GetString("Foo {0}", 1.23));
		}

		#endregion

		#region GetPluralString

		[Fact]
		public void TestGetPluralString()
		{
			var t = new Catalog(new CultureInfo("en-US"));
			t.Translations.Add("key1", new[] {"value1"});
			t.Translations.Add("key2plural1", new[] {"value2plural1", "value2plural2"});

			Assert.Equal("value2plural1", t.GetPluralString("key2plural1", "key2plural2", 1));
			Assert.Equal("value1", t.GetPluralString("key1", "key1", 1));
			Assert.Equal("key1plural", t.GetPluralString("key1", "key1plural", 2));

			Assert.Equal("key3singular", t.GetPluralString("key3singular", "key3plural", 1));
			Assert.Equal("key3plural", t.GetPluralString("key3singular", "key3plural", 2));
		}

		[Fact]
		public void TestGetPluralStringFormat()
		{
			var t = new Catalog(new CultureInfo("en-US"));

			Assert.Equal("You have 1 apple", t.GetPluralString("You have {0} apple", "You have {0} apples", 1, 1));
			Assert.Equal("You have 2 apples", t.GetPluralString("You have {0} apple", "You have {0} apples", 2, 2));
			Assert.Equal("You have 0 apples", t.GetPluralString("You have {0} apple", "You have {0} apples", 0, 0));

			Assert.Equal("You have 1 apple", t.GetPluralString("{0} have {1} apple", "{0} have {1} apples", 1, "You", 1));
			Assert.Equal("You have 2 apples", t.GetPluralString("{0} have {1} apple", "{0} have {1} apples", 2, "You", 2));
			Assert.Equal("You have 0 apples", t.GetPluralString("{0} have {1} apple", "{0} have {1} apples", 0, "You", 0));
		}

		[Fact]
		public void TestGetPluralStringInternational()
		{
			var t = new Catalog(new CultureInfo("ru-RU"));
			t.Translations.Add("You have {0} apple", new[] { "У Вас {0} яблоко", "У Вас {0} яблока", "У Вас {0} яблок" });

			Assert.Equal("У Вас 1 яблоко", t.GetPluralString("You have {0} apple", "You have {0} apples", 1, 1));
			Assert.Equal("У Вас 2 яблока", t.GetPluralString("You have {0} apple", "You have {0} apples", 2, 2));
			Assert.Equal("У Вас 3 яблока", t.GetPluralString("You have {0} apple", "You have {0} apples", 3, 3));
			Assert.Equal("У Вас 4 яблока", t.GetPluralString("You have {0} apple", "You have {0} apples", 4, 4));
			Assert.Equal("У Вас 5 яблок", t.GetPluralString("You have {0} apple", "You have {0} apples", 5, 5));
			Assert.Equal("У Вас 6 яблок", t.GetPluralString("You have {0} apple", "You have {0} apples", 6, 6));
			Assert.Equal("У Вас 11 яблок", t.GetPluralString("You have {0} apple", "You have {0} apples", 11, 11));
			Assert.Equal("У Вас 21 яблоко", t.GetPluralString("You have {0} apple", "You have {0} apples", 21, 21));

			Assert.Equal("He has 5 apples", t.GetPluralString("He has {0} apple", "He has {0} apples", 5, 5));
		}

		[Fact]
		public void TestGetPluralStringFormatCulture()
		{
			var catalogEn = new Catalog(new CultureInfo("en-US"));
			var catalogRu = new Catalog(new CultureInfo("ru-RU"));

			Assert.Equal("Foo 1.23", catalogEn.GetPluralString("Foo {0}", "Foo {0}", (long)1.23, 1.23));
			Assert.Equal("Foo 1,23", catalogRu.GetPluralString("Foo {0}", "Foo {0}", (long)1.23, 1.23));
		}

		#endregion

		#region GetParticularString

		[Fact]
		public void TestGetParticularString()
		{
			var t = new Catalog();
			t.Translations.Add("key1", new[] { "value1" });
			t.Translations.Add("context1" + Catalog.CONTEXT_GLUE + "key1", new[] { "value2" });
			t.Translations.Add("context2" + Catalog.CONTEXT_GLUE + "key1", new[] { "value3" });

			Assert.Equal("value1", t.GetString("key1"));
			Assert.Equal("value2", t.GetParticularString("context1", "key1"));
			Assert.Equal("value3", t.GetParticularString("context2", "key1"));
			Assert.Equal("key1", t.GetParticularString("context3", "key1"));
		}

		[Fact]
		public void TestGetParticularStringFormat()
		{
			var t = new Catalog();

			Assert.Equal("Foo bar", t.GetParticularString("context", "Foo {0}", "bar"));
			Assert.Equal("Foo bar baz", t.GetParticularString("context", "Foo {0} {1}", "bar", "baz"));
		}

		[Fact]
		public void TestGetParticularStringFormatCulture()
		{
			var catalogEn = new Catalog(new CultureInfo("en-US"));
			var catalogRu = new Catalog(new CultureInfo("ru-RU"));

			Assert.Equal("Foo 1.23", catalogEn.GetParticularString("context", "Foo {0}", 1.23));
			Assert.Equal("Foo 1,23", catalogRu.GetParticularString("context", "Foo {0}", 1.23));
		}

		#endregion

		#region GetParticularPluralString

		[Fact]
		public void TestGetParticularPluralString()
		{
			var t = new Catalog(new CultureInfo("en-US"));
			t.Translations.Add("context1" + Catalog.CONTEXT_GLUE + "key1-1", new[] { "value1-1", "value1-2" });
			t.Translations.Add("context2" + Catalog.CONTEXT_GLUE + "key1-1", new[] { "value2-1", "value2-2" });

			Assert.Equal("value1-1", t.GetParticularPluralString("context1", "key1-1", "key1-2", 1));
			Assert.Equal("value1-2", t.GetParticularPluralString("context1", "key1-1", "key1-2", 2));
			Assert.Equal("value2-1", t.GetParticularPluralString("context2", "key1-1", "key1-2", 1));
			Assert.Equal("key1-1", t.GetParticularPluralString("context3", "key1-1", "key1-2", 1));
		}

		[Fact]
		public void TestGetParticularPluralStringFormat()
		{
			var t = new Catalog(new CultureInfo("en-US"));

			Assert.Equal("Foo bar", t.GetParticularPluralString("context", "Foo {0}", "Bar {0}", 1, "bar"));
			Assert.Equal("Bar bar", t.GetParticularPluralString("context", "Foo {0}", "Bar {0}", 2, "bar"));
			Assert.Equal("Foo bar baz", t.GetParticularPluralString("context", "Foo {0} {1}", "Bar {0} {1}", 1, "bar", "baz"));
			Assert.Equal("Bar bar baz", t.GetParticularPluralString("context", "Foo {0} {1}", "Bar {0} {1}", 2, "bar", "baz"));
		}

		[Fact]
		public void TestGetParticularPluralStringFormatCulture()
		{
			var catalogEn = new Catalog(new CultureInfo("en-US"));
			var catalogRu = new Catalog(new CultureInfo("ru-RU"));

			Assert.Equal("Foo 1.23", catalogEn.GetParticularPluralString("context", "Foo {0}", "Foo {0}", (long)1.23, 1.23));
			Assert.Equal("Foo 1,23", catalogRu.GetParticularPluralString("context", "Foo {0}", "Foo {0}", (long)1.23, 1.23));
		}

		#endregion

		#region *Default methods

		[Fact]
		public void TestProtectedGetString()
		{
			var t = new Catalog();
			t.Translations.Add("key1", new[] { "value1" });

			Assert.Equal("value1", t.GetStringDefault("key1", null));
			Assert.Equal(null, t.GetStringDefault("key2", null));
			Assert.Equal("defaultMessage", t.GetStringDefault("key2", "defaultMessage"));
		}

		[Fact]
		public void TestProtectedGetPluralString()
		{
			var t = new Catalog(new CultureInfo("en-US"));
			t.Translations.Add("key1", new[] { "value1" });

			Assert.Equal("value1", t.GetPluralStringDefault("key1", "defaultSingular", "defaultPlural", 1));
			Assert.Equal("defaultPlural", t.GetPluralStringDefault("key1", "defaultSingular", "defaultPlural", 2));
			Assert.Equal("defaultSingular", t.GetPluralStringDefault("key2", "defaultSingular", "defaultPlural", 1));
			Assert.Equal("defaultPlural", t.GetPluralStringDefault("key2", "defaultSingular", "defaultPlural", 2));
		}

		[Fact]
		public void TestProtectedGetTranslations()
		{
			var t = new Catalog();
			t.Translations.Add("key1", new[] { "value1" });
			t.Translations.Add("key2", new[] { "value2" });
			t.Translations.Add("key3", new[] { "value3plural1", "value3plural2" });

			Assert.Equal(new[] { "value1" }, t.GetTranslations("key1"));
			Assert.Equal(new[] { "value2" }, t.GetTranslations("key2"));
			Assert.Equal(new[] { "value3plural1", "value3plural2" }, t.GetTranslations("key3"));
			Assert.Equal(null, t.GetTranslations("key4"));
		}
		
		[Fact]
		public void TesIsTranslationExist()
		{
			var t = new Catalog(new CultureInfo("en-US"));
			t.Translations.Add("key1", new[] { "value1" });

			Assert.Equal(true, t.IsTranslationExist("key1"));
			Assert.Equal(false, t.IsTranslationExist("key2"));
		}

		#endregion
	}
}
