NGettext [![Build Status](https://travis-ci.org/neris/NGettext.svg?branch=master)](https://travis-ci.org/neris/NGettext) [![Build Status](https://ci.appveyor.com/api/projects/status/oc151pvllqqy0po9?svg=true)](https://ci.appveyor.com/project/neris/ngettext)
========

Copyright (C) 2012-2016 Neris Ereptoris <http://neris.ws/>.


Just another one GNU/Gettext implementation for .NET.
Works fine on Microsoft .NET Framework version 2.0 or higher, Mono and even on .NET Core.

This implementation loads translations directly from gettext *.mo files (no need to compile a satellite assembly) and can handle multiple translation domains and multiple locales in one application instance.
It supports both little-endian and big-endian MO files, automatic (header-based) encoding detection.

By default, NGettext uses pre-compiled plural form rules for most known locales.
You can enable plural form rule parsing from *.mo file headers or use a custom plural rules (see description below).



Why not others?
---------------

**Why not Mono.Unix.Catalog?**
Mono's Catalog is just a bindings to three native functions (bindtextdomain, gettext, and ngettext). It does not support multiple domains/locales and contexts. It is not cross-patform, it have problems with Windows OS.


**Why not GNU.Gettext?**
It uses satellite assemblies as a translation files and does not support multiple locales in one application instance.
It's hard to build and maintain translation files and change locale inside your application.

**So why NGettext?**
* NGettext is fully cross-platform as it doesn't use any native or managed 3rd-party libraries.
* NGettext supports multiple domains. You can separate translation files for each of your application's module or plugin.
* NGettext supports multiple locales in one application instance and gives really simple API to choose locale of your application.
  You don't even need to care about locales of your application's threads.
* NGettext loads translations from *.mo files. You can even load translations from specified file or stream.
* NGettext supports message contexts.
* NGettext provides nice and simple API for translation.



Build status
------------

|OS |Target frameworks (build)|Target frameworks (test)|Status|
|:--|:--|:--|:--|
|Windows|net20 net35 net40 net45 net46<br/>net35-client net40-client<br/>dnx451<br/>dnxcore50 uap10.0|dnx451|[![Build Status](https://ci.appveyor.com/api/projects/status/oc151pvllqqy0po9?svg=true)](https://ci.appveyor.com/project/neris/ngettext)|
|Linux|dnx451-mono|dnx451-mono|[![Build Status](https://travis-ci.org/neris/NGettext.svg?branch=master)](https://travis-ci.org/neris/NGettext)|



Installation and usage
----------------------

All you need to do is just install a [NuGet package](https://www.nuget.org/packages/NGettext/)
from the package manager console:
```
PM> Install-Package NGettext
```
or through DNX Utility:
```
$ dnu install NGettext
```

Now you can use NGettext in your code:
```csharp
	using NGettext;
```
```csharp
	// This will load translations from "./locale/<CurrentUICulture>/LC_MESSAGES/Example.mo"
	ICatalog catalog = new Catalog("Example", "./locale");
	
	// or
	
	// This will load translations from "./locale/ru_RU/LC_MESSAGES/Example.mo"
	ICatalog catalog = new Catalog("Example", "./locale", new CultureInfo("ru-RU"));
```
```csharp
	Console.WriteLine(catalog.GetString("Hello, World!")); // will translate "Hello, World!" using loaded translations
	Console.WriteLine(catalog.GetString("Hello, {0}!", "World")); // String.Format support
```

### .NET CoreCLR

If you using this library under CoreCLR and you want to use encodings different from UTF-8 for your *.mo files, you need to include [System.Text.Encoding.CodePages](https://www.nuget.org/packages/System.Text.Encoding.CodePages/) package into your application and initialize it like this:
```csharp
	#if DNXCORE50
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	#endif
```


### Culture-specific message formatting

```csharp
	// All translation methods support String.Format optional arguments
	catalog.GetString("Hello, {0}!", "World");
	
	// Catalog's current locale will be used to format messages correctly
	catalog.GetString("Here's a number: {0}!", 1.23);
	// Will return "Here's a number: 1.23!" for en_US locale
	// But something like this will be returned for ru_RU locale with Russian translation: "А вот и номер: 1,23!"
```



### Plural forms

```csharp
	catalog.GetPluralString("You have {0} apple.", "You have {0} apples.", count);
	// Returns (for en_US locale):
	//     "You have {0} apple." for count = 1
	//     "You have {0} apples." otherwise


	catalog.GetPluralString("You have {0} apple.", "You have {0} apples.", 5, 5);
	// Returns translated plural massage: "You have 5 apples." (for en_US locale)
	// First “5” used in plural forms determination; second — in String.Format method


	// Example plural forms usage for fractional numbers:
	catalog.GetPluralString("You have {0} apple.", "You have {0} apples.", (long)1.23, 1.23);
	// Internal String.Format will be used in context of catalog's locale and formats objects respectively
```



### Contexts

```csharp
	catalog.GetParticularString("Menu|File|", "Open"); // will translate message "Open" using context "Menu|File|"
	catalog.GetParticularString("Menu|Project|", "Open"); // will translate message "Open" using context "Menu|Project|"
```



### Multiple locales and domains in one application instance

```csharp
	// "./locale/en_US/LC_MESSAGES/Example.mo"
	ICatalog example_en = new Catalog("Example", "./locale", new CultureInfo("en-US"));

	// "./locale/fr/LC_MESSAGES/Example.mo"
	ICatalog example_fr = new Catalog("Example", "./locale", new CultureInfo("fr"));

	// "./locale/<CurrentUICulture>/LC_MESSAGES/AnotherDomainName.mo"
	ICatalog anotherDomain = new Catalog("AnotherDomainName", "./locale");
```



### Direct MO file loading

```csharp
	Stream moFileStream = File.OpenRead("path/to/domain.mo");
	ICatalog catalog = new Catalog(moFileStream, new CultureInfo("en-US"));
```



### Parsing plural rules from the *.mo file header

To support this you can just create catalog using `MoAstPluralLoader`.
```csharp
	ICatalog catalog = new Catalog(new MoAstPluralLoader("Example", "./locale"));
```
Please note that this solution is slightly slower than NGettext default behavior even it's pretty well optimized.
However, in most cases, performance degradation will not be noticeable.



### Custom plural formulas

```csharp
	catalog.PluralRule = new PluralRule(numPlurals, n => ( n == 1 ? 0 : 1 ));
```
Also you can create custom plural rule generator by implementing IPluralRuleGenerator interface, which will create
a PluralRule for any culture.



Debugging
---------

Debug version of the NGettext binary outputs debug messages to System.Diagnostics.Trace.
You can register trace listeners to see NGettext debug messages.
Please note that Release version of the NGettext binary does not produse any trace messages.

```csharp
	Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
```



Shorter syntax
--------------

In `doc/examples/T.cs` you can see an example of shorter syntax creation for NGettext:
```csharp
	T._("Hello, World!"); // GetString
	T._n("You have {0} apple.", "You have {0} apples.", count, count); // GetPluralString
	T._p("Context", "Hello, World!"); // GetParticularString
	T._pn("Context", "You have {0} apple.", "You have {0} apples.", count, count); // GetParticularPluralString
```



Poedit compatibility
--------------------

For [Poedit](http://www.poedit.net/) compatibility, you need to specify plural form in your *.pot file header, even for english language:
```
	"Plural-Forms: nplurals=2; plural=n != 1;\n"
```

And a keywords list:
```
	"X-Poedit-KeywordsList: GetString;GetPluralString:1,2;GetParticularString:1c,2;GetParticularPluralString:1c,2,3;_;_n:1,2;_p:1c,2;_pn:1c,2,3\n"
```



