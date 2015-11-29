using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using System.Reflection;

using NUnit.Framework;

namespace NGettext.Tests
{
	[TestFixture]
	public class CatalogTest
	{
		public string LocalesDir;

		[SetUp]
		public void Init()
		{
			this.LocalesDir = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("TestResources", "locales"));
		}

		[Test]
		public void TestEmpty()
		{
			var t = new Catalog();

			Assert.AreEqual(0, t.Translations.Count);
			Assert.AreEqual(CultureInfo.CurrentUICulture, t.CultureInfo);

			t = new Catalog(new CultureInfo("fr"));
			Assert.AreEqual(new CultureInfo("fr"), t.CultureInfo);
		}

		[Test]
		public void TestStream()
		{
			using (var stream = File.OpenRead(Path.Combine(this.LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test.mo")))))
			{
				var t = new Catalog(stream, new CultureInfo("ru-RU"));
				this._TestLoadedTranslation(t);
			}
		}

		[Test]
		public void TestLocaleDir()
		{
			var t = new Catalog("Test", this.LocalesDir, new CultureInfo("ru-RU"));
			this._TestLoadedTranslation(t);
		}

		private void _TestLoadedTranslation(ICatalog t)
		{
			Assert.AreEqual("тест", t.GetString("test"));
			Assert.AreEqual("тест2", t.GetString("test2"));
			Assert.AreEqual("1 минута", t.GetPluralString("{0} minute", "{0} minutes", 1, 1));
			Assert.AreEqual("5 минут", t.GetPluralString("{0} minute", "{0} minutes", 5, 5));

			Assert.AreEqual("тест3контекст1", t.GetParticularString("context1", "test3"));
			Assert.AreEqual("тест3контекст2", t.GetParticularString("context2", "test3"));
		}

	}
}