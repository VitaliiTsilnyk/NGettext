using System;

namespace NGettext
{
	/// <summary>
	/// Represents a Gettext catalog instance.
	/// </summary>
	public interface ITranslator
	{
		/// <summary>
		/// Returns <paramref name="text"/> translated into the selected language.
		/// Similar to <c>gettext</c> function.
		/// </summary>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
		string GetString(string text);

		/// <summary>
		/// Returns <paramref name="text"/> translated into the selected language.
		/// Similar to <c>gettext</c> function.
		/// </summary>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		string GetString(string text, params object[] args);
		
		/// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/>.
		/// Similar to <c>ngettext</c> function.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
		string GetPluralString(string text, string pluralText, long n);

		/// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/>.
		/// Similar to <c>ngettext</c> function.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		string GetPluralString(string text, string pluralText, long n, params object[] args);

		/// <summary>
		/// Returns <paramref name="text"/> translated into the selected language using given <paramref name="context"/>.
		/// Similar to <c>pgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
		string GetParticularString(string context, string text);

		/// <summary>
		/// Returns <paramref name="text"/> translated into the selected language using given <paramref name="context"/>.
		/// Similar to <c>pgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		string GetParticularString(string context, string text, params object[] args);

		/// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/> using given <paramref name="context"/>.
		/// Similar to <c>npgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
		string GetParticularPluralString(string context, string text, string pluralText, long n);

		/// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/> using given <paramref name="context"/>.
		/// Similar to <c>npgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		string GetParticularPluralString(string context, string text, string pluralText, long n, params object[] args);

		#region Aliases
// ReSharper disable InconsistentNaming

		/// <summary>
		/// Alias for <see cref="GetString(string)"/> method.
		/// </summary>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
		string _(string text);

		/// <summary>
		/// Alias for <see cref="GetString(string, object[])"/> method.
		/// </summary>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		string _(string text, params object[] args);

		/// <summary>
		/// Alias for <see cref="GetPluralString(string,string,long)"/> method.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
		string _n(string text, string pluralText, long n);

		/// <summary>
		/// Alias for <see cref="GetPluralString(string,string,long, object[])"/> method.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		string _n(string text, string pluralText, long n, params object[] args);

		/// <summary>
		/// Alias for <see cref="GetParticularString(string,string)"/> method.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
		string _p(string context, string text);
		
		/// <summary>
		/// Alias for <see cref="GetParticularString(string,string, object[])"/> method.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		string _p(string context, string text, params object[] args);

		/// <summary>
		/// Alias for <see cref="GetParticularPluralString(string,string,string,long)"/> method.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
		string _pn(string context, string text, string pluralText, long n);

		/// <summary>
		/// Alias for <see cref="GetParticularPluralString(string,string,string,long, object[])"/> method.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		string _pn(string context, string text, string pluralText, long n, params object[] args);

// ReSharper restore InconsistentNaming
		#endregion
	}
}