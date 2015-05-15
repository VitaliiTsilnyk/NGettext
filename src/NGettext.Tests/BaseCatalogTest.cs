using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using NUnit.Framework;

using NGettext.Tests.Stabs;

namespace NGettext.Tests
{
	[TestFixture]
	public class BaseCatalogTest
	{
		[SetUp]
		public void Init()
		{
			
		}

		#region GetString

		[Test]
		public void TestGetString()
		{
			var t = new EmptyBaseCatalog();
			t.Translations.Add("key1", new[] { "value1" });
			t.Translations.Add("key2", new[] { "value2" });
			t.Translations.Add("key3", new[] { "value3plural1", "value3plural2" });
			t.Translations.Add("", new[] { "emptyIdValue" });

			Assert.AreEqual("value1", t.GetString("key1"));
			Assert.AreEqual("value2", t.GetString("key2"));
			Assert.AreEqual("value3plural1", t.GetString("key3"));
			Assert.AreEqual("notFoundKey2", t.GetString("notFoundKey2"));
			Assert.AreEqual("", t.GetString(""));
		}

		[Test]
		public void TestGetStringFormat()
		{
			var t = new EmptyBaseCatalog();

			Assert.AreEqual("Foo bar", t.GetString("Foo {0}", "bar"));
			Assert.AreEqual("Foo bar baz", t.GetString("Foo {0} {1}", "bar", "baz"));
			Assert.AreEqual("Foo 1 2", t.GetString("Foo {0} {1}", 1, 2));
		}

		[Test]
		public void TestGetStringFormatCulture()
		{
			var catalogEn = new EmptyBaseCatalog(new CultureInfo("en-US"));
			var catalogRu = new EmptyBaseCatalog(new CultureInfo("ru-RU"));

			Assert.AreEqual("Foo 1.23", catalogEn.GetString("Foo {0}", 1.23));
			Assert.AreEqual("Foo 1,23", catalogRu.GetString("Foo {0}", 1.23));
		}

		#endregion

		#region GetPluralString

		[Test]
		public void TestGetPluralString()
		{
			var t = new EmptyBaseCatalog(CultureInfo.CreateSpecificCulture("en-US"));
			t.Translations.Add("key1", new[] {"value1"});
			t.Translations.Add("key2plural1", new[] {"value2plural1", "value2plural2"});

			Assert.AreEqual("value2plural1", t.GetPluralString("key2plural1", "key2plural2", 1));
			Assert.AreEqual("value1", t.GetPluralString("key1", "key1", 1));
			Assert.AreEqual("key1plural", t.GetPluralString("key1", "key1plural", 2));

			Assert.AreEqual("key3singular", t.GetPluralString("key3singular", "key3plural", 1));
			Assert.AreEqual("key3plural", t.GetPluralString("key3singular", "key3plural", 2));
		}

		[Test]
		public void TestGetPluralStringFormat()
		{
			var t = new EmptyBaseCatalog(CultureInfo.CreateSpecificCulture("en-US"));

			Assert.AreEqual("You have 1 apple", t.GetPluralString("You have {0} apple", "You have {0} apples", 1, 1));
			Assert.AreEqual("You have 2 apples", t.GetPluralString("You have {0} apple", "You have {0} apples", 2, 2));
			Assert.AreEqual("You have 0 apples", t.GetPluralString("You have {0} apple", "You have {0} apples", 0, 0));

			Assert.AreEqual("You have 1 apple", t.GetPluralString("{0} have {1} apple", "{0} have {1} apples", 1, "You", 1));
			Assert.AreEqual("You have 2 apples", t.GetPluralString("{0} have {1} apple", "{0} have {1} apples", 2, "You", 2));
			Assert.AreEqual("You have 0 apples", t.GetPluralString("{0} have {1} apple", "{0} have {1} apples", 0, "You", 0));
		}

		[Test]
		public void TestGetPluralStringInternational()
		{
			var t = new EmptyBaseCatalog(CultureInfo.CreateSpecificCulture("ru-RU"));
			t.Translations.Add("You have {0} apple", new[] { "У Вас {0} яблоко", "У Вас {0} яблока", "У Вас {0} яблок" });

			Assert.AreEqual("У Вас 1 яблоко", t.GetPluralString("You have {0} apple", "You have {0} apples", 1, 1));
			Assert.AreEqual("У Вас 2 яблока", t.GetPluralString("You have {0} apple", "You have {0} apples", 2, 2));
			Assert.AreEqual("У Вас 3 яблока", t.GetPluralString("You have {0} apple", "You have {0} apples", 3, 3));
			Assert.AreEqual("У Вас 4 яблока", t.GetPluralString("You have {0} apple", "You have {0} apples", 4, 4));
			Assert.AreEqual("У Вас 5 яблок", t.GetPluralString("You have {0} apple", "You have {0} apples", 5, 5));
			Assert.AreEqual("У Вас 6 яблок", t.GetPluralString("You have {0} apple", "You have {0} apples", 6, 6));
			Assert.AreEqual("У Вас 11 яблок", t.GetPluralString("You have {0} apple", "You have {0} apples", 11, 11));
			Assert.AreEqual("У Вас 21 яблоко", t.GetPluralString("You have {0} apple", "You have {0} apples", 21, 21));

			Assert.AreEqual("He has 5 apples", t.GetPluralString("He has {0} apple", "He has {0} apples", 5, 5));
		}

		[Test]
		public void TestGetPluralStringFormatCulture()
		{
			var catalogEn = new EmptyBaseCatalog(new CultureInfo("en-US"));
			var catalogRu = new EmptyBaseCatalog(new CultureInfo("ru-RU"));

			Assert.AreEqual("Foo 1.23", catalogEn.GetPluralString("Foo {0}", "Foo {0}", (long)1.23, 1.23));
			Assert.AreEqual("Foo 1,23", catalogRu.GetPluralString("Foo {0}", "Foo {0}", (long)1.23, 1.23));
		}

		#endregion

		#region GetParticularString

		[Test]
		public void TestGetParticularString()
		{
			var t = new EmptyBaseCatalog();
			t.Translations.Add("key1", new[] { "value1" });
			t.Translations.Add("context1" + BaseCatalog.CONTEXT_GLUE + "key1", new[] { "value2" });
			t.Translations.Add("context2" + BaseCatalog.CONTEXT_GLUE + "key1", new[] { "value3" });

			Assert.AreEqual("value1", t.GetString("key1"));
			Assert.AreEqual("value2", t.GetParticularString("context1", "key1"));
			Assert.AreEqual("value3", t.GetParticularString("context2", "key1"));
			Assert.AreEqual("key1", t.GetParticularString("context3", "key1"));
		}

		[Test]
		public void TestGetParticularStringFormat()
		{
			var t = new EmptyBaseCatalog();

			Assert.AreEqual("Foo bar", t.GetParticularString("context", "Foo {0}", "bar"));
			Assert.AreEqual("Foo bar baz", t.GetParticularString("context", "Foo {0} {1}", "bar", "baz"));
		}

		[Test]
		public void TestGetParticularStringFormatCulture()
		{
			var catalogEn = new EmptyBaseCatalog(new CultureInfo("en-US"));
			var catalogRu = new EmptyBaseCatalog(new CultureInfo("ru-RU"));

			Assert.AreEqual("Foo 1.23", catalogEn.GetParticularString("context", "Foo {0}", 1.23));
			Assert.AreEqual("Foo 1,23", catalogRu.GetParticularString("context", "Foo {0}", 1.23));
		}

		#endregion

		#region GetParticularPluralString

		[Test]
		public void TestGetParticularPluralString()
		{
			var t = new EmptyBaseCatalog(CultureInfo.CreateSpecificCulture("en-US"));
			t.Translations.Add("context1" + BaseCatalog.CONTEXT_GLUE + "key1-1", new[] { "value1-1", "value1-2" });
			t.Translations.Add("context2" + BaseCatalog.CONTEXT_GLUE + "key1-1", new[] { "value2-1", "value2-2" });

			Assert.AreEqual("value1-1", t.GetParticularPluralString("context1", "key1-1", "key1-2", 1));
			Assert.AreEqual("value1-2", t.GetParticularPluralString("context1", "key1-1", "key1-2", 2));
			Assert.AreEqual("value2-1", t.GetParticularPluralString("context2", "key1-1", "key1-2", 1));
			Assert.AreEqual("key1-1", t.GetParticularPluralString("context3", "key1-1", "key1-2", 1));
		}

		[Test]
		public void TestGetParticularPluralStringFormat()
		{
			var t = new EmptyBaseCatalog(CultureInfo.CreateSpecificCulture("en-US"));

			Assert.AreEqual("Foo bar", t.GetParticularPluralString("context", "Foo {0}", "Bar {0}", 1, "bar"));
			Assert.AreEqual("Bar bar", t.GetParticularPluralString("context", "Foo {0}", "Bar {0}", 2, "bar"));
			Assert.AreEqual("Foo bar baz", t.GetParticularPluralString("context", "Foo {0} {1}", "Bar {0} {1}", 1, "bar", "baz"));
			Assert.AreEqual("Bar bar baz", t.GetParticularPluralString("context", "Foo {0} {1}", "Bar {0} {1}", 2, "bar", "baz"));
		}

		[Test]
		public void TestGetParticularPluralStringFormatCulture()
		{
			var catalogEn = new EmptyBaseCatalog(new CultureInfo("en-US"));
			var catalogRu = new EmptyBaseCatalog(new CultureInfo("ru-RU"));

			Assert.AreEqual("Foo 1.23", catalogEn.GetParticularPluralString("context", "Foo {0}", "Foo {0}", (long)1.23, 1.23));
			Assert.AreEqual("Foo 1,23", catalogRu.GetParticularPluralString("context", "Foo {0}", "Foo {0}", (long)1.23, 1.23));
		}

		#endregion

		#region *Default methods

		[Test]
		public void TestProtectedGetString()
		{
			var t = new EmptyBaseCatalog();
			t.Translations.Add("key1", new[] { "value1" });

			Assert.AreEqual("value1", t.GetStringDefault("key1", null));
			Assert.AreEqual(null, t.GetStringDefault("key2", null));
			Assert.AreEqual("defaultMessage", t.GetStringDefault("key2", "defaultMessage"));
		}

		[Test]
		public void TestProtectedGetPluralString()
		{
			var t = new EmptyBaseCatalog(CultureInfo.CreateSpecificCulture("en-US"));
			t.Translations.Add("key1", new[] { "value1" });

			Assert.AreEqual("value1", t.GetPluralStringDefault("key1", "defaultSingular", "defaultPlural", 1));
			Assert.AreEqual("defaultPlural", t.GetPluralStringDefault("key1", "defaultSingular", "defaultPlural", 2));
			Assert.AreEqual("defaultSingular", t.GetPluralStringDefault("key2", "defaultSingular", "defaultPlural", 1));
			Assert.AreEqual("defaultPlural", t.GetPluralStringDefault("key2", "defaultSingular", "defaultPlural", 2));
		}

		[Test]
		public void TestProtectedGetTranslations()
		{
			var t = new EmptyBaseCatalog();
			t.Translations.Add("key1", new[] { "value1" });
			t.Translations.Add("key2", new[] { "value2" });
			t.Translations.Add("key3", new[] { "value3plural1", "value3plural2" });

			Assert.AreEqual(new[] { "value1" }, t.GetTranslations("key1"));
			Assert.AreEqual(new[] { "value2" }, t.GetTranslations("key2"));
			Assert.AreEqual(new[] { "value3plural1", "value3plural2" }, t.GetTranslations("key3"));
			Assert.AreEqual(null, t.GetTranslations("key4"));
		}

		#endregion
	}
}