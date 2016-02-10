using System;
using System.Collections.Generic;
using NGettext.Plural;
using NGettext.Plural.Ast;
using Xunit;

namespace NGettext.Tests.Plural.Ast
{
    public class AstTokenParserTest
    {
		public static IEnumerable<object[]> GetParseRulesTestData()
		{
			// Basic calculations
			yield return new object[] { "addition"         , "2 + 3", 5 };
            yield return new object[] { "substraction"     , "3 - 2", 1 };
            yield return new object[] { "multiplication"   , "2 * 3", 6 };
            yield return new object[] { "division"         , "6 / 3", 2 };
            yield return new object[] { "integer-division" , "7 / 4", 1 };
            yield return new object[] { "modulo"           , "7 % 4", 3 };
            // Boolean NOT
            yield return new object[] { "boolean-not-0"  , "!0", 1 };
            yield return new object[] { "boolean-not-1"  , "!1", 0 };
            yield return new object[] { "boolean-not-15" , "!1", 0 };
            // Equal operators
            yield return new object[] { "equal-true"      , "5 == 5", 1 };
            yield return new object[] { "equal-false"     , "5 == 4", 0 };
            yield return new object[] { "not-equal-true"  , "5 != 5", 0 };
            yield return new object[] { "not-equal-false" , "5 != 4", 1 };
            // Compare operators
            yield return new object[] { "less-than-true"         , "5 > 4", 1 };
            yield return new object[] { "less-than-false"        , "5 > 5", 0 };
            yield return new object[] { "less-or-equal-true"     , "5 >= 5", 1 };
            yield return new object[] { "less-or-equal-false"    , "5 >= 6", 0 };
            yield return new object[] { "greater-than-true"      , "5 < 6", 1 };
            yield return new object[] { "greater-than-false"     , "5 < 5", 0 };
            yield return new object[] { "greater-or-equal-true"  , "5 <= 5", 1 };
            yield return new object[] { "greater-or-equal-false" , "5 <= 4", 0 };
            // Boolean operators
            yield return new object[] { "boolean-and-true"  , "1 && 1", 1 };
            yield return new object[] { "boolean-and-false" , "1 && 0", 0 };
            yield return new object[] { "boolean-or-true"   , "1 || 0", 1 };
            yield return new object[] { "boolean-or-false"  , "0 || 0", 0 };
            // Variable injection
            yield return new object[] { "variable-injection" , "n", 0 };
		}

		[Fact]
		public void ParseRulesTest()
		{
			var parser = new AstTokenParser();
			foreach (object[] mapping in GetParseRulesTestData())
			{
				var astRoot = parser.Parse((string)mapping[1]);
				var rule = new AstPluralRule(100, astRoot);
				Assert.Equal((int)mapping[2], rule.Evaluate(0));
			}
		}

		public static IEnumerable<object[]> GetParseCompleteRulesTestData()
		{
			// Taken from original gettext tests
			yield return new object[] {
				"n != 1",
				"10111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"
			};
			yield return new object[] {
				"n>1",
				"00111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"
			};
			yield return new object[] {
				"n==1 ? 0 : n==2 ? 1 : 2",
				"20122222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222"
			};
			yield return new object[] {
				"n==1 ? 0 : (n==0 || (n%100 > 0 && n%100 < 20)) ? 1 : 2",
				"10111111111111111111222222222222222222222222222222222222222222222222222222222222222222222222222222222111111111111111111122222222222222222222222222222222222222222222222222222222222222222222222222222222"
			};
			yield return new object[] {
				"n%10==1 && n%100!=11 ? 0 : n%10>=2 && (n%100<10 || n%100>=20) ? 1 : 2",
				"20111111112222222222201111111120111111112011111111201111111120111111112011111111201111111120111111112011111111222222222220111111112011111111201111111120111111112011111111201111111120111111112011111111"
			};
			yield return new object[] {
				"n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2",
				"20111222222222222222201112222220111222222011122222201112222220111222222011122222201112222220111222222011122222222222222220111222222011122222201112222220111222222011122222201112222220111222222011122222"
			};
			yield return new object[] {
				"n%100/10==1 ? 2 : n%10==1 ? 0 : (n+9)%10>3 ? 2 : 1",
				"20111222222222222222201112222220111222222011122222201112222220111222222011122222201112222220111222222011122222222222222220111222222011122222201112222220111222222011122222201112222220111222222011122222"
			};
			yield return new object[] {
				"n==1 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2",
				"20111222222222222222221112222222111222222211122222221112222222111222222211122222221112222222111222222211122222222222222222111222222211122222221112222222111222222211122222221112222222111222222211122222"
			};
			yield return new object[] {
				"n%100==1 ? 0 : n%100==2 ? 1 : n%100==3 || n%100==4 ? 2 : 3",
				"30122333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333012233333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333"
			};
		}

		[Fact]
		public void ParseCompleteRulesTest()
		{
			var parser = new AstTokenParser();
			foreach (object[] mapping in GetParseCompleteRulesTestData())
			{
				var astRoot = parser.Parse((string)mapping[0]);
				var rule = new AstPluralRule(100, astRoot);
				var expectedSeq = (string)mapping[1];

				for (int i = 0; i < 200; i++)
				{
					var expected = int.Parse(expectedSeq[i].ToString());
					Assert.Equal(expected, rule.Evaluate(i));
				}
			}
		}
	}
}
