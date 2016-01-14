using System.Globalization;
using System.IO;
using Xunit;

namespace NGettext.Tests
{
	public class CatalogTest
	{
		public string LocalesDir;

		public CatalogTest()
		{
			this.LocalesDir = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("TestResources", "locales"));
		}

		[Fact]
		public void TestEmpty()
		{
			var t = new Catalog();

			Assert.Equal(0, t.Translations.Count);
			Assert.Equal(CultureInfo.CurrentUICulture, t.CultureInfo);

			t = new Catalog(new CultureInfo("fr"));
			Assert.Equal(new CultureInfo("fr"), t.CultureInfo);
		}

		[Fact]
		public void TestStream()
		{
			using (var stream = File.OpenRead(Path.Combine(this.LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test.mo")))))
			{
				var t = new Catalog(stream, new CultureInfo("ru-RU"));
				this._TestLoadedTranslation(t);
			}
		}

		[Fact]
		public void TestLocaleDir()
		{
			var t = new Catalog("Test", this.LocalesDir, new CultureInfo("ru-RU"));
			this._TestLoadedTranslation(t);
		}

		private void _TestLoadedTranslation(ICatalog t)
		{
			Assert.Equal("тест", t.GetString("test"));
			Assert.Equal("тест2", t.GetString("test2"));
			Assert.Equal("1 минута", t.GetPluralString("{0} minute", "{0} minutes", 1, 1));
			Assert.Equal("5 минут", t.GetPluralString("{0} minute", "{0} minutes", 5, 5));

			Assert.Equal("тест3контекст1", t.GetParticularString("context1", "test3"));
			Assert.Equal("тест3контекст2", t.GetParticularString("context2", "test3"));
		}

	}
}