using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace NGettext.Plural
{
	/// <summary>
	/// Plural forms processor.
	/// </summary>
	public class PluralFormProcessor
	{
		/// <summary>
		/// Returns plural form index by given number.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public delegate int FormulaDelegate(long n);

		/// <summary>
		/// Default plural forms processor.
		/// </summary>
		public static readonly PluralFormProcessor Default = new PluralFormProcessor();

		/// <summary>
		/// Collection with custom plural forms formulas.
		/// Culture Info LCID => Formula function
		/// </summary>
		private readonly Dictionary<int, FormulaDelegate> _CustomFormulas = new Dictionary<int, FormulaDelegate>();

		/// <summary>
		/// Set custom plural form formula function for given locale.
		/// </summary>
		/// <param name="cultureInfo">Locale</param>
		/// <param name="formula">Function that returns plural form index by given number.</param>
		public void SetCustomFormula(CultureInfo cultureInfo, FormulaDelegate formula)
		{
			this._CustomFormulas.Add(cultureInfo.LCID, formula);
		}

		/// <summary>
		/// Get plural form index for given locale by given number.
		/// </summary>
		/// <remarks>
		///	Default furmulas created using information from http://cldr.unicode.org/
		/// </remarks>
		/// <param name="cultureInfo"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		public int GetPluralFormIndex(CultureInfo cultureInfo, long number)
		{
			Trace.WriteLine(String.Format("Getting plural index for locale \"{0}\" and number \"{1}\".", cultureInfo.Name, number), "NGettext");

			if (this._CustomFormulas.ContainsKey(cultureInfo.LCID))
			{
				var formula = this._CustomFormulas[cultureInfo.LCID];
				if (formula != null)
				{
					Trace.WriteLine(String.Format("Using custom plural formula for locale \"{0}\".", cultureInfo.Name), "NGettext");
					return formula.Invoke(number);
				}
			}

			var langCode = cultureInfo.TwoLetterISOLanguageName;
			Trace.WriteLine(String.Format("Using built-in plural formula with langcode \"{0}\" for locale \"{1}\".", langCode, cultureInfo.Name), "NGettext");


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
					return 0;

				case "ar":
					return (number == 0) ? 0 : ((number == 1) ? 1 : ((number == 2) ? 2 : (((number >= 3) && (number <= 10)) ? 3 : (((number >= 11) && (number <= 99)) ? 4 : 5))));

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
					return (number == 1) ? 0 : 1;

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
					return ((number == 0) || (number == 1)) ? 0 : 1;

				case "lv":
					return (number == 0) ? 0 : (((number % 10 == 1) && (number % 100 != 11)) ? 1 : 2);

				case "iu":
				case "kw":
				case "naq":
				case "se":
				case "sma":
				case "smi":
				case "smj":
				case "smn":
				case "sms":
					return (number == 1) ? 0 : ((number == 2) ? 1 : 2);

				case "ga":
					return (number == 1) ? 0 : ((number == 2) ? 1 : (((number >= 3) && (number <= 6)) ? 2 : ((number >= 7 && number <= 10) ? 3 : 4)));

				case "ro":
				case "mo":
					return (number == 1) ? 0 : (((number == 0) || ((number % 100 > 0) && (number % 100 < 20))) ? 1 : 2);

				case "lt":
					return ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2);

				case "be":
				case "bs":
				case "hr":
				case "ru":
				case "sh":
				case "sr":
				case "uk":
					return ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2);

				case "cs":
				case "sk":
					return (number == 1) ? 0 : (((number >= 2) && (number <= 4)) ? 1 : 2);

				case "pl":
					return (number == 1) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 12) || (number % 100 > 14))) ? 1 : 2);

				case "sl":
					return (number % 100 == 1) ? 0 : ((number % 100 == 2) ? 1 : (((number % 100 == 3) || (number % 100 == 4)) ? 2 : 3));

				case "mt":
					return (number == 1) ? 0 : (((number == 0) || ((number % 100 > 1) && (number % 100 < 11))) ? 1 : (((number % 100 > 10) && (number % 100 < 20)) ? 2 : 3));

				case "mk":
					return ((number % 10 == 1) && (number != 11)) ? 0 : 1;

				case "cy":
					return (number == 0) ? 0 : ((number == 1) ? 1 : ((number == 2) ? 2 : ((number == 3) ? 3 : ((number == 6) ? 4 : 5))));

				case "lag":
				case "ksh":
					return (number == 0) ? 0 : ((number == 1) ? 1 : 2);

				case "shi":
					return ((number == 0) && (number == 1)) ? 0 : (((number >= 2) && (number <= 10)) ? 1 : 2);

				case "tzm":
					return ((number == 0) || (number == 1) || (((number >= 11) && (number <= 99)))) ? 0 : 1;

				case "gv":
					return ((number % 10 == 1) || (number % 10 == 2) || (number % 20 == 0)) ? 0 : 1;

				case "gd":
					return ((number == 1) || (number == 11)) ? 0 : (((number == 2) || (number == 12)) ? 1 : (((((number >= 3) && (number <= 10)) || ((number >= 13) && (number <= 19))) ? 2 : 3)));

				default:
					return 0;
			}
		}
	}
}