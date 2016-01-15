using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;
using NGettext.Loaders;

namespace NGettext.Tests.Loaders
{
	public class MoFileParserTest
	{
		public string LocalesDir;
		
		public MoFileParserTest()
		{
			this.LocalesDir = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("TestResources", "locales"));
#if DNXCORE50
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
		}

		[Fact]
		public void TestParsing()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test.mo")))))
			{
				var parser = new MoFileParser();
				var parsedFile = parser.Parse(stream);
				this._TestLoadedTranslation(parsedFile.Translations);
			}
		}

		[Fact]
		public void TestBigEndianParsing()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test_BigEndian.mo")))))
			{
				var parser = new MoFileParser();
				var parsedFile = parser.Parse(stream);
				this._TestLoadedTranslation(parsedFile.Translations);
			}
		}

		[Fact]
		public void TestAutoEncoding()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test_KOI8-R.mo")))))
			{
				var parser = new MoFileParser();
				var parsedFile = parser.Parse(stream);
				this._TestLoadedTranslation(parsedFile.Translations);
			}
		}

		[Fact]
		public void TestManualEncoding()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test_KOI8-R.mo")))))
			{
				var parser = new MoFileParser(Encoding.GetEncoding("KOI8-R"), false);
				var parsedFile = parser.Parse(stream);
				this._TestLoadedTranslation(parsedFile.Translations);
			}
		}

		private void _TestLoadedTranslation(IDictionary<string, string[]> dict)
		{
			Assert.Equal(new[] { "тест" }, dict["test"]);
			Assert.Equal(new[] { "тест2" }, dict["test2"]);
			Assert.Equal(new[] { "{0} минута", "{0} минуты", "{0} минут" }, dict["{0} minute"]);
			Assert.Equal(new[] { "тест3контекст1" }, dict["context1" + Catalog.CONTEXT_GLUE + "test3"]);
			Assert.Equal(new[] { "тест3контекст2" }, dict["context2" + Catalog.CONTEXT_GLUE + "test3"]);
		}
	}
}