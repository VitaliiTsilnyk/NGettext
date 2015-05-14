NGettext [![Build Status](https://travis-ci.org/neris/NGettext.svg?branch=master)](https://travis-ci.org/neris/NGettext)
========

Copyright (C) 2012 Neris Ereptoris <http://neris.ws/>.


Just another one GNU/Gettext implementation for .NET.
Requires Microsoft .NET Framework 2.0 or higher or Mono.

This implementation loads translations directly from gettext *.mo files (no need to compile a satellite assembly) and can handle multiple translation domains and multiple locales in one application instance.

NGettext currently not supports *.mo file headers (stored in mo file plural formulas, encoding, etc.).
It uses precompiled plural formulas and supports custom plural formulas passed through API.
It supports both little-endian and big-endian MO files.



Why not others?
---------------

**Why not Mono.Unix.Catalog?**
Mono's Catalog is just a bindings to three native functions (bindtextdomain, gettext, and ngettext). It does not support multiple domains/locales and contexts. It is not cross-patform, it have problems with Windows OS.


**Why not GNU.Gettext?**
It uses satellite assemblies as a translation files and does not support multiple locales in one application instance.
It's hard to build and maintain translation files and change locale inside your application.

**So why NGettext?**
* NGettext is fully cross-platform as it don't uses any native or managed 3rd-party libraries.
* NGettext supports multiple domains. You can separate translation files for each of your application's module or plugin.
* NGettext supports multiple locales in one application instance and gives really simple API to choose locale of your application.
  You don't even need to care about locales of your application's threads.
* NGettext loads translations from *.mo files. You can even load translations from specified file or stream.
* NGettext supports message contexts.
* NGettext provides nice and simple API for translation.


Building from the sources
-------------------------

### Building on Linux
  
  Requirements:
    [Mono](http://www.go-mono.com/mono-downloads/download.html),
    [NAnt](http://nant.sourceforge.net/).
  
  Execute the `nant` command in the project directory to build project with the Release configuration.

### Building on Windows
  
  Requirements:
    Microsoft .NET Framework 2.0 or higher,
    [NAnt](http://nant.sourceforge.net/).
  
  Just run `build.bat` to build project with the Release configuration.



Installation and usage
----------------------

All you need to do is just install a [NuGet package](https://www.nuget.org/packages/NGettext/):
```
PM> Install-Package NGettext
```

Alternatively you can download [compiled binaries](https://github.com/neris/NGettext/releases) or the [source code](https://github.com/neris/NGettext) and add a reference to `NGettext.dll` or `NGettext.csproj` to your project.


Now you can use NGettext in your code:
```csharp
	using NGettext;
```
```csharp
	// This will load translations from "./locale/<CurrentUICulture>/LC_MESSAGES/Example.mo"
	ICatalog catalog = new Catalog("Example", "./locale");
	
	// or
	
	// This will load translations from "./locale/ru_RU/LC_MESSAGES/Example.mo"
	ICatalog catalog = new Catalog("Example", "./locale", CultureInfo.CreateSpecificCulture("ru-RU"));
```
```csharp
	Console.WriteLine(catalog.GetString("Hello, World!")); // will translate "Hello, World!" using loaded translations
	Console.WriteLine(catalog.GetString("Hello, {0}!", "World")); // String.Format support
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
```



### Contexts

```csharp
	catalog.GetParticularString("Menu|File|", "Open"); // will translate message "Open" using context "Menu|File|"
	catalog.GetParticularString("Menu|Project|", "Open"); // will translate message "Open" using context "Menu|Project|"
```



### Multiple locales and domains in one application instance

```csharp
	// "./locale/en_US/LC_MESSAGES/Example.mo"
	ICatalog example_en = new Catalog("Example", "./locale", CultureInfo.CreateSpecificCulture("en-US"));

	// "./locale/fr/LC_MESSAGES/Example.mo"
	ICatalog example_fr = new Catalog("Example", "./locale", CultureInfo.CreateSpecificCulture("fr"));

	// "./locale/<CurrentUICulture>/LC_MESSAGES/AnotherDomainName.mo"
	ICatalog anotherDomain = new Catalog("AnotherDomainName", "./locale");
```



### Direct MO file loading

```csharp
	Stream moFileStream = File.OpenRead("path/to/domain.mo");
	ICatalog catalog = new Catalog(moFileStream);
```



### Custom plural formulas

```csharp
	catalog.PluralForm.SetCustomFormula(cultureInfo, n => ( n == 1 ? 0 : 1 ));
```



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



