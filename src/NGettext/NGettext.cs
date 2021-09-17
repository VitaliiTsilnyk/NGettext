using NGettext.Loaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace NGettext
{
    /// <summary>
    /// Syntactic sugar for NGettext, to provide a similar
    /// style of usage to GNU gettext in C/C++ for calling the l10n methods.
    /// 
    /// Note that this sugar creates a "default" Catalog that
    /// you load your mo file into using the "InitGettext()" method
    /// 
    /// If you still need (or want) to use multiple catalogs, continue using catalog instances
    /// 
    /// In general, this uses a moddified form of the '_()' notation described by GNU
    /// _() will be simple singular and plural form retrieval
    /// _c() adds context to the singular or plural form
    /// _f() adds formatting to the singular or plural form
    /// _cf() adds both context and formatting to the singular or plural form
    /// </summary>
    public static class NGettext
    {
        private static string domain;
        private static string locale;

        private static ICatalog catalog;

        /// <summary>
        /// Default Catalog
        /// </summary>
        private static ICatalog Catalog
        {
            get
            {
                if (catalog == default)
                {
                    domain = "";
                    locale = "";
                    catalog = new Catalog();
                    // Should probably throw some kind of error here too!
                }
                return catalog;
            }
        }

        /// <summary>
        /// Initiallize or reinitialize this default catalog
        /// </summary>
        /// <param name="newDomain">Catalog domain name.</param>
        /// <param name="newLocale">Directory that contains gettext localization files.</param>
        public static void InitGettext(string newDomain, string newLocale)
        {
            if ((locale != newLocale) || (domain != newDomain))
            {
                domain = newDomain;
                locale = newLocale;

                var moloader = new MoAstPluralLoader(domain, locale);
                catalog = new Catalog(moloader);
            }
        }

// dotnet doesn't like the traditional gettext _() functions, so let's disable that warning
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS3008 // Identifier is not CLS-compliant

        /// <summary>
        /// Returns <paramref name="text"/> translated into the selected language.
        /// Similar to <c>gettext</c> function.
        /// </summary>
        /// <param name="text">Text to translate.</param>
        /// <returns>Translated text.</returns>
        public static string _(string text) => Catalog.GetString(text);

        /// <summary>
        /// Returns <paramref name="text"/> translated into the selected language.
        /// Similar to <c>gettext</c> function.
        /// </summary>
        /// <param name="text">Text to translate.</param>
        /// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
        /// <returns>Translated text.</returns>
        public static string _f(string text, params object[] args) => Catalog.GetString(text, args);

        /// <summary>
		/// Returns <paramref name="text"/> translated into the selected language using given <paramref name="context"/>.
		/// Similar to <c>pgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
        public static string _c(string context, string text) => Catalog.GetParticularString(context, text);

        /// <summary>
		/// Returns <paramref name="text"/> translated into the selected language using given <paramref name="context"/>.
		/// Similar to <c>pgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
        public static string _cf(string context, string text, params object[] args) => Catalog.GetParticularString(context, text, args);

        /// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/>.
		/// Similar to <c>ngettext</c> function.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
        public static string _(string text, string pluralText, long n) => Catalog.GetPluralString(text, pluralText, n);

        /// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/>.
		/// Similar to <c>ngettext</c> function.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
        public static string _f(string text, string pluralText, long n, params object[] args) => Catalog.GetPluralString(text, pluralText, n, args);

        /// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/> using given <paramref name="context"/>.
		/// Similar to <c>npgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
        public static string _c(string context, string text, string pluralText, long n) => Catalog.GetParticularPluralString(context, text, pluralText, n);

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
        public static string _cf(string context, string text, string pluralText, long n, params object[] args) => Catalog.GetParticularPluralString(context, text, pluralText, n, args);

#pragma warning restore CS3008 // Identifier is not CLS-compliant
#pragma warning restore IDE1006 // Naming Styles

    }
}
