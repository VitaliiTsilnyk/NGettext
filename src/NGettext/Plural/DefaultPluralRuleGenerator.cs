using System;
using System.Diagnostics;
using System.Globalization;

namespace NGettext.Plural
{
	/// <summary>
	/// Default plural rule generator.
	/// </summary>
	public class DefaultPluralRuleGenerator : IPluralRuleGenerator
	{
		/// <summary>
		/// Creates a plural rule for given culture.
		/// </summary>
		/// <remarks>
		///	Default furmulas created using information from http://cldr.unicode.org/
		/// </remarks>
		/// <param name="cultureInfo"></param>
		/// <returns></returns>
		public virtual IPluralRule CreateRule(CultureInfo cultureInfo)
		{

			var langCode = cultureInfo.TwoLetterISOLanguageName;
			Trace.WriteLine(String.Format("Creating a built-in plural rule for langcode \"{0}\" for locale \"{1}\".", langCode, cultureInfo.Name), "NGettext");

			switch (langCode)
			{
				case "az":
				case "bm":
				case "bo":
				case "dz":
				case "fa":
				case "id":
				case "ig":
				case "ii":
				case "hu":
				case "ja":
				case "jv":
				case "ka":
				case "kde":
				case "kea":
				case "km":
				case "kn":
				case "ko":
				case "lo":
				case "ms":
				case "my":
				case "sah":
				case "ses":
				case "sg":
				case "th":
				case "to":
				case "tr":
				case "vi":
				case "wo":
				case "yo":
				case "zh":
					return new PluralRule(1, number => 0);

				case "ar":
					return new PluralRule(6, number => (number == 0) ? 0 : ((number == 1) ? 1 : ((number == 2) ? 2 : (((number >= 3) && (number <= 10)) ? 3 : (((number >= 11) && (number <= 99)) ? 4 : 5)))));

				case "asa":
				case "af":
				case "bem":
				case "bez":
				case "bg":
				case "bn":
				case "brx":
				case "ca":
				case "cgg":
				case "chr":
				case "da":
				case "de":
				case "dv":
				case "ee":
				case "el":
				case "en":
				case "eo":
				case "es":
				case "et":
				case "eu":
				case "fi":
				case "fo":
				case "fur":
				case "fy":
				case "gl":
				case "gsw":
				case "gu":
				case "ha":
				case "haw":
				case "he":
				case "is":
				case "it":
				case "jmc":
				case "kaj":
				case "kcg":
				case "kk":
				case "kl":
				case "ksb":
				case "ku":
				case "lb":
				case "lg":
				case "mas":
				case "ml":
				case "mn":
				case "mr":
				case "nah":
				case "nb":
				case "nd":
				case "ne":
				case "nl":
				case "nn":
				case "no":
				case "nr":
				case "ny":
				case "nyn":
				case "om":
				case "or":
				case "pa":
				case "pap":
				case "ps":
				case "pt":
				case "rof":
				case "rm":
				case "rwk":
				case "saq":
				case "seh":
				case "sn":
				case "so":
				case "sq":
				case "ss":
				case "ssy":
				case "st":
				case "sv":
				case "sw":
				case "syr":
				case "ta":
				case "te":
				case "teo":
				case "tig":
				case "tk":
				case "tn":
				case "ts":
				case "ur":
				case "wae":
				case "ve":
				case "vun":
				case "xh":
				case "xog":
				case "zu":
					return new PluralRule(2, number => (number == 1) ? 0 : 1);

				case "ak":
				case "am":
				case "bh":
				case "fil":
				case "ff":
				case "fr":
				case "guw":
				case "hi":
				case "kab":
				case "ln":
				case "mg":
				case "nso":
				case "ti":
				case "wa":
					return new PluralRule(2, number => ((number == 0) || (number == 1)) ? 0 : 1);

				case "lv":
					return new PluralRule(3, number => (number == 0) ? 0 : (((number % 10 == 1) && (number % 100 != 11)) ? 1 : 2));

				case "iu":
				case "kw":
				case "naq":
				case "se":
				case "sma":
				case "smi":
				case "smj":
				case "smn":
				case "sms":
					return new PluralRule(3, number => (number == 1) ? 0 : ((number == 2) ? 1 : 2));

				case "ga":
					return new PluralRule(5, number => (number == 1) ? 0 : ((number == 2) ? 1 : (((number >= 3) && (number <= 6)) ? 2 : ((number >= 7 && number <= 10) ? 3 : 4))));
					
				case "ro":
				case "mo":
					return new PluralRule(3, number => (number == 1) ? 0 : (((number == 0) || ((number % 100 > 0) && (number % 100 < 20))) ? 1 : 2));
					
				case "lt":
					return new PluralRule(3, number => ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2));
					
				case "be":
				case "bs":
				case "hr":
				case "ru":
				case "sh":
				case "sr":
				case "uk":
					return new PluralRule(3, number => ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2));
					
				case "cs":
				case "sk":
					return new PluralRule(3, number => (number == 1) ? 0 : (((number >= 2) && (number <= 4)) ? 1 : 2));

				case "pl":
					return new PluralRule(3, number => (number == 1) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 12) || (number % 100 > 14))) ? 1 : 2));
					
				case "sl":
					return new PluralRule(4, number => (number % 100 == 1) ? 0 : ((number % 100 == 2) ? 1 : (((number % 100 == 3) || (number % 100 == 4)) ? 2 : 3)));
					
				case "mt":
					return new PluralRule(4, number => (number == 1) ? 0 : (((number == 0) || ((number % 100 > 1) && (number % 100 < 11))) ? 1 : (((number % 100 > 10) && (number % 100 < 20)) ? 2 : 3)));
					
				case "mk":
					return new PluralRule(2, number => ((number % 10 == 1) && (number != 11)) ? 0 : 1);

				case "cy":
					return new PluralRule(6, number => (number == 0) ? 0 : ((number == 1) ? 1 : ((number == 2) ? 2 : ((number == 3) ? 3 : ((number == 6) ? 4 : 5)))));
					
				case "lag":
				case "ksh":
					return new PluralRule(3, number => (number == 0) ? 0 : ((number == 1) ? 1 : 2));

				case "shi":
					return new PluralRule(3, number => ((number == 0) && (number == 1)) ? 0 : (((number >= 2) && (number <= 10)) ? 1 : 2));

				case "tzm":
					return new PluralRule(2, number => ((number == 0) || (number == 1) || (((number >= 11) && (number <= 99)))) ? 0 : 1);

				case "gv":
					return new PluralRule(2, number => ((number % 10 == 1) || (number % 10 == 2) || (number % 20 == 0)) ? 0 : 1);

				case "gd":
					return new PluralRule(4, number => ((number == 1) || (number == 11)) ? 0 : (((number == 2) || (number == 12)) ? 1 : (((((number >= 3) && (number <= 10)) || ((number >= 13) && (number <= 19))) ? 2 : 3))));
			}
			return new PluralRule(1, number => 0);
		}
	}
}